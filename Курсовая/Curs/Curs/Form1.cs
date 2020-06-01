using System.Drawing;
using System.Windows.Forms;

namespace Curs
{
    public partial class Form1 : Form
    {
        public Bitmap bmp, newBmp;
        public double cent, ty;
        public bool color = false, chek = false, vcl = false;

        public const int L = 256;
        public double red, green, blue;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Activated(object sender, System.EventArgs e)
        {
            Program.f1.numericUpDown1.Value = (decimal)ty;
            if (!vcl) Program.f1.radioButton1.Checked = true;
            else Program.f1.radioButton2.Checked = true;
        }
    }
}
