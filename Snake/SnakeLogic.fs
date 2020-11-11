namespace Snake

module SnakeLogic =
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

        let accumulateNewSnake (oldState:SnakeAkkumulator) (nextSnakepart:GameField) : SnakeAkkumulator=
            let newSnakeElement = 
                nextSnakepart
                |> newGameElement oldState.MovementDirection
            {NewSnakeElements= newSnakeElement :: oldState.NewSnakeElements; MovementDirection= nextSnakepart.MoveDirection}

        let firstAkkumulator = {
            NewSnakeElements = list.Empty
            MovementDirection = direction
        }

        let reverseAkkumulator (akku:SnakeAkkumulator) =
            {akku with NewSnakeElements = akku.NewSnakeElements |> List.rev}

        let toNewSnake (akku:SnakeAkkumulator) =
            akku.ToSnake()

        let asGamefieldList = snake.ToGameFieldList()
        asGamefieldList
        |> List.fold accumulateNewSnake firstAkkumulator
        |> reverseAkkumulator
        |> toNewSnake

    let addNewSnakeElement (oldSnake:Snake) (newSnake:Snake) =
        let lastElementList = oldSnake.Last() :: List.Empty
        {newSnake with SnakeElements = newSnake.SnakeElements @ lastElementList}

    let isSnakeTouchingItself (snake:Snake) =
        snake.SnakeElements
        |> List.map (fun x -> x.ToStaticField())
        |> List.tryFind (fun x -> x = snake.Head.ToStaticField ())
        |> Option.isSome