using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Curs
{
    public partial class MDIParent1 : Form
    {
        private int fff;
        private ImageFormat[] formats = {ImageFormat.Bmp, ImageFormat.Jpeg, ImageFormat.Png };
        private string namefile = "", file = "BMP files (*.BMP)|*.bmp| JPG files (*.JPG,)|*.jpg| PNG files (*.PNG)|*.png",
                       nf = "Окно контрастности ";

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
                fff = openFileDialog.FilterIndex - 1;
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
                actForm.newBmp.Save(saveFileDialog.FileName, formats[fff]);
                actForm.bmp = actForm.newBmp;
            }
            catch
            {
                MessageBox.Show("Файл для сохранения не найден!", "Ошибка");
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = file;
            saveFileDialog.FileName = namefile;
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                actForm = (Form1)this.ActiveMdiChild;
                actForm.bmp = new Bitmap(actForm.pictureBox1.Image);
                actForm.bmp.Save(saveFileDialog.FileName, formats[fff]);
                actForm.Name = saveFileDialog.FileName;
                actForm.Text = nf + System.IO.Path.GetFileName(saveFileDialog.FileName);
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void helpMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Курсовая работа.\nКапустин Д.Е. ЭИС-26", "О программе");
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
