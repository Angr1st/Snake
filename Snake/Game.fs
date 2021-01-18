namespace Snake

open Snake.Lib
open System
open System.Diagnostics
open System.Threading
open Snake.GameConstants
open Snake.SnakeLogic
open Snake.ScoreBoardLogic
open Snake.GameFieldLogic
open System.Windows.Forms

module Game =
    
    let IsActive gameState =
        match gameState with
        | Init | Running -> true
        | _ -> false

    let Clean matchFieldState =
        matchFieldState.MatchField.SetAllFields Empty
        
    let WriteSnake matchFieldState snake =
        matchFieldState.MatchField.[snake.Head.X, snake.Head.Y] <- ToGameFieldType snake
        snake.SnakeElements
        |> List.iter (fun body -> matchFieldState.MatchField.[body.X, body.Y] <- SnakeBody body)

    let WriteApple matchFieldState (apple:StaticField) =
        matchFieldState.MatchField.[apple.X, apple.Y] <- Apple apple

    let WriteGameFinish matchFieldState gameState =
        let middleRowIndex = 10    
        let writeMessage (text:string) =
            let writeChar index cha =
                matchFieldState.MatchField.[ 9 + index, middleRowIndex] <- ScoreField cha
                index + 1
                    
            text.ToCharArray()
            |> Array.fold writeChar 0 
            |> ignore
    
        match gameState with
        | Won score ->
            writeMessage <| sprintf "Won %i" score 
        | Lost score ->
                writeMessage <| sprintf "Lost %i" score 
        | _ -> ()

    let GenerateNewApple appleGenerator (blockedFields:GameField list) maxX maxY=
        let rec findFreeField () =
            let nextX = appleGenerator.RandomGenerator.Next(maxX)
            let nextY = appleGenerator.RandomGenerator.Next(maxY)
            if blockedFields 
                |> List.tryFind (fun t -> t.X = nextX && t.Y = nextY)
                |> Option.isNone then
                {X=nextX;Y=nextY}
            else
                findFreeField ()

        {appleGenerator with Apple = findFreeField()}

    let SetupBorder (completeGameField:GameFieldType[,]) =
        completeGameField.[0,maxIndexY] <- UpperLeftCorner {X=0;Y=maxIndexY}
        completeGameField.[maxIndexX,maxIndexY] <- UpperRightCorner {X=maxIndexX;Y=maxIndexY}
        completeGameField.[0,0] <- LowerLeftCorner {X=0;Y=0}
        completeGameField.[maxIndexX,0] <- LowerRightCorner {X=maxIndexX;Y=0}
        completeGameField.[0,2] <- RightThreeWay {X=0;Y=2}
        completeGameField.[maxIndexX,2] <- LeftThreeWay {X=maxIndexX;Y=2}

        completeGameField.[0,1] <- VerticalBorder {X=0;Y=1}
        completeGameField.[maxIndexX,1] <- VerticalBorder {X=maxIndexX;Y=1}

        for i = 1 to maxIndexX - 1 do
            completeGameField.[i,0] <- HorizontalBorder {X=i;Y=0}
            completeGameField.[i,2] <- HorizontalBorder {X=i;Y=2}
            completeGameField.[i,maxIndexY] <- HorizontalBorder {X=i;Y=maxIndexY}

        for i = 3 to maxIndexY - 1 do
            completeGameField.[0,i] <- VerticalBorder {X=0;Y=i}
            completeGameField.[maxIndexX,i] <- VerticalBorder {X=maxIndexX;Y=i}

        completeGameField

    let initGame = 

    //               |
    //               |
    //               |
    //  -------------+------------ x comes first in the 2d Array and zero is the bottom left
    //               |
    //               |
    //               y

        let completeGameField = 
            Array2D.create maxX maxY Empty
            |> SetupBorder
        let scoreArea = ArraySegment.Create(completeGameField, TwoDimensions.First, 1,(maxIndexX-1), 1)
        let matchField = MultiArraySegment.Create( completeGameField, 1, 4 ,maxMatchfieldX, maxMatchfieldY)
        let score = {ScoreFields = scoreArea}

        {
        Score = 0
        CompleteMatchField = completeGameField
        Matchfield = {MatchField = matchField}
        ScoreArea = score
        CurrentDirection= Direction.Up
        Status= GameState.Init
        Snake = {Head={X=11;Y=11;MoveDirection=Direction.Up}; SnakeElements = List.Empty}
        AppleGen = {Apple={X=7;Y=7};RandomGenerator= System.Random()}
        }

    let PrintGame state =
        SetScore state.ScoreArea state.Score
        System.Console.Clear()
        for i = maxIndexY downto 0 do
            for j = 0 to maxIndexX do
                let symbol = (state.CompleteMatchField.[j,i] :> IDisplayable).Display()
                if j < maxIndexX then
                    printf "%c" symbol
                else
                    printfn "%c" symbol

    let MatchKeyToDirection currentDirection (input:Keys)=
        match input, currentDirection with
        | (Keys.Up | Keys.W), Up -> Direction Up
        | (Keys.Up | Keys.W), Left -> Direction Up
        | (Keys.Up | Keys.W), Right -> Direction Up
        | (Keys.Down | Keys.S), Down -> Direction Down
        | (Keys.Down | Keys.S), Left -> Direction Down
        | (Keys.Down | Keys.S), Right -> Direction Down
        | (Keys.Left | Keys.A), Left -> Direction Left
        | (Keys.Left | Keys.A), Up -> Direction Left
        | (Keys.Left | Keys.A), Down -> Direction Left
        | (Keys.Right | Keys.D), Right -> Direction Right
        | (Keys.Right | Keys.D), Up -> Direction Right
        | (Keys.Right | Keys.D), Down -> Direction Right
        | _ -> NoInput

    let private stopWatch = Stopwatch()

    let GetInput tick currentDirection =
        let getConsoleKey elapsed =
            if Console.KeyAvailable then
                Console.ReadKey(true)
                |> MatchKeyToDirection currentDirection    
            elif elapsed < tick then
                Waiting
            else
                NoInput

        let resetStopWatch (watch:Stopwatch) =
            do watch.Stop()
            let remaining = tick - watch.ElapsedMilliseconds
            do watch.Reset()
            if remaining > 0L then
                do Thread.Sleep(int remaining)

        let rec GetInput' ()=
            let userInput = getConsoleKey stopWatch.ElapsedMilliseconds
            match userInput with
            | Direction dir -> 
                do resetStopWatch stopWatch
                dir
            | NoInput ->
                do resetStopWatch stopWatch
                currentDirection
            | Waiting -> 
                do Thread.Sleep(100)
                GetInput' ()

        stopWatch.Start()
        GetInput' ()        

    let GetInput'' = GetInput tick

    let GetNewApple appleGen snake =
        GenerateNewApple appleGen (ToGameFieldList snake) GameConstants.maxMatchfieldX GameConstants.maxMatchfieldY

    let GameLoop (input:Direction -> Direction) (state:GlobalGameState) :GlobalGameState=
        let initGame innerState =
            if innerState.Status = GameState.Init then
                {innerState with Status = Running}
            else 
                innerState

        let newDirection = input state.CurrentDirection
        let newSnake = newDirection 
                        |> SnakeLogic.moveSnake state.Snake

        let res = { initGame state with CurrentDirection = newDirection; Snake = newSnake }
        
        let nextRes =  
            if SnakeLogic.isSnakeTouchingItself res.Snake then
                {res with Status = Lost res.Score}
            elif ToStaticField res.Snake.Head = res.AppleGen.Apple then
                let newScore = res.Score + 5;
                do SetScore res.ScoreArea newScore
                let longerSnake = SnakeLogic.addNewSnakeElement state.Snake newSnake
                {res with AppleGen = GetNewApple res.AppleGen res.Snake; Score = newScore; Snake = longerSnake}
            else
                res

        do Clean nextRes.Matchfield
        if IsActive nextRes.Status then
            do WriteApple nextRes.Matchfield res.AppleGen.Apple
            do WriteSnake nextRes.Matchfield newSnake
        else
            do WriteGameFinish nextRes.Matchfield nextRes.Status

        nextRes

    let GameLoop' = GameLoop GetInput''


