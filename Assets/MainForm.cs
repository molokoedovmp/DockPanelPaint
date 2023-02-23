using System;
using System.Linq;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;

namespace WindowsFormsApplication1
{
	public partial class MainForm : Form
	{
		public static Color penColor { get; set; }
		public static int penSize { get; set; }
		public int WidthImage { get; set; }
		public int HeightImage { get; set; }
		public bool changed { get; set; }
		public Tools tools { get; set; }
		public int count { get; set; }

		public MainForm()
		{
			InitializeComponent();
			penColor = Color.Black;
			penSize = 3;
			WidthImage = 1920;
			HeightImage = 1080;
			changed = false;
			tools = Tools.Pen;
			toolStripTextBox2.Text = $"{penSize}";
			count = 1;
		}

        #region Новый файл
        private void Form1_Load(object sender, EventArgs e)
		{
			this.IsMdiContainer = true;
		}
		private void toolStripButtonNew_Click(object sender, EventArgs e)
		{
			DocumentForm formChild = CreateChild();
			if(dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
			{
				formChild.MdiParent = this;
				formChild.Show();
			}
			else
			{
				formChild.Show(dockPanel1);
			}
		}
		private DocumentForm CreateChild()
		{
			DocumentForm formChild = new DocumentForm(this);
			formChild.MdiParent = this;
			int count = 1;
			string text = $"Новое окно {count}";
			while(ChildExist(text))
			{
				count++;
				text = $"Новое окно {count}";
			}
			formChild.Text = text;
			return formChild;
		}
		private bool ChildExist(string text)
		{
			bool flag = false;
			if(dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
			{
				if(MdiChildren.Any(form => form.Text.Equals(text)))
				{
					flag = true;
				}
			}
			else
			{
				if(dockPanel1.Contents.Any(item => item.DockHandler.TabText.Equals(text)))
				{
					flag = true;
				}
			}
			return flag;
		}
        #endregion

        #region вкладка Файл
        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
			DocumentForm img = new DocumentForm(this);
			img.MdiParent = this;
			img.Show();
			count++;
		}

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "All files|*.*|*.bmp|*.bmp|*.jpg|*.jpg|*.png|*.png";
			if (dlg.ShowDialog() == DialogResult.OK && checkopen(dlg.FileName))
			{
				DocumentForm img = new DocumentForm(this, dlg.FileName);
				if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
				{
					img.MdiParent = this;
					img.Show();
				}
				else
				{
					img.Show(dockPanel1);
				}
			}
		}
		private bool checkopen(string s)
		{
			if (s[s.Length - 3] == 'b' && s[s.Length - 2] == 'm' && s[s.Length - 1] == 'p' || s[s.Length - 3] == 'j' && s[s.Length - 2] == 'p' && s[s.Length - 1] == 'g' || s[s.Length - 3] == 'p' && s[s.Length - 2] == 'n' && s[s.Length - 1] == 'g')
				return true;
			else
			{
				MessageBox.Show("Неподходящий формат файла!");
				return false;
			}
		}

        public void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (ActiveMdiChild != null && checkopen2(ActiveMdiChild.Text))
				сохранитьКакToolStripMenuItem_Click(sender, e);
			else
			{
				((DocumentForm)ActiveMdiChild).Image.Save(ActiveMdiChild.Text);
				changed = true;
				((DocumentForm)ActiveMdiChild).localChanged = false;
			}
		}
		private bool checkopen2(string s)
		{
			if (s[0] == 'I' && s[1] == 'm' && s[2] == 'a' || s[3] == 'j' && s[4] == 'e')
				return true;
			else
				return false;
		}

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
			var dlg = new SaveFileDialog();
			dlg.Filter = "*.bmp|*.bmp|*.jpg|*.jpg|*.png|*.png|All files|*.*";
			dlg.FileName = $"{((DocumentForm)ActiveMdiChild).Text}";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				((DocumentForm)ActiveMdiChild).Image.Save(dlg.FileName);
				changed = true;
				((DocumentForm)ActiveMdiChild).localChanged = false;
				ActiveMdiChild.Text = dlg.FileName;
			}
		}

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
			Application.Exit();
		}
		#endregion

		#region выбор цвета
		private void выходToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
			Application.Exit();
        }

        private void другойToolStripMenuItem_Click(object sender, EventArgs e)
        {
			var colorChoice = new ColorDialog();
			if (colorChoice.ShowDialog() == DialogResult.OK)
				penColor = colorChoice.Color;
		}

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
			penColor = Color.Red;
		}

        private void синийToolStripMenuItem_Click(object sender, EventArgs e)
        {
			penColor = Color.Blue;
		}

        private void зелёныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
			penColor = Color.Green;
		}
		#endregion

        #region Выбор кисти
        private void DeleteImage()
		{
			пероToolStripMenuItem.Image = null;
			линияToolStripMenuItem.Image = null;
			эллипсToolStripMenuItem.Image = null;
			ластикToolStripMenuItem.Image = null;
			звёздочкаToolStripMenuItem.Image = null;
		}

        private void пероToolStripMenuItem_Click(object sender, EventArgs e)
        {
			tools = Tools.Pen;
			DeleteImage();
		}

        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
			tools = Tools.Line;
			DeleteImage();
		}

        private void эллипсToolStripMenuItem_Click(object sender, EventArgs e)
        {
			tools = Tools.Ellipse;
			DeleteImage();
		}

        private void ластикToolStripMenuItem_Click(object sender, EventArgs e)
        {
			tools = Tools.Eraser;
			DeleteImage();
		}

        private void звёздочкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
			tools = Tools.Star;
			DeleteImage();
		}


        private void звездаToolStripMenuItem_Click(object sender, EventArgs e)
        {
			StarSettings s = new StarSettings(this);
			if (s.ShowDialog() == DialogResult.OK)
				звёздочкаToolStripMenuItem_Click(sender, e);
		}
        #endregion

		#region доступность toolstrip 
		private void окноToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
			упорядочитьToolStripMenuItem.Enabled = ActiveMdiChild != null;
		}

        private void рисунокToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
			размерХолстаToolStripMenuItem.Enabled = !(ActiveMdiChild == null);
			звездаToolStripMenuItem.Enabled = !(ActiveMdiChild == null);
		}
        private void toolStripMenuItem1_DropDownOpening(object sender, EventArgs e)
        {
			сохранитьКакToolStripMenuItem.Enabled = ActiveMdiChild != null;
			сохранитьToolStripMenuItem.Enabled = ActiveMdiChild != null;
		}



		#endregion

		#region расположение окон
		private void каскадToolStripMenuItem_Click(object sender, EventArgs e)
        {
			this.dockPanel1.UpdateDockWindowZOrder(DockStyle.Right, true);
		}
        private void слеваНаправоToolStripMenuItem_Click(object sender, EventArgs e)
        {
			LayoutMdi(MdiLayout.TileVertical);
		}

        private void сверхуВнизToolStripMenuItem_Click(object sender, EventArgs e)
        {
			LayoutMdi(MdiLayout.TileHorizontal);
		}
        private void упорядочитьЗначкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
			LayoutMdi(MdiLayout.ArrangeIcons);
		}

        #endregion

        #region Меню
        public void changeXY(int X, int Y)
		{
			toolStripStatusLabel1.Text = $"X:{X}, Y:{Y}";
		} //показ координат на документе

        private void toolStripTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
			if (e.KeyCode == Keys.Enter)
			{
				if (int.TryParse(toolStripTextBox2.Text, out int w))
				{
					if (penSize > 0)
						penSize = w;
				}
				toolStripTextBox2.Text = $"{penSize}";
			}
		}

        private void размерХолстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
			CanvasSizeForm size = new CanvasSizeForm(this);
			if (size.ShowDialog() == DialogResult.OK)
				((DocumentForm)ActiveMdiChild).changeSize();
		}

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (changed && ActiveMdiChild != null && ((DocumentForm)ActiveMdiChild).localChanged == false)
			{
				var r = MessageBox.Show($"Изображение {((DocumentForm)ActiveMdiChild).Text} было изменено. Сохранить?", "Сохранение изображения", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				switch (r)
				{
					case DialogResult.Cancel:
						e.Cancel = true;
						break;
					case DialogResult.Yes:
						сохранитьToolStripMenuItem_Click(sender, e);
						break;
				}
			}
		}

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
			if (int.TryParse(toolStripTextBox2.Text, out int w) || toolStripTextBox2.Text == "")
			{
				if (w <= 0 && toolStripTextBox2.Text != "")
				{
					MessageBox.Show("Вы ввели отрицательное число, введите положительное!");
					toolStripTextBox2.Clear();
				}
			}
			else
			{
				MessageBox.Show("Вы ввели символ, вводите цифры!");
				toolStripTextBox2.Clear();
			}
		}

        private void посмотретьСправкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
			new AboutBox().ShowDialog();
        }
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
			сохранитьКакToolStripMenuItem_Click(sender, e);

		}
        #endregion

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
			

			try
			{
				
				if (ActiveMdiChild != null)
				{
					((DocumentForm)ActiveMdiChild).Image = new Bitmap(((DocumentForm)ActiveMdiChild).Image, new Size(((DocumentForm)ActiveMdiChild).Image.Width + 300, ((DocumentForm)ActiveMdiChild).Image.Height + 300));
					((DocumentForm)ActiveMdiChild).newUpdate();
				}
			}
			catch
			{

			}
		}

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
			

			try
			{
				
				if (ActiveMdiChild != null)
				{
					((DocumentForm)ActiveMdiChild).Image = new Bitmap(((DocumentForm)ActiveMdiChild).Image, new Size(((DocumentForm)ActiveMdiChild).Image.Width - 300, ((DocumentForm)ActiveMdiChild).Image.Height - 300));
					((DocumentForm)ActiveMdiChild).newUpdate();
				}
			}
			catch
			{

			}
		}
    }
}
