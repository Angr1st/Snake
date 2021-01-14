namespace Snake

open Snake.FormLib.Properties
open Snake.Lib
open System.Windows.Forms
open System.Drawing

type IDisplayable =
    abstract Display: unit -> Bitmap

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

type FruitType =
    | Avocado
    | Banana
    | Pear

type UserInput =
    | Direction of Direction
    | Waiting
    | NoInput

type StaticField =
    {
    X:int
    Y:int
    Tile:PictureBox
    }

and SnakeField =
    {
    X:int //left and right
    Y:int //up and down
    MoveDirection:Direction
    Tile:PictureBox
    }

and SnakeBodyField =
    {
    X:int //left and right
    Y:int //up and down
    OldMoveDirection:Direction
    NewMoveDirection:Direction
    Tile:PictureBox
    }

and FruitField =
    {
    X:int //left and right
    Y:int //up and down
    FruitType:FruitType
    Tile:PictureBox
    }

type SnakeFieldType =
    | SnakeHead of SnakeField
    | SnakeBody of SnakeBodyField
    | SnakeTail of SnakeField
    interface IDisplayable with
        member g.Display() =
            match g with
            | SnakeHead gf->
                match gf.MoveDirection with
                | Down -> Resources.Snake_Kopf_oben
                | Up -> Resources.Snake_Kopf_unten
                | Right -> Resources.Snake_Kopf_links
                | Left -> Resources.Snake_Kopf_rechts
            | SnakeBody sb->
                match sb.NewMoveDirection, sb.OldMoveDirection with
                | Up, Up | Down,Down -> Resources.Snake_Körper_oben_unten
                | Left, Left | Right, Right -> Resources.Snake_Körper_links_rechts
                | Left, Up | Up, Left -> Resources.Snake_Ecke_links_oben
                | Up, Right | Right, Up -> Resources.Snake_Ecke_oben_rechts
                | Left, Down | Down, Left -> Resources.Snake_Ecke_unten_links
                | Right, Down | Down, Right -> Resources.Snake_Ecke_rechts_unten
                | _,_ -> failwith "This combination of old and new movedirection is illegal!!!"
            | SnakeTail gf ->
                match gf.MoveDirection with
                | Up -> Resources.Snake_Schwanz_oben
                | Down -> Resources.Snake_Schwanz_unten
                | Left -> Resources.Snake_Schwanz_links
                | Right -> Resources.Snake_Schwanz_rechts

type GameFieldType =
    | Empty of StaticField
    | Apple of FruitField
    | SnakeField of SnakeFieldType
    interface IDisplayable with
        member g.Display() =
            match g with
            | Empty _-> Resources.Snake_Hintergrund
            | Apple ff ->
                match ff.FruitType with
                | Avocado -> Resources.Snake_Avocado
                | Banana -> Resources.Snake_Banane
                | Pear -> Resources.Snake_Birne
            | SnakeField s -> (s :> IDisplayable).Display()

type Snake = 
    {
        Head:SnakeField
        SnakeElements:SnakeBodyField list
        Tail: SnakeField
    }

type MatchFieldState = {MatchField:MultiArraySegment<GameFieldType>}         

type SnakeAkkumulator = 
    {
        NewSnakeElements:SnakeFieldType list
        MovementDirection:Direction
    }

type AppleGenerator =
    {
    Apple:FruitField
    RandomGenerator:System.Random
    }

type GlobalGameState = 
    {
    Score:int
    Matchfield:GameFieldType[,]
    CurrentDirection:Direction
    Status:GameState
    Snake:Snake
    AppleGen:AppleGenerator
    }

module GameConstants =
    let maxX = 10
    
    let maxY = maxX
    
    let maxIndexX = maxX - 1
    
    let maxIndexY = maxIndexX

    let tick :int64 = 800L //ms


module ScoreBoardLogic =
    let SetScore (scoreBoard:Label) value =
        let stringValue = value.ToString()

        scoreBoard.Text <- stringValue

module GameFieldLogic =
    let ToSnakeHeadField (staticField:StaticField) direction : SnakeField=
        {X=staticField.X;Y=staticField.Y;MoveDirection=direction;Tile=staticField.Tile}

    let ToStaticField (gameField:SnakeField):StaticField =
        {X=gameField.X;Y=gameField.Y;Tile=gameField.Tile}