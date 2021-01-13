namespace Snake

open Game
open System
open System.Windows.Forms
open Snake.FormLib

module Program =

    let execGame state = 
        let mutable innerState = state

        while IsActive innerState.Status do
            innerState <- GameLoop' innerState 
            PrintGame innerState

    [<EntryPoint; STAThread>]
    let main argv =
        Application.SetHighDpiMode(HighDpiMode.SystemAware)
        |> ignore
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(false)
        let mainForm = new MainForm()
        Application.Run(mainForm)
        PrintGame initGame
        initGame
        |> execGame
            
        0 // return an integer exit code
