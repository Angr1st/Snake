// Learn more about F# at http://fsharp.org

open System

type IDisplayable =
    abstract member Display: unit -> string

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

let setupBorder (completeGameField:GameFieldType[,]) =
    completeGameField.[0,18] <- UpperLeftCorner {X=0;Y=18}
    completeGameField.[18,18] <- UpperRightCorner {X=18;Y=18}
    completeGameField.[0,0] <- LowerLeftCorner {X=0;Y=0}
    completeGameField.[18,0] <- LowerRightCorner {X=18;Y=0}
    completeGameField.[]

let initGame = 

//               |
//               |
//               |
//  -------------+------------ x comes first in the 2d Array and zero is the bottom left
//               |
//               |
//               y
    let completeGameField = Array2D.create 17 19 Empty
    let scoreArea = completeGameField.[1..15,1]
    let matchField = completeGameField.[1..15,4..17]
    
    {
    Score = 0
    CompleteMatchField = completeGameField
    Matchfield = {MatchField = matchField}
    ScoreArea = {ScoreFields = scoreArea}
    }

[<EntryPoint>]
let main argv =
    
    0 // return an integer exit code
