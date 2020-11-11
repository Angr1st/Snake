namespace Snake

open Snake.Lib
open System
open System.Diagnostics
open System.Threading
open Snake.GameConstants

module Game =
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
        let matchT = {MatchField = matchField}
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
        state.ScoreArea.SetScore state.Score
        System.Console.Clear()
        for i = maxIndexY downto 0 do
            for j = 0 to maxIndexX do
                let symbol = (state.CompleteMatchField.[j,i] :> IDisplayable).Display()
                if j < maxIndexX then
                    printf "%c" symbol
                else
                    printfn "%c" symbol

    let MatchKeyToDirection currentDirection (input:ConsoleKeyInfo)=
        match input.Key, currentDirection with
        | ConsoleKey.UpArrow, Up -> Direction Up
        | ConsoleKey.UpArrow, Left -> Direction Up
        | ConsoleKey.UpArrow, Right -> Direction Up
        | ConsoleKey.DownArrow, Down -> Direction Down
        | ConsoleKey.DownArrow, Left -> Direction Down
        | ConsoleKey.DownArrow, Right -> Direction Down
        | ConsoleKey.LeftArrow, Left -> Direction Left
        | ConsoleKey.LeftArrow, Up -> Direction Left
        | ConsoleKey.LeftArrow, Down -> Direction Left
        | ConsoleKey.RightArrow, Right -> Direction Right
        | ConsoleKey.RightArrow, Up -> Direction Right
        | ConsoleKey.RightArrow, Down -> Direction Right
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

    let GetNewApple (appleGen:AppleGenerator) (snake:Snake) =
        appleGen.GenerateNewApple (snake.ToGameFieldList ()) GameConstants.maxMatchfieldX GameConstants.maxMatchfieldY

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
            elif res.Snake.Head.ToStaticField() = res.AppleGen.Apple then
                let newScore = res.Score + 5;
                do res.ScoreArea.SetScore newScore
                let longerSnake = SnakeLogic.addNewSnakeElement state.Snake newSnake
                {res with AppleGen = GetNewApple res.AppleGen res.Snake; Score = newScore; Snake = longerSnake}
            else
                res

        do nextRes.Matchfield.Clean()
        do nextRes.Matchfield.WriteApple res.AppleGen.Apple
        do nextRes.Matchfield.WriteSnake newSnake

        nextRes

    let GameLoop' = GameLoop GetInput''


