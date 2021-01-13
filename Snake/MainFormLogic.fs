namespace Snake

open Snake.FormLib

module MainFormLogic =

    let MainForm = new MainForm()
    
    let private clickStartButton (mainForm:MainForm) =
        do mainForm.gb_Score.Visible <- true
        do mainForm.btn_Start.Visible <- false

    let registerEvents mainForm =
        MainForm.btn_Start.Click.Add(fun eventArgs -> clickStartButton mainForm )

