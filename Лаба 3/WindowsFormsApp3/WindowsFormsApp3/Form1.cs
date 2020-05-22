using System;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        string namefile = "", file = "txt files (*.rtf)|*.rtf";
        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = file;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(openFileDialog1.FileName);
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
                if (namefile != "")
                    richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                else сохранитьКакToolStripMenuItem_Click(sender, e);
            }
            catch
            {
                MessageBox.Show("Файл для сохранения не найден!");
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = file;
            saveFileDialog1.FileName = namefile;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите выйти?", "Выход", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (richTextBox1.Text != "")
                {
                    сохранитьКакToolStripMenuItem_Click(sender, e);
                }
                Close();
            }
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = richTextBox1.SelectionBackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionBackColor = colorDialog1.Color;
            }
        }

        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionFont = fontDialog1.Font;
            }
        }

        private void цветВыделенногоТекстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = richTextBox1.SelectionColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = colorDialog1.Color;
            }
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Капустин Д.Е.", "О программе");
        }
    }
}
