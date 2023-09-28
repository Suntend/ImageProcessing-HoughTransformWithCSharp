using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;

namespace HoughWithCSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            Program.id = 0;
            Array.Clear(Program.positions, 0, Program.positions.Length);
            Program.points.Clear();
            Program.pointsID.Clear();
            Program.pointsToPaint.Clear();
            panel1.Refresh();
        }

        //start
        private void button1_Click(object sender, EventArgs e)
        {
            Program.DoAnalyse();

            int num = Program.pointsToPaint.Count;
            for (int i = 0; i < num; i++)
            {
                //retraverse
                double b = Program.pointsToPaint[i].X;
                double k = Program.pointsToPaint[i].Y;
                double y = k * 1150 + b;
                Point pt1 = new Point(0, (int)b);
                Point pt2 = new Point(1150, (int)y);

                doPaintLine(pt1, pt2);
            }
        }

        //clean
        private void button2_Click(object sender, EventArgs e)
        {
            init();
        }

        //record
        private void panelMouseDown(object sender, MouseEventArgs e)
        {
            if (Program.id >= 100){return;}

            Point mp = panel1.PointToClient(Control.MousePosition);
            double m = - 1.0 / (double)mp.X;
            double n = (double)mp.Y / (double)mp.X;
            int[] toPaint = new int[] { mp.X, mp.Y };
            doPaint(toPaint);
            
            Program.positions[Program.id, 0] = mp.X;
            Program.positions[Program.id, 1] = mp.Y;
            Program.positions[Program.id, 2] = m;
            Program.positions[Program.id, 3] = n;

            Program.id++;
        }

        private void doPaint(int[] i)
        {
            int x = i[0];
            int y = i[1];
            int r = 5;
            Graphics g = panel1.CreateGraphics();
            g.FillEllipse(Brushes.Black, x - r, y - r, r * 2, r * 2);
        }

        public void doPaintLine(Point pt1, Point pt2)
        {
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(Color.Blue, 2f);
            g.DrawLine(pen, pt1, pt2);
        }
    }
}