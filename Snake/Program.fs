open Snake.Game


[<EntryPoint>]
let main argv =
    PrintGame initGame
    let nextState = GameLoop initGame GetInput''
    PrintGame nextState        
    0 // return an integer exit code
