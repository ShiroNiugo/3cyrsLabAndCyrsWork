using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        Bitmap bmp, newBmp;
        ImageFormat[] formats = { ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Png };
        int brig, ty, color2;
        bool color = false, chek = false;
        Color c;
        int cR, cG, cB;
        string namefile = "", file = "BMP files (*.BMP)|*.bmp|" +
                                     "JPG files (*.JPG,)|*.jpg|" +
                                     "PNG files (*.PNG)|*.png";

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
            if (openFileDialog1.FileName != saveFileDialog1.FileName)
                openFileDialog1.FileName = saveFileDialog1.FileName;
            try
            {
                newBmp = new Bitmap(pictureBox1.Image);
                newBmp.Save(saveFileDialog1.FileName, formats[saveFileDialog1.FilterIndex - 1]);
                bmp = newBmp;
            }
            catch
            {
                MessageBox.Show("Файл для сохранения не найден!", "Ошибка");
            }

        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (namefile != "")
            {
                saveFileDialog1.Filter = file;
                saveFileDialog1.FileName = namefile;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    bmp = new Bitmap(pictureBox1.Image);
                    bmp.Save(saveFileDialog1.FileName, formats[saveFileDialog1.FilterIndex - 1]);
                }
            }
            else MessageBox.Show("Для сохранения ничего не открыто", "Ошибка");
        }

        public void brightness(int brig)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    pictureBox1.Image = bmp;
                }
                if (radioButton2.Checked)
                {
                    newBmp = new Bitmap(bmp.Width, bmp.Height);
                    brig = (brig < -255) ? -255 : (brig > 255) ? 255 : brig;
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        for (int i = 0; i < bmp.Width; i++)
                        {
                            c = bmp.GetPixel(i, j);
                            cR = c.R + brig;
                            cG = c.G + brig;
                            cB = c.B + brig;

                            cR = (cR < 0) ? 1 : (cR > 255) ? 255 : cR;
                            cG = (cG < 0) ? 1 : (cG > 255) ? 255 : cG;
                            cB = (cB < 0) ? 1 : (cB > 255) ? 255 : cB;

                            newBmp.SetPixel(i, j, Color.FromArgb((byte)cR, (byte)cG, (byte)cB));
                            progressBar1.Value += (progressBar1.Value != progressBar1.Maximum) ? 1 : 0;
                        }
                        pictureBox1.Image = newBmp;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Картинка не выбрана", "Ошибка");
                radioButton1.Checked = true;
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Студент группы ЭИС-26\nКапустин Д.Е.", "О программе");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = pictureBox1.Width * pictureBox1.Height / 100;
            progressBar1.Value = progressBar1.Minimum;
            brig = (int)numericUpDown1.Value;
            chek = (brig == ty) ? true : false;
            if (color && chek)
            {
                radioButton1.Checked = true;
                brightness(brig);
                color = false;
            }
            else
            {
                radioButton2.Checked = true;
                brightness(brig);
                color = true;
                ty = (int)numericUpDown1.Value;
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
