using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        ImageFormat[] formats = {ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Png};
        Graphics g;
        Point lastPoint = new Point(),
              fistPoint = new Point(),
              Re = new Point();

        int val;
        string namefile = "", file = "BMP files (*.bmp)| *.bmp| JPG files (*.JPG)| *.jpg| PNG files (*.PNG)| *.png";
        
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            lol();
        }

        void lol()
        {
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
        }


        // Открыть\сохранить
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = file;
            openFileDialog1.FileName = namefile;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp = new Bitmap(Image.FromFile(openFileDialog1.FileName));
                pictureBox1.Image = bmp;
                saveFileDialog1.FileName = openFileDialog1.FileName;
                namefile = openFileDialog1.SafeFileName;
            }
        }
        
        public void Save()
        {
            bmp = new Bitmap(pictureBox1.Image);
            bmp.Save(saveFileDialog1.FileName, formats[saveFileDialog1.FilterIndex - 1]);
            namefile = System.IO.Path.GetFileName(saveFileDialog1.FileName);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.FileName == saveFileDialog1.FileName)
                    saveFileDialog1.FileName = openFileDialog1.FileName;
                else openFileDialog1.FileName = saveFileDialog1.FileName;
                if (namefile != "")  Save();
                else сохранитьКакToolStripMenuItem_Click(sender, e);
            }
            catch
            {
                MessageBox.Show("Файл для сохранения не найден!", "Ошибка");
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = saveFileDialog1.FileName;
            saveFileDialog1.Filter = file;
            saveFileDialog1.FileName = namefile;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) Save();
        }

        // Рисование мышкой
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            g = Graphics.FromImage(bmp);
            val = Convert.ToInt32(toolStripMenuItem2.Text);
            fistPoint = e.Location;
            Re = e.Location;
        }

        void re() {
            Re.X = fistPoint.X < lastPoint.X ? fistPoint.X : lastPoint.X;
            Re.Y = fistPoint.Y < lastPoint.Y ? fistPoint.Y : lastPoint.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
            if (fistPoint != null)
            {
                if (radioButton1.Checked) g.DrawLine(new Pen(colorDialog1.Color, val), fistPoint.X, fistPoint.Y, lastPoint.X, lastPoint.Y);
                if (checkBox1.Checked)
                {
                    if (radioButton2.Checked)
                    {
                        g.FillEllipse(new SolidBrush(colorDialog1.Color), fistPoint.X, fistPoint.Y, lastPoint.X - fistPoint.X, lastPoint.Y - fistPoint.Y);
                    }
                    if (radioButton3.Checked)
                    {
                        re();
                        g.FillRectangle(new SolidBrush(colorDialog1.Color), Re.X, Re.Y, Math.Abs(lastPoint.X - fistPoint.X), Math.Abs(lastPoint.Y - fistPoint.Y));
                    }
                }
                else
                {
                    if (radioButton2.Checked)
                    {
                        g.DrawEllipse(new Pen(colorDialog1.Color, val), fistPoint.X, fistPoint.Y, lastPoint.X - fistPoint.X, lastPoint.Y - fistPoint.Y);
                    }
                    if (radioButton3.Checked)
                    {
                        re();
                        g.DrawRectangle(new Pen(colorDialog1.Color, val), Re.X, Re.Y, Math.Abs(lastPoint.X - fistPoint.X), Math.Abs(lastPoint.Y - fistPoint.Y));
                    }
                }
                pictureBox1.Invalidate();
            }
            pictureBox1.Image = bmp;
        }

        private void изменитьЦветToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                colorDialog1.Color = colorDialog1.Color;
            }
        }

        private void очиститьПолеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lol();
            pictureBox1.Invalidate();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Студент группы ЭИС-26\nКапустин Д.Е.", "О программе");
        }

        // Выход
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите выйти?", "Выход", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (pictureBox1.Image != null)
                {
                    сохранитьКакToolStripMenuItem_Click(sender, e);
                }
                Close();
            }
        }
    }
}
