using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class StarSettings : Form
    {
        MainForm m;

        public StarSettings(MainForm m)
        {
            InitializeComponent();
            this.m = m;
            textBox1.Text = $"{DocumentForm.starEnd}";
            textBox2.Text = $"{DocumentForm.outerRadius}";
            textBox3.Text = $"{DocumentForm.innerRadius}";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            m.tools = Tools.Star;
            if (int.TryParse(textBox1.Text, out int a))
            {
                DocumentForm.starEnd = a;
            }
            if (int.TryParse(textBox2.Text, out int b))
            {
                DocumentForm.outerRadius = b;
            }
            if (int.TryParse(textBox3.Text, out int c))
            {
                DocumentForm.innerRadius = c;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Checker();
        }
        private void Checker()
        {
            if (int.TryParse(textBox1.Text, out int w) || textBox1.Text == "")
            {
                if (w <= 0 && textBox1.Text != "" && w <= 60)
                {
                    MessageBox.Show("Число должно быть положительным, меньше 60");
                    textBox1.Clear();
                }
            }
            else
            {
                MessageBox.Show("Вы ввели символ, вводите цифры!");
                textBox1.Clear();
            }
        }
    }
}
