using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Curs
{
    public partial class Form1 : Form
    {
        int cent, ty;
        public bool color = false, chek = false;
        const int L = 256;
        double red, green, blue;
        

        public Bitmap bmp, newBmp;

        public Form1()
        {
            InitializeComponent();
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

                        progressBar1.Value += progressBar1.Value != progressBar1.Maximum ? 1 : 0;
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
            progressBar1.Maximum = pictureBox1.Width * pictureBox1.Height / 100;
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
    }
}
