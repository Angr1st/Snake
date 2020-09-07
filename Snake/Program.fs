// Learn more about F# at http://fsharp.org

open System

type IDisplayable =
    abstract Display: unit -> string

type TurnDirection =
    | LeftTurn = 0
    | RightTurn = 1

type Direction =
    | Up
    | Down
    | Left
    | Right

type GameField =
    {
    X:int //left and right
    Y:int //up and down
    MoveDirection:Direction
    }

type StaticField =
    {
    X:int
    Y:int
    }

type GameFieldType =
    | HorizontalBorder of StaticField
    | VerticalBorder of StaticField
    | UpperLeftCorner of StaticField
    | LowerLeftCorner of StaticField
    | LeftThreeWay of StaticField
    | RightThreeWay of StaticField
    | UpperRightCorner of StaticField
    | LowerRightCorner of StaticField
    | Empty
    | Apple of GameField
    | SnakeHead of GameField
    | SnakeBody of GameField
    | ScoreField of char
    interface IDisplayable with
        member g.Display() =
            match g with
            | Empty -> " "
            | Apple _-> "O"
            | SnakeHead _-> "■"
            | SnakeBody _-> "■"
            | HorizontalBorder _-> "═"
            | VerticalBorder _-> "║"
            | UpperLeftCorner _-> "╔"
            | UpperRightCorner _-> "╗"
            | LowerLeftCorner _-> "╚"
            | LowerRightCorner _-> "╝"
            | LeftThreeWay _-> "╣"
            | RightThreeWay _-> "╠"
            | ScoreField c-> c.ToString()

type GameState = {MatchField:GameFieldType[,]}

type ScoreBoard = {ScoreFields:GameFieldType[]}

type GlobalGameState = 
    {
    Score:int
    CompleteMatchField:GameFieldType[,]
    Matchfield:GameState
    ScoreArea:ScoreBoard
    } 

let maxX = 25

let maxY = maxX + 3

let maxIndexX = maxX - 1

let maxIndexY = maxY - 1

let setupBorder (completeGameField:GameFieldType[,]) =
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
        |> setupBorder
    let scoreArea = completeGameField.[1..(maxIndexX-1),1]
    scoreArea.[scoreArea.Length - 1] <- ScoreField '0'
    let matchField = completeGameField.[1..(maxIndexX-1),4..(maxIndexY - 1)]
    
    {
    Score = 0
    CompleteMatchField = completeGameField
    Matchfield = {MatchField = matchField}
    ScoreArea = {ScoreFields = scoreArea}
    }

[<EntryPoint>]
let main argv =
    for i = maxIndexY downto 0 do
        for j = 0 to maxIndexX do
            let symbol = (initGame.CompleteMatchField.[j,i] :> IDisplayable).Display()
            if j < maxIndexX then
                printf "%s" symbol
            else
                printfn "%s" symbol
            
    0 // return an integer exit code
