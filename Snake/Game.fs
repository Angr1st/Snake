﻿namespace Snake

open Snake.Lib
open System
open System.Diagnostics
open System.Threading

module Game =
    let maxX = 25

    let maxY = maxX + 3

    let maxIndexX = maxX - 1

    let maxIndexY = maxY - 1

    let tick :int64 = 1000L //ms

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
        let matchField = MultiArraySegment.Create( completeGameField, 1, 4 ,maxIndexX-1, maxIndexY - 5)
        let score = {ScoreFields = scoreArea}
        let matchT = {MatchField = matchField}
        matchT.MatchField.[4,5] <- SnakeHead {X=4;Y=5;MoveDirection=Direction.Up}
        score.SetScore 12456
        {
        Score = 0
        CompleteMatchField = completeGameField
        Matchfield = {MatchField = matchField}
        ScoreArea = score
        CurrentDirection= Direction.Up
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
            watch.Reset()
            ()

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

    let GameLoop (state:GlobalGameState) (input:Direction -> Direction) :GlobalGameState=
        let newDirection = input state.CurrentDirection
        state

