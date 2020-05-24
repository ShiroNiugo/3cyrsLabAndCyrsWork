using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        double Xn, Xh, dX, x, z;
        string namefile = "", file = "Text files (*.txt)|*.txt";
        public Form1()
        {
            InitializeComponent();
            chart1.Series[0].ChartType = SeriesChartType.Spline;
        }

        void clearForm()
        {
            dataGridView1.Rows.Clear();
            chart1.Series[0].Points.Clear();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = file;
            openFileDialog1.FileName = namefile;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                clearForm();

                foreach (string line in File.ReadLines(openFileDialog1.FileName))
                {
                    string[] array = line.Split();
                    dataGridView1.Rows.Add(array);
                    int n = array.Length;
                    for (int i = 0; i < array.Length; ++i)
                    {
                        for (int j = 0; j < dataGridView1.ColumnCount; ++j)
                        {
                            if (j == 0)
                            {
                                x = Convert.ToDouble(array[j]);
                            }
                            else
                            {
                                z = Convert.ToDouble(array[j]);
                                chart1.Series[0].Points.AddXY(x, z);
                            }
                        }
                    }
                }
                saveFileDialog1.FileName = openFileDialog1.FileName;
                namefile = openFileDialog1.SafeFileName;
            }
        }

        void save()
        {
            string[] lines = new string[dataGridView1.RowCount - (dataGridView1.AllowUserToAddRows ? 1 : 0)];
            string[] values = new string[dataGridView1.Columns.Count];
            for (int i = 0; i < lines.Length; ++i)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; ++j)
                {
                    values[j] = (string)dataGridView1.Rows[i].Cells[j].Value;
                    lines[i] = string.Join("\t", values);
                }
            }
            File.WriteAllLines(saveFileDialog1.FileName, lines);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.FileName == openFileDialog1.FileName)
                saveFileDialog1.FileName = openFileDialog1.FileName;
            else openFileDialog1.FileName = saveFileDialog1.FileName;
            try
            {
                if (namefile != "") save();
                else сохранитьКакToolStripMenuItem_Click(sender, e);
            }catch
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
                save();
            }
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text = textBox3.Text = null;
            clearForm();
            namefile = "";
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Студент группы ЭИС-26\nКапустин Д.Е.", "О программе");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Xn = Convert.ToDouble(textBox1.Text);
                Xh = Convert.ToDouble(textBox2.Text);
                dX = Convert.ToDouble(textBox3.Text);

                double o = (Xh - Xn) / dX;

                clearForm();
                if (Xn < Xh)
                {
                    for (x = Xn; x <= Xh; x += o)
                    {
                        z = x * x;
                        dataGridView1.Rows.Add(x.ToString(), z.ToString());
                        chart1.Series[0].Points.AddXY(x, z);
                    }
                }
                else
                {
                    MessageBox.Show("Перепроверьте данные", "Ошибка");
                }
            }
            catch
            {
                MessageBox.Show("Данные пусты или заполнены неправильно", "Ошибка");
            }
            
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите выйти?", "Выход", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (dataGridView1.Rows[0].Cells[0].Value != null)
                {
                    сохранитьКакToolStripMenuItem_Click(sender, e);
                }
                Close();
            }
        }
    }
}
