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

        let newHead = snake.Head |> newGameElement direction
        {Head=newHead; SnakeElements= List.Empty}