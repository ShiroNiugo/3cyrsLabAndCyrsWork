using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        Bitmap bmp, newBmp;

        Color c;
        ImageFormat[] formats = { ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Png };

        byte gray;
        int color;
        bool Gray = false;
        string namefile, file = "BMP files (*.BMP)|*.bmp| JPG files (*.JPEG)|*.jpeg| PNG files (*.PNG)|*.png";

        public Form1()
        {
            InitializeComponent();
        }


        // Открыть\сохранить
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = file;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp = new Bitmap(Image.FromFile(openFileDialog1.FileName));
                pictureBox1.Image = bmp;
                saveFileDialog1.FileName = openFileDialog1.FileName;
                namefile = openFileDialog1.SafeFileName;
            }
        }
        


        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.FileName == openFileDialog1.FileName)
                saveFileDialog1.FileName = openFileDialog1.FileName;
            else openFileDialog1.FileName = saveFileDialog1.FileName;
            try
            {
                newBmp = new Bitmap(pictureBox1.Image);
                newBmp.Save(saveFileDialog1.FileName, formats[saveFileDialog1.FilterIndex-1]);
                bmp = newBmp;
            }
            catch
            {
                MessageBox.Show("Файл для сохранения не найден!", "Ошибка");
            }
            
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = file;
            saveFileDialog1.FileName = namefile;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp = new Bitmap(pictureBox1.Image);
                bmp.Save(saveFileDialog1.FileName, formats[saveFileDialog1.FilterIndex - 1]);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton q = sender as RadioButton;
            color = q.TabIndex;
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Студент группы ЭИС-26\nКапустин Д.Е.", "О программе");
        }

        void colorGray(bool Gray)
        {
            progressBar1.Value = 0;
            newBmp = new Bitmap(bmp.Width, bmp.Height);
            for (int j = 0; j < bmp.Height; j++)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    c = bmp.GetPixel(i, j);
                    gray = (Gray) ? (byte)(.299 * c.R + .587 * c.G + .114 * c.B) 
                                    : (byte)(.5126 * c.R + .1152 * c.G + .1722 * c.B);
                    newBmp.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                    progressBar1.Value += progressBar1.Value != progressBar1.Maximum ? 1 : 0;
                }
                pictureBox1.Image = newBmp;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Maximum = pictureBox1.Width * pictureBox1.Height;
                progressBar1.Value = progressBar1.Minimum;
                switch (color)
                {
                    case 4:
                        {
                            pictureBox1.Image = bmp;
                            break;
                        }
                    case 5:
                        {
                            Gray = true;
                            colorGray(Gray);
                            break;
                        }
                    case 6:
                        {
                            Gray = false;
                            colorGray(Gray);
                            break;
                        }
                }
            }
            catch
            {
                MessageBox.Show("Картинка не выбрана", "Ошибка");
                radioButton1.Checked = true;
            }
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
