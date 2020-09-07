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

type GameFieldType =
    | Empty of GameField
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

type GameState = 
    {
    Score:int
    Matchfield:GameFieldType[,]
    }   

[<EntryPoint>]
let main argv =
    
    printfn "Hello World from F#!"
    0 // return an integer exit code
