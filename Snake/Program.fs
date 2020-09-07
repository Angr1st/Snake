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

let initGame = 
    let completeGameField = Array2D.create 17 19 Empty
    let scoreArea = completeGameField.[1..15,1]
    let matchField = completeGameField.[1..15,4..17]
    {
    Score = 0
    CompleteMatchField = completeGameField
    Matchfield = {MatchField=matchField}
    ScoreArea = {ScoreFields= scoreArea}
    }
[<EntryPoint>]
let main argv =
    
    0 // return an integer exit code
