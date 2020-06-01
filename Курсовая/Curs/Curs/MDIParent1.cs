using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Curs
{
    public partial class MDIParent1 : Form
    {
        private ImageFormat[] formats = {ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Png };
        private string namefile = "", file = "BMP files (*.BMP)|*.bmp| JPG files (*.JPG,)|*.jpg| PNG files (*.PNG)|*.png",
                       nf = "Окно контрастности ";
        public int chekid;

        public SaveFileDialog saveFileDialog = new SaveFileDialog();
        public OpenFileDialog openFileDialog = new OpenFileDialog();
        
        public Form1 actForm;

        public MDIParent1()
        {
            InitializeComponent();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            openFileDialog.Filter = file;
            openFileDialog.FileName = namefile;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Form1 childForm = new Form1();
                childForm.MdiParent = this;
                childForm.bmp = new Bitmap(Image.FromFile(openFileDialog.FileName));
                childForm.pictureBox1.Image = childForm.bmp;
                namefile = openFileDialog.SafeFileName;
                saveFileDialog.FileName = openFileDialog.FileName;
                childForm.Text = nf + namefile;
                childForm.Name = openFileDialog.FileName;
                childForm.Show();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actForm = (Form1)this.ActiveMdiChild;
            string actName = actForm.Text.Remove(0, nf.Length);
            if (openFileDialog.FileName == saveFileDialog.FileName && actForm.Name==saveFileDialog.FileName)
            {
                saveFileDialog.FileName = openFileDialog.FileName;
            }
            else
            {
                saveFileDialog.FileName = saveFileDialog.FileName.Replace(saveFileDialog.FileName, actForm.Name);
                openFileDialog.FileName = saveFileDialog.FileName;
            }
            try
            {
                actForm.newBmp = new Bitmap(actForm.pictureBox1.Image);
                actForm.newBmp.Save(saveFileDialog.FileName, formats[saveFileDialog.FilterIndex-1]);
                actForm.bmp = actForm.newBmp;
            }
            catch
            {
                MessageBox.Show("Файл для сохранения не найден!", "Ошибка");
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (namefile != "")
            {
                saveFileDialog.Filter = file;
                saveFileDialog.FileName = namefile;
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    actForm = (Form1)this.ActiveMdiChild;
                    actForm.bmp = new Bitmap(actForm.pictureBox1.Image);
                    actForm.bmp.Save(saveFileDialog.FileName, formats[saveFileDialog.FilterIndex - 1]);
                    actForm.Name = saveFileDialog.FileName;
                    actForm.Text = nf + System.IO.Path.GetFileName(saveFileDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Для сохранения ничего не открыто", "Ошибка");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            actForm = (Form1)this.ActiveMdiChild;
            RadioButton radio = sender as RadioButton;
            chekid = radio.TabIndex;
            try
            {
                switch (chekid)
                {
                    case 26:
                        {
                            actForm.vcl = false;
                            actForm.pictureBox1.Image = actForm.bmp;
                            break;
                        }
                    case 27:
                        {
                            if (actForm.newBmp != null)
                            {
                                actForm.vcl = true;
                                actForm.pictureBox1.Image = actForm.newBmp;
                            }
                            break;
                        }
                }
            }
            catch
            {
                radioButton1.Checked = true;
            }
}

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                actForm = (Form1)this.ActiveMdiChild;
                actForm.progressBar1.Maximum = actForm.pictureBox1.Width * actForm.pictureBox1.Height / 100;
                actForm.cent = (double)numericUpDown1.Value;
                actForm.chek = (actForm.cent == actForm.ty) ? true : false;
                if (actForm.color && actForm.chek && radioButton2.Checked)
                {
                    radioButton1.Checked = (actForm.cent != actForm.ty) ? false : true;
                    contrast(actForm.cent);
                    actForm.color = actForm.vcl = false;
                }
                else
                {
                    radioButton2.Checked = actForm.vcl = actForm.color = true;
                    contrast(actForm.cent);
                    actForm.ty = (double)numericUpDown1.Value;
                }
            }
            catch
            {
                MessageBox.Show("Картинка не была загружена", "Ошибка");
            }
        }

        public void contrast(double cent)
        {
            actForm = (Form1)this.ActiveMdiChild;
            if (radioButton2.Checked && actForm.cent != actForm.ty)
            {
                actForm.progressBar1.Value = actForm.progressBar1.Minimum;
                BitmapData bdata = actForm.bmp.LockBits(
                    new Rectangle(0, 0, actForm.bmp.Width, actForm.bmp.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[bdata.Stride * bdata.Height];
                Marshal.Copy(bdata.Scan0, pixelBuffer, 0, pixelBuffer.Length);

                actForm.bmp.UnlockBits(bdata);

                double contrastLevel = 1.0 + cent / 100.0;

                for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
                {
                    actForm.red = ((((pixelBuffer[k] / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;
                    actForm.green = ((((pixelBuffer[k + 1] / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;
                    actForm.blue = ((((pixelBuffer[k + 2] / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;

                    actForm.red = (actForm.red < 0) ? 1 : (actForm.red > 255) ? 255 : actForm.red;
                    actForm.green = (actForm.green < 0) ? 1 : (actForm.green > 255) ? 255 : actForm.green;
                    actForm.blue = (actForm.blue < 0) ? 1 : (actForm.blue > 255) ? 255 : actForm.blue;

                    pixelBuffer[k] = (byte)actForm.red;
                    pixelBuffer[k + 1] = (byte)actForm.green;
                    pixelBuffer[k + 2] = (byte)actForm.blue;

                    actForm.progressBar1.Value += actForm.progressBar1.Value != actForm.progressBar1.Maximum ? 1 : 0;
                }

                actForm.newBmp = new Bitmap(actForm.bmp.Width, actForm.bmp.Height);
                BitmapData resultNewBmp = actForm.newBmp.LockBits(
                    new Rectangle(0, 0, actForm.newBmp.Width, actForm.newBmp.Height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                Marshal.Copy(pixelBuffer, 0, resultNewBmp.Scan0, pixelBuffer.Length);
                actForm.newBmp.UnlockBits(resultNewBmp);

                actForm.pictureBox1.Image = actForm.newBmp;
            }
        }
        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите выйти?", "Выход", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void helpMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Курсовая работа.\nВыполил студент группы ЭИС-26\nКапустин Д.Е.", "О программе");
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

    }
}
