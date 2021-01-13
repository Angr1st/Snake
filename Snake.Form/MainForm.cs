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
            MoveStartButton();
        }

        private void MoveStartButton()
        {
            btn_Start.Location = new Point((Width / 2) - (btn_Start.Width / 2), (Height / 3) - (btn_Start.Height / 2));
        }
    }
}
