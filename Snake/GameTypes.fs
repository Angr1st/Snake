namespace Snake

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

module GameTypes =

