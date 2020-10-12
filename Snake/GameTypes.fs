﻿namespace Snake

open Snake.Lib

type IDisplayable =
    abstract Display: unit -> char

type GameState =
    | Init
    | Running
    | Won of int
    | Lost of int
    with 
        member self.IsActive ()=
            match self with
            | Init | Running -> true
            | _ -> false

type TurnDirection =
    | LeftTurn = 0
    | RightTurn = 1

type Direction =
    | Up
    | Down
    | Left
    | Right

type UserInput =
    | Direction of Direction
    | Waiting
    | NoInput

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
            | Empty -> ' '
            | Apple _-> 'O'
            | SnakeHead _-> '■'
            | SnakeBody _-> '■'
            | HorizontalBorder _-> '═'
            | VerticalBorder _-> '║'
            | UpperLeftCorner _-> '╔'
            | UpperRightCorner _-> '╗'
            | LowerLeftCorner _-> '╚'
            | LowerRightCorner _-> '╝'
            | LeftThreeWay _-> '╣'
            | RightThreeWay _-> '╠'
            | ScoreField c-> c

type ScoreBoard = {ScoreFields:ArraySegment<GameFieldType>}
    with 
        member self.SetScore value =
            let stringValue = value.ToString().PadLeft(self.ScoreFields.Length, ' ').ToCharArray()

            for i = self.ScoreFields.Length - 1 downto 0 do
                let ch = stringValue.[i]
                let field = match ch with
                            | ' ' -> Empty
                            | x -> ScoreField x
                self.ScoreFields.[i] <- field

type Snake = 
    {
        Head:GameField
        SnakeElements:GameField list
    }
    with
        member self.ToGameFieldType ()=
            SnakeHead self.Head

type MatchFieldState = {MatchField:MultiArraySegment<GameFieldType>}
    with
        member self.Clean ()=
            self.MatchField.SetAllFields Empty

        member self.WriteSnake (snake:Snake) =
            self.MatchField.[snake.Head.X, snake.Head.Y] <- snake.ToGameFieldType()
            snake.SnakeElements
            |> List.iter (fun body -> self.MatchField.[body.X, body.Y] <- SnakeBody body)

type SnakeAkkumulator = 
    {
        NewSnakeElements:GameField list
        MovementDirection:Direction
    }
    with
        member self.NewSnakeHead ()=
            self.NewSnakeElements
            |> List.tryHead

        member self.ToSnake ()=
            let head = self.NewSnakeHead()
            if head.IsNone then
                failwith "The Snake lost its head!"
            else
                {Head=head.Value;SnakeElements=self.NewSnakeElements}

type GlobalGameState = 
    {
    Score:int
    CompleteMatchField:GameFieldType[,]
    Matchfield:MatchFieldState
    ScoreArea:ScoreBoard
    CurrentDirection:Direction
    Status:GameState
    Snake:Snake
    }

module GameConstants =
    let maxX = 25
    
    let maxY = maxX + 3
    
    let maxIndexX = maxX - 1
    
    let maxIndexY = maxY - 1

    let maxMatchfieldX = maxIndexX - 2

    let maxMatchfieldY = maxIndexY - 5

    let tick :int64 = 1000L //ms