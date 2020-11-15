namespace Snake

open Game

module Program =

    let execGame state = 
        let mutable innerState = state

        while IsActive innerState.Status do
            innerState <- GameLoop' innerState 
            PrintGame innerState

    [<EntryPoint>]
    let main argv =
        PrintGame initGame
        initGame
        |> execGame
            
        0 // return an integer exit code
