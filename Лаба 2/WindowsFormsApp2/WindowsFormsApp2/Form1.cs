using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        string namefile = "", file = "txt files (*.txt)|*.txt";
        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = file;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(openFileDialog1.FileName);
                saveFileDialog1.FileName = openFileDialog1.FileName;
                namefile = openFileDialog1.SafeFileName;
            }
        }

        void save()
        {
            File.WriteAllText(saveFileDialog1.FileName, textBox1.Text);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.FileName == openFileDialog1.FileName)
                saveFileDialog1.FileName = openFileDialog1.FileName;
            else openFileDialog1.FileName = saveFileDialog1.FileName;
            try
            {
                if (saveFileDialog1.FileName != "") save();
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
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) save();
        }




        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите выйти?", "Выход", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                сохранитьToolStripMenuItem_Click(sender, e);
                Close();
            }
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = textBox1.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK) {
                textBox1.BackColor = colorDialog1.Color;
            }
        }

        private void цветТекстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = textBox1.ForeColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK) {
                
                textBox1.ForeColor = colorDialog1.Color;
            }
        }
        private void fontDialog1_Apply(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK) {
                textBox1.Font = fontDialog1.Font;
            }
        }


        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Студент группы ЭИС-26\nКапустин Д.Е.", "О программе");
        }
    }
}
