namespace Snake

open Game

module Program =

    let execGame (state:GlobalGameState) = 
        let mutable innerState = state

        while innerState.Status.IsActive() do
            innerState <- GameLoop' innerState 
            PrintGame innerState

    [<EntryPoint>]
    let main argv =
        PrintGame initGame
        initGame
        |> execGame
            
        0 // return an integer exit code
