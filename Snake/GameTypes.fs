namespace Snake

open Snake.Lib

type IDisplayable =
    abstract Display: unit -> char

type GameState =
    | Init
    | Running
    | Won of int
    | Lost of int

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

type StaticField =
    {
    X:int
    Y:int
    }

and GameField =
    {
    X:int //left and right
    Y:int //up and down
    MoveDirection:Direction
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
    | Apple of StaticField
    | SnakeHead of GameField
    | SnakeBody of GameField
    | ScoreField of char
    interface IDisplayable with
        member g.Display() =
            match g with
            | Empty -> ' '
            | Apple _-> '@'
            | SnakeHead _-> '█'
            | SnakeBody _-> '█'
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

type Snake = 
    {
        Head:GameField
        SnakeElements:GameField list
    }

type MatchFieldState = {MatchField:MultiArraySegment<GameFieldType>}         

type SnakeAkkumulator = 
    {
        NewSnakeElements:GameField list
        MovementDirection:Direction
    }

type AppleGenerator =
    {
    Apple:StaticField
    RandomGenerator:System.Random
    }

type GlobalGameState = 
    {
    Score:int
    CompleteMatchField:GameFieldType[,]
    Matchfield:MatchFieldState
    ScoreArea:ScoreBoard
    CurrentDirection:Direction
    Status:GameState
    Snake:Snake
    AppleGen:AppleGenerator
    }

module GameConstants =
    let maxX = 25
    
    let maxY = maxX + 3
    
    let maxIndexX = maxX - 1
    
    let maxIndexY = maxY - 1

    let maxMatchfieldX = maxIndexX - 2

    let maxMatchfieldY = maxIndexY - 5

    let tick :int64 = 800L //ms


module ScoreBoardLogic =
    let SetScore scoreBoard value =
        let stringValue = value.ToString().PadLeft(scoreBoard.ScoreFields.Length, ' ').ToCharArray()

        for i = scoreBoard.ScoreFields.Length - 1 downto 0 do
            let ch = stringValue.[i]
            let field = match ch with
                        | ' ' -> Empty
                        | x -> ScoreField x
            scoreBoard.ScoreFields.[i] <- field

module GameFieldLogic =
    let ToGameField staticField direction=
        {X=staticField.X;Y=staticField.Y;MoveDirection=direction}

    let ToStaticField gameField =
        {X=gameField.X;Y=gameField.Y}