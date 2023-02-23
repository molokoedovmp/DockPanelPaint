using System;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WindowsFormsApplication1
{
	public partial class DocumentForm : DockContent
	{
        private int X, Y;
        private MainForm parentForm;
        private Graphics img;
        public bool localChanged;
        public Bitmap Image { get; set; }
        private Bitmap tmp { get; set; }
        public static int starEnd { get; set; } 
        public static int outerRadius { get; set; }
        public static int innerRadius { get; set; }

        #region Конструктор
        public DocumentForm(MainForm parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            Image = new Bitmap(parentForm.WidthImage, parentForm.HeightImage);
            tmp = new Bitmap(Image.Width, Image.Height);
            img = Graphics.FromImage(Image);
            img.Clear(Color.White);
            localChanged = true;
            starEnd = 5;
            this.Text = "Image" + $"{parentForm.count}";
        }
        public DocumentForm(MainForm parentForm, string filename)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            using (Stream s = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                Image = new Bitmap(s);
            }
            tmp = new Bitmap(Image.Width, Image.Height);
            this.Text = filename;
            localChanged = false;
            starEnd = 5;
        }
        public DocumentForm()
        {
            InitializeComponent ();
        }
        #endregion

        private void draw(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    switch (parentForm.tools)
                    {
                        case Tools.Pen:
                            img = Graphics.FromImage(Image);
                            var pen = new Pen(MainForm.penColor, MainForm.penSize);
                            pen.StartCap = LineCap.Round;
                            pen.EndCap = LineCap.Round;
                            img.DrawLine(pen, X, Y, e.X, e.Y);
                            X = e.X;
                            Y = e.Y;
                            Invalidate();
                            parentForm.changed = true;
                            localChanged = true;
                            break;
                        case Tools.Eraser:
                            img = Graphics.FromImage(Image);
                            var pen2 = new Pen(Color.White, MainForm.penSize);
                            pen2.StartCap = LineCap.Round;
                            pen2.EndCap = LineCap.Round;
                            img.DrawLine(pen2, X, Y, e.X, e.Y);
                            X = e.X;
                            Y = e.Y;
                            Invalidate();
                            parentForm.changed = true;
                            localChanged = true;
                            break;
                        case Tools.Line:
                            tmp = new Bitmap(Image.Width, Image.Height);
                            using (var g = Graphics.FromImage(tmp))
                            {
                                g.DrawLine(new Pen(MainForm.penColor, MainForm.penSize), X, Y, e.X, e.Y);
                            }
                            Invalidate();
                            break;
                        case Tools.Ellipse:
                            tmp = new Bitmap(Image.Width, Image.Height);
                            using (var g = Graphics.FromImage(tmp))
                            {
                                g.DrawEllipse(new Pen(MainForm.penColor, MainForm.penSize), X, Y, e.X - X, e.Y - Y);
                            }
                            Invalidate();
                            break;
                        case Tools.Star:
                            tmp = new Bitmap(Image.Width, Image.Height);
                            PointF[] pts = StarPoints(starEnd, outerRadius, innerRadius, new Rectangle(new Point(X, Y), new Size(e.X - X, e.Y - Y)));
                            using (var g = Graphics.FromImage(tmp))
                            {
                                g.DrawPolygon(new Pen(MainForm.penColor, MainForm.penSize), pts);
                            }
                            Invalidate();
                            break;
                    }
                }
                parentForm.changeXY(e.X, e.Y);
            }
            catch
            {

            }
        }
        private void DocumentForm_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (parentForm.tools == Tools.Line)
                {
                    img = Graphics.FromImage(Image);
                    img.DrawLine(new Pen(MainForm.penColor, MainForm.penSize), X, Y, e.X, e.Y);
                    tmp = new Bitmap(1, 1);
                    Invalidate();
                    parentForm.changed = true;
                    localChanged = true;
                }
                if (parentForm.tools == Tools.Ellipse)
                {
                    img = Graphics.FromImage(Image);
                    img.DrawEllipse(new Pen(MainForm.penColor, MainForm.penSize), X, Y, e.X - X, e.Y - Y);
                    tmp = new Bitmap(1, 1);
                    Invalidate();
                    parentForm.changed = true;
                    localChanged = true;
                }
                if (parentForm.tools == Tools.Star)
                {
                    img = Graphics.FromImage(Image);
                    PointF[] pts = StarPoints(starEnd, outerRadius,innerRadius,new Rectangle(new Point(X, Y), new Size(e.X - X, e.Y - Y)));
                    img.DrawPolygon(new Pen(MainForm.penColor, MainForm.penSize), pts);
                    tmp = new Bitmap(1,1);
                    Invalidate();
                    parentForm.changed = true;
                    localChanged = true;
                }
            }
            catch { }
        }
        public void newUpdate()
        {
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(Image, 0, 0); // 0 0 - место на форме с которого срисовывается
            if (parentForm.tools == Tools.Line || parentForm.tools == Tools.Ellipse || parentForm.tools == Tools.Star)
                e.Graphics.DrawImage(tmp, 0, 0);
        }
        public void changeSize()
        {
            try
            {
                Bitmap tmp = (Bitmap)Image.Clone();
                Image = new Bitmap(parentForm.WidthImage, parentForm.HeightImage);
                img = Graphics.FromImage(Image);
                img.Clear(Color.White);
                for (int Xcount = 0; Xcount < tmp.Width && Xcount < Image.Width; Xcount++)
                {
                    for (int Ycount = 0; Ycount < tmp.Height && Ycount < Image.Height; Ycount++)
                    {
                        Image.SetPixel(Xcount, Ycount, tmp.GetPixel(Xcount, Ycount));
                    }
                }
                Invalidate();
                parentForm.changed = true;
                localChanged = true;
            }
            catch { }
        } //изменение размера холста
        private void DocumentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (localChanged)
            {
                var r = MessageBox.Show($"Изображение {this.Text} было изменено. Сохранить?", "Сохранение изображения", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (r)
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.Yes:
                        parentForm.сохранитьToolStripMenuItem_Click(sender, e);
                        break;
                }
            }
            if (parentForm.MdiChildren.Length == 1)
                parentForm.changed = false;
        }
        private void DocumentForm_MouseDown(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;
        }
        private void DocumentForm_MouseLeave(object sender, EventArgs e)
        {
            parentForm.changeXY(0, 0);
        }
        private void DocumentForm_MouseMove(object sender, MouseEventArgs e)
        {
            draw(sender,e);
        }
        private PointF[] StarPoints(int num_points, int outer, int inner ,Rectangle bounds)
        {
            //int R = bounds.X, r = bounds.Y;   // радиусы
            int R = outer, r = inner;
            double alpha = 0;        // поворот
            double rx = bounds.Width;
            double ry = bounds.Height;
            try
            {
                PointF[] pts = new PointF[2 * num_points + 1];
                double a = alpha, da = Math.PI / num_points, l;

                for (int k = 0; k < 2 * num_points + 1; k++)
                {
                    l = k % 2 == 0 ? r : R;
                    pts[k] = new PointF((float)(rx + l * Math.Cos(a)), (float)(ry + l * Math.Sin(a)));
                    a += da;
                }


                return pts;
            }
            catch
            {
                return null;
            }

        }

    }
}
