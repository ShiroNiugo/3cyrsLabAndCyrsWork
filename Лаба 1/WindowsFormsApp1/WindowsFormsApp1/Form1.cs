using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        double x, y;
        bool tochka = false, yer = true;
        int sign;
        string f = "Делить на нуль нельзя!";

        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Button a = sender as Button;
            if (a != button18)
            {
                if (textBox1.Text == "0" || textBox1.Text == f)
                {
                    textBox1.Text = "";
                }
                textBox1.Text += a.Text;
            }
            if (a == button18 && !tochka)
            {
                tochka = !tochka;
                textBox1.Text += a.Text;
            }
        }

        void clear()
        {
            x = 0;
            tochka = false;
            yer = true;
            textBox1.Text = "0";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            y = 0;
            clear();
        }

        void tre()
        {
            if(textBox1.Text == f) {
                textBox1.Text = "0";
            }
            x = Convert.ToDouble(textBox1.Text);
            y = x;
            clear();
            yer = false;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Control r = sender as Control;
            sign = r.TabIndex;
            tre();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            x = Convert.ToDouble(textBox1.Text);
            textBox1.Text = (-x).ToString();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (!yer)
            {
                x = Convert.ToDouble(textBox1.Text);
                yer = true;
            }

            if (x == 0 && sign == 22)
            {
                textBox1.Text = f;
            }
            else
            {
                switch (sign)
                {
                    case 10:
                        {
                            y += x;
                            break;
                        }
                    case 14:
                        {
                            y -= x;
                            break;
                        }
                    case 18:
                        {
                            y *= x;
                            break;
                        }
                    case 22:
                        {
                            y /= x;
                            break;
                        }
                }
                textBox1.Text = y.ToString();
            }
        }
    }
}
