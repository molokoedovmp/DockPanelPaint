﻿using System;
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
        public StarSettings()
        {
            InitializeComponent();
        }
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
            //m.tools = Tools.Star;
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
        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int t) || textBox2.Text == "")
            {
                if (t <= 0 && textBox2.Text != "" && t <= 60)
                {
                    MessageBox.Show("Число должно быть положительным, меньше 60");
                    textBox2.Clear();
                }
            }
            else
            {
                MessageBox.Show("Вы ввели символ, вводите цифры!");
                textBox2.Clear();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox3.Text, out int r) || textBox3.Text == "")
            {
                if (r <= 0 && textBox3.Text != "" && r <= 60)
                {
                    MessageBox.Show("Число должно быть положительным, меньше 60");
                    textBox1.Clear();
                }
            }
            else
            {
                MessageBox.Show("Вы ввели символ, вводите цифры!");
                textBox3.Clear();
            }
        }
    }
}
