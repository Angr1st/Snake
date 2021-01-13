using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake.FormLib
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            MoveStartButton(true);
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            gb_Score.Visible = true;
            btn_Start.Visible = false;
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            MoveStartButton();
        }

        private void MoveStartButton(bool initial = false)
        {
            if (btn_Start.Visible || initial)
            {
                btn_Start.Location = new Point((Width / 2) - (btn_Start.Width / 2), (Height / 3) - (btn_Start.Height / 2));
            }
        }
    }
}
