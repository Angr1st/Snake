namespace Snake

open Snake.GameFieldLogic

module SnakeLogic =
    let ToGameFieldType snake =
        SnakeHead snake.Head

    let ToGameFieldList snake =
        snake.Head :: snake.SnakeElements

    let Last snake =
        match snake.SnakeElements.IsEmpty with
        | true -> snake.Head
        | false -> snake.SnakeElements |> List.last

    let NewSnakeHead snakeAkku =
        snakeAkku.NewSnakeElements
        |> List.tryHead

    let ToSnake snakeAkku =
        let head = NewSnakeHead snakeAkku
        if head.IsNone then
            failwith "The Snake lost its head!"
        else
            {Head=head.Value;SnakeElements=snakeAkku.NewSnakeElements.Tail}

    let moveSnake (snake:Snake) (direction:Direction) =
        let newGameElement nextDirection (oldGameElement:GameField)=
            match nextDirection with
            | Up -> 
                if oldGameElement.Y = GameConstants.maxMatchfieldY then
                   {Y=0;X=oldGameElement.X;MoveDirection=nextDirection}
                else     
                    {Y=oldGameElement.Y + 1;X=oldGameElement.X;MoveDirection=nextDirection}
            | Down -> 
                if oldGameElement.Y = 0 then          
                    {Y=GameConstants.maxMatchfieldY;X=oldGameElement.X;MoveDirection=nextDirection}
                else
                    {Y=oldGameElement.Y - 1;X=oldGameElement.X;MoveDirection=nextDirection}
            | Left -> 
                if oldGameElement.X = 0 then
                    {Y=oldGameElement.Y;X=GameConstants.maxMatchfieldX;MoveDirection=nextDirection}
                else
                    {Y=oldGameElement.Y;X=oldGameElement.X - 1;MoveDirection=nextDirection}
            | Right -> 
                if oldGameElement.X = GameConstants.maxMatchfieldX then
                    {Y=oldGameElement.Y;X=0;MoveDirection=nextDirection}
                else
                    {Y=oldGameElement.Y;X=oldGameElement.X + 1;MoveDirection=nextDirection}

        let accumulateNewSnake oldState nextSnakepart=
            let newSnakeElement = 
                nextSnakepart
                |> newGameElement oldState.MovementDirection
            {NewSnakeElements= newSnakeElement :: oldState.NewSnakeElements; MovementDirection= nextSnakepart.MoveDirection}

        let firstAkkumulator = {
            NewSnakeElements = list.Empty
            MovementDirection = direction
        }

        let reverseAkkumulator akku =
            {akku with NewSnakeElements = akku.NewSnakeElements |> List.rev}

        let toNewSnake akku =
            ToSnake akku

        let asGamefieldList = ToGameFieldList snake
        asGamefieldList
        |> List.fold accumulateNewSnake firstAkkumulator
        |> reverseAkkumulator
        |> toNewSnake

    let addNewSnakeElement oldSnake newSnake =
        let lastElementList = Last oldSnake :: List.Empty
        {newSnake with SnakeElements = newSnake.SnakeElements @ lastElementList}

    let isSnakeTouchingItself snake =
        let sneakHead = ToStaticField snake.Head
        snake.SnakeElements
        |> List.map (fun x -> ToStaticField x)
        |> List.tryFind (fun x -> x = sneakHead)
        |> Option.isSome