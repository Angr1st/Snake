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
    X:int
    Y:int
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
    | Empty of StaticField
    | Apple of GameField
    | SnakeHead of GameField
    | SnakeBody of GameField
    interface IDisplayable with
        member g.Display() =
            match g with
            | Empty _-> " "
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

type GameState = 
    {
    Score:int
    Matchfield:GameFieldType[,]
    }   

[<EntryPoint>]
let main argv =
    
    printfn "Hello World from F#!"
    0 // return an integer exit code
