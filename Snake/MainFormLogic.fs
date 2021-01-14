namespace Snake

open Snake.FormLib
open System.Windows.Forms
open System.Drawing

module MainFormLogic =

    let MainForm = new MainForm()
    
    let private clickStartButton (mainForm:MainForm) =
        do mainForm.gb_Score.Visible <- true
        do mainForm.btn_Start.Visible <- false

        let tileSize = new Size(64,64)

        for x = 0 to 9 do
            for y = 0 to 9 do
                let newTile = new PictureBox()
                do mainForm.Controls.Add(newTile)
                do newTile.Location <- new Point(12 + x * 64, 12 + y * 64)
                do newTile.Size <- tileSize
                do newTile.Image <- Properties.Resources.Snake_Hintergrund
                do newTile.Visible <- true



    let registerEvents mainForm =
        MainForm.btn_Start.Click.Add(fun eventArgs -> clickStartButton mainForm )

