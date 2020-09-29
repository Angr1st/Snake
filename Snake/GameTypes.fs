namespace Snake

open Snake.Lib

type IDisplayable =
    abstract Display: unit -> char

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

type GameState = {MatchField:MultiArraySegment<GameFieldType>}

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

type GlobalGameState = 
    {
    Score:int
    CompleteMatchField:GameFieldType[,]
    Matchfield:GameState
    ScoreArea:ScoreBoard
    CurrentDirection:Direction
    }

type Snake = {Head:GameField}
    with
        member self.ToGameFieldType ()=
            SnakeHead self.Head