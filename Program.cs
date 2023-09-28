using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace HoughWithCSharp
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static int id;
        public static double[,] positions = new double[100, 4];

        //public static double[,] pointsVertical = new double[100, 2];
        //public static double[,] pointsCross = new double[100, 3];
        public static List<Point3D> points = new List<Point3D>();
        public static List<int> pointsID = new List<int>();
        public static List<PointD> pointsToPaint = new List<PointD>();

        public static void DoAnalyse()
        {
            double pLineIngM = -1.0;
            double pLineIngN = -1.0;
            double[,] pLineED = new double[100, 2];
            Boolean isWritenED = false;

            //traverse positions[][][][]
            for (int i = 0; i < 100; i++)
            {
                pLineIngM = positions[i, 2]; //m
                pLineIngN = positions[i, 3]; //n
                if (pLineIngM == 0.0 || pLineIngN == 0.0)
                {
                    continue;
                }

                if (!isWritenED)
                {
                    pLineED[0, 0] = pLineIngM;
                    pLineED[0, 1] = pLineIngN;
                    isWritenED = true;
                    continue;
                }

                //traverse pLineED[][]
                for (int j = 0; j < i; j++)
                {
                    //no cross
                    //if (pLineIngM == pLineED[j, 0])
                    //{
                    //    continue;
                    //}

                    //have cross
                    double m1, n1;
                    double m2, n2;
                    double bb, kk;
                    m1 = pLineIngM;
                    n1 = pLineIngN;
                    m2 = pLineED[j, 0];
                    n2 = pLineED[j, 1];
                    bb = (n2 - n1) / (m1 - m2);
                    kk = m1 * bb + n1;
                    //get point: (bb, kk) where two lines crossed

                    Point3D p = new Point3D(bb, kk, 1.0);
                    DoAnalysePoints(p);
                }

                pLineED[i, 0] = pLineIngM;
                pLineED[i, 1] = pLineIngN;
            }

            int maxTimes = 0;
            int curTimes;
            int num1 = points.Count;
            for (int i = 0; i < num1; i++)
            {
                curTimes = (int)points[i].Z;
                if (curTimes > maxTimes)
                {
                    maxTimes = curTimes;
                    pointsID.Clear();
                    pointsID.Add(i);
                }
                else if (curTimes == maxTimes)
                {
                    pointsID.Add(i);
                }
                else
                {
                    //
                }
            }

            int num2 = pointsID.Count;
            for (int i = 0; i < num2; i++)
            {
                int id = pointsID[i];
                PointD pt = new PointD();
                pt.X = points[id].X;
                pt.Y = points[id].Y;
                pointsToPaint.Add(pt);
            }
        }

        private static void DoAnalysePoints(Point3D p)
        {
            if (!points.Any())
            {
                //empty list!
                points.Add(p);
                return;
            }

            //else, not empty list:
            int num = points.Count; //how many exsit ED !
            int idInPoints = 0;
            bool sameAsAny = false;
            for (int i = 0; i < num; i++)
            {
                Point3D pInList = points[i];
                double distance = Math.Pow((pInList.X - p.X), 2.0) + Math.Pow((pInList.Y - p.Y), 2.0);
                if (distance < 1000.0)
                {
                    //two points close, should be think as one point
                    idInPoints = i;
                    sameAsAny = true;
                    break;
                }
            }

            if (sameAsAny)
            {
                //two points close, should be think as one point
                Point3D pp = new Point3D();
                pp.X = points[idInPoints].X;
                pp.Y = points[idInPoints].Y;
                pp.Z = points[idInPoints].Z + 1.0;
                points[idInPoints] = pp;
            }
            else
            {
                //normal, should add this new point into the list
                points.Add(p);
            }
        }
    }

    public struct PointD
    {
        public double X;
        public double Y;

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point ToPoint()
        {
            return new Point((int)X, (int)Y);
        }
    }
}
