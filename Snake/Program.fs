open System
open Snake
open Snake.Lib

let maxX = 25

let maxY = maxX + 3

let maxIndexX = maxX - 1

let maxIndexY = maxY - 1

let setupBorder (completeGameField:GameFieldType[,]) =
    completeGameField.[0,maxIndexY] <- UpperLeftCorner {X=0;Y=maxIndexY}
    completeGameField.[maxIndexX,maxIndexY] <- UpperRightCorner {X=maxIndexX;Y=maxIndexY}
    completeGameField.[0,0] <- LowerLeftCorner {X=0;Y=0}
    completeGameField.[maxIndexX,0] <- LowerRightCorner {X=maxIndexX;Y=0}
    completeGameField.[0,2] <- RightThreeWay {X=0;Y=2}
    completeGameField.[maxIndexX,2] <- LeftThreeWay {X=maxIndexX;Y=2}

    completeGameField.[0,1] <- VerticalBorder {X=0;Y=1}
    completeGameField.[maxIndexX,1] <- VerticalBorder {X=maxIndexX;Y=1}

    for i = 1 to maxIndexX - 1 do
        completeGameField.[i,0] <- HorizontalBorder {X=i;Y=0}
        completeGameField.[i,2] <- HorizontalBorder {X=i;Y=2}
        completeGameField.[i,maxIndexY] <- HorizontalBorder {X=i;Y=maxIndexY}

    for i = 3 to maxIndexY - 1 do
        completeGameField.[0,i] <- VerticalBorder {X=0;Y=i}
        completeGameField.[maxIndexX,i] <- VerticalBorder {X=maxIndexX;Y=i}

    completeGameField



let initGame = 

//               |
//               |
//               |
//  -------------+------------ x comes first in the 2d Array and zero is the bottom left
//               |
//               |
//               y

    let completeGameField = 
        Array2D.create maxX maxY Empty
        |> setupBorder
    let scoreArea = ArraySegment.Create(completeGameField, TwoDimensions.First, 1,(maxIndexX-1))
    scoreArea.[(scoreArea.Count - 1)] <- ScoreField '0'
    let matchField = MultiArraySegment.Create( completeGameField, 1, 4 ,maxIndexX-1, maxIndexY - 5)
    let score = {ScoreFields = scoreArea}
    let matchT = {MatchField = matchField}
    matchT.MatchField.[4,5] <- SnakeHead {X=4;Y=5;MoveDirection=Direction.Up}
    score.ScoreFields.[0] <- ScoreField '9'
    {
    Score = 0
    CompleteMatchField = completeGameField
    Matchfield = {MatchField = matchField}
    ScoreArea = score
    }

[<EntryPoint>]
let main argv =
    for i = maxIndexY downto 0 do
        for j = 0 to maxIndexX do
            let symbol = (initGame.CompleteMatchField.[j,i] :> IDisplayable).Display()
            if j < maxIndexX then
                printf "%c" symbol
            else
                printfn "%c" symbol
            
    0 // return an integer exit code
