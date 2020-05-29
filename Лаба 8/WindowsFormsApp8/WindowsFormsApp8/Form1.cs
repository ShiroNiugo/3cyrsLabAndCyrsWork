using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Form1 : Form
    {
        Bitmap bmp, newBmp;
        ImageFormat[] formats = { ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Png };
        int cent, ty;
        bool color = false, chek = false;
        const int L = 256;
        double red, green, blue;
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
            saveFileDialog1.Filter = file;
            saveFileDialog1.FileName = namefile;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp = new Bitmap(pictureBox1.Image);
                bmp.Save(saveFileDialog1.FileName, formats[saveFileDialog1.FilterIndex - 1]);
            }
        }

        public void contrast(int cent)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    pictureBox1.Image = bmp;
                }
                if (radioButton2.Checked)
                {
                    BitmapData bdata = bmp.LockBits(
                        new Rectangle(0, 0, bmp.Width, bmp.Height),
                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                    byte[] pixelBuffer = new byte[bdata.Stride * bdata.Height];
                    Marshal.Copy(bdata.Scan0, pixelBuffer, 0, pixelBuffer.Length);

                    bmp.UnlockBits(bdata);

                    double contrastLevel = 1.0 + cent / 100.0;
                    int y = pixelBuffer.Length;

                    for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
                    {
                        red = ((((pixelBuffer[k] / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;
                        green = ((((pixelBuffer[k + 1] / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;
                        blue = ((((pixelBuffer[k + 2] / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;

                        red = (red < 0) ? 1 : (red > 255) ? 255 : red;
                        green = (green < 0) ? 1 : (green > 255) ? 255 : green;
                        blue = (blue < 0) ? 1 : (blue > 255) ? 255 : blue;

                        pixelBuffer[k] = (byte)red;
                        pixelBuffer[k + 1] = (byte)green;
                        pixelBuffer[k + 2] = (byte)blue;
                        
                        progressBar1.Value += (progressBar1.Value != progressBar1.Maximum) ? 1 : 0;
                    }

                    newBmp = new Bitmap(bmp.Width, bmp.Height);
                    BitmapData resultNewBmp = newBmp.LockBits(
                        new Rectangle(0, 0, newBmp.Width, newBmp.Height),
                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                    Marshal.Copy(pixelBuffer, 0, resultNewBmp.Scan0, pixelBuffer.Length);
                    newBmp.UnlockBits(resultNewBmp);

                    pictureBox1.Image = newBmp;
                }
            }
            catch
            {
                MessageBox.Show("Картинка не выбрана", "Ошибка");
                radioButton1.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = pictureBox1.Width * pictureBox1.Height;
            progressBar1.Value = progressBar1.Minimum;
            cent = (int)numericUpDown1.Value;
            chek = (cent == ty) ? true : false;
            if (color && chek)
            {
                radioButton1.Checked = true;
                contrast(cent);
                color = false;
            }
            else
            {
                radioButton2.Checked = true;
                contrast(cent);
                color = true;
                ty = (int)numericUpDown1.Value;
            }
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
