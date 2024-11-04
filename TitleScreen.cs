using Spros_SE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJECT_SPROS
{
    public partial class TitleScreen : Form
    {
        public TitleScreen() //титульный экран
        {
            InitializeComponent();
            string userName = Environment.UserName; //переменная, хранящая текущее имя пользователя
            label1.Text = "Добро пожаловать, " + userName + "!"; // приветствие программой пользователя
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TitleScreen_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
