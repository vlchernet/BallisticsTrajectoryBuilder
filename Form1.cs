using System;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace BallisticsTrajectoryBuilder
{
    public partial class Form1 : Form
    {
        static Timer timer = new Timer();
        double x, y;
        double a;
        double t;
        double v;
        const double g = 9.81;
        double m;
        Graphics gr;

        public Form1()
        {
            InitializeComponent();
            t = 0;
            x = 0;
            y = 0;
            a = (double)numericUpDown1.Value;
            m = (double)numericUpDown3.Value;
            v = (double)numericUpDown2.Value;
            pictureBox1.Left = this.Left + 50;
            pictureBox1.Width = this.Width - 100;
            pictureBox1.Top = this.Top + 150;
            pictureBox1.Height = this.Height - pictureBox1.Top - 100;
            Paint += new PaintEventHandler(Form1_Paint_Init);
            timer.Interval = 50;
            gr = Graphics.FromHwnd(pictureBox1.Handle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            x = 0;
            y = 0;
            t = 0;
            label9.Text = "0";
            label12.Text = "0";
            Paint += new PaintEventHandler(Form1_Paint);
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Enabled = true;
        }

        private void Form1_Paint_Init(object sender, PaintEventArgs pe)
        {
            RedrawGrid();
        }

        private void RedrawGrid()
        {
            Pen p = new Pen(Color.Green, 2);
            SolidBrush solidBrush = new SolidBrush(this.ForeColor);
            int x = 0;
            int stepx = (int)(pictureBox1.Width * 0.1);
            while (x <= pictureBox1.Width)
            {
                gr.DrawLine(p, new Point(x, 0), new Point(x, pictureBox1.Height));
                gr.DrawString(Math.Round((x / m), 0).ToString(), this.Font, solidBrush, new Point(x, pictureBox1.Height - 15));
                x += stepx;
            }
            int y = 0;
            int stepy = (int)(pictureBox1.Height * 0.1);
            while (y <= pictureBox1.Height)
            {
                gr.DrawLine(p, new Point(0, y), new Point(pictureBox1.Width, y));
                gr.DrawString(Math.Round((y / m), 0).ToString(), this.Font, solidBrush, new Point(0, pictureBox1.Height - y));
                y += stepy;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs pe)
        {
            gr.FillEllipse(Brushes.Red, new Rectangle((int)(m * x), (int)(pictureBox1.Height - m * y), 4, 4));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            a = (double)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            v = (double)numericUpDown2.Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double vx, vy, vv;
            if (y >= 0 && m * y <= pictureBox1.Height && m * x <= pictureBox1.Width)
            {
                t += 0.05;
                label5.Text = Math.Round(t, 2).ToString();
                label7.Text = Math.Round(x, 2).ToString();
                vx = v * Math.Cos(a / 180 * Math.PI);
                vy = v * Math.Sin(a / 180 * Math.PI) - g * t;
                vv = Math.Sqrt(vx * vx + vy * vy);
                label12.Text = Math.Round(vv, 2).ToString();
                if (y > Double.Parse(label9.Text)) label9.Text = Math.Round(y, 2).ToString();
                x = v * Math.Cos(a / 180 * Math.PI) * t;
                y = v * Math.Sin(a / 180 * Math.PI) * t - g * t * t / 2;
                this.Text = "x = " + Math.Round(x, 2).ToString() + "  y = " + Math.Round(y, 2).ToString();
                Invalidate();
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            m = (double)numericUpDown3.Value;
            gr.Clear(SystemColors.Control);
            RedrawGrid();
        }


    }
}
