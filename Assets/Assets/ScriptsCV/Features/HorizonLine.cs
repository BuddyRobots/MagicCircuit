using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using OpenCVForUnity;
using System.IO;
using System;

namespace OpenCVForUnitySample
{

    public class HorizonLine : Feature
    {
        public Point left;
        public Point right;

        public HorizonLine(MyObject obj)
        {
            m_Obj = obj;
        }

        public override void Calculate()
        {
            Mat lines = new Mat();
            Mat tmp = new Mat();

            Core.bitwise_not(m_Obj.erodeImg, tmp);
            int threshold = 50;
            Imgproc.HoughLinesP(tmp, lines, 1, Mathf.PI / 180, threshold, 30, 20);
            List<List<Point>> linePts = new List<List<Point>>();


            int[] linesArray = new int[lines.cols() * lines.rows() * lines.channels()];
            lines.get(0, 0, linesArray);

            for (int i = 0; i < linesArray.Length; i = i + 4)
            {
                List<Point> oneLine = new List<Point>();
                oneLine.Add(new Point(linesArray[i + 0], linesArray[i + 1]));
                oneLine.Add(new Point(linesArray[i + 2], linesArray[i + 3]));
                linePts.Add(oneLine);
            }

            /// find out the longest line
            Point p1 = new Point(), p2 = new Point();
            double len = 0, cur_len;
            for (int i = 0; i < linePts.Count; i++)
            {
                cur_len = FigProc.FindLen(linePts[i][0], linePts[i][1]);
                if (cur_len > len)
                {
                    len = cur_len;
                    p1 = linePts[i][0];
                    p2 = linePts[i][1];
                }
            }

            /// judge which point is on the left and which is on the right
            float angle = Mathf.Asin((float)((p2.y - p1.y) / len)) * 180 / Mathf.PI;

            Mat trans = Imgproc.getRotationMatrix2D(p1, angle, 1.0f);
            List<Point> endPts = new List<Point>();
            List<Point> points = new List<Point>();
            endPts.Add(p1);
            endPts.Add(p2);

            MatOfPoint endPtsMat = new MatOfPoint(endPts.ToArray());
            Core.transform(endPtsMat, endPtsMat, trans);
            endPts = endPtsMat.toList();

            MatOfPoint pointsMat = new MatOfPoint();
            Core.transform(new MatOfPoint(m_Obj.imgPts.ToArray()), pointsMat, trans);
            points = pointsMat.toList();

            double y = endPts[0].y;

            int above = 0, below = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].y < y)
                    above++;
                if (points[i].y > y)
                    below++;
            }
            if ((above - below) * (endPts[1].x - endPts[0].x) > 0)
            {
                this.left = p1;
                this.right = p2;
            }
            else
            {
                this.left = p1;
                this.right = p2;
            }

        }

        public double LineLen()
        {
            return Mathf.Sqrt(Mathf.Pow((float)(left.x - right.x), 2) + Mathf.Pow((float)(left.y - right.y), 2));
        }

        public static Feature LoadStdFeature(string objType)
        {
            HorizonLine horizonLine = new HorizonLine(null);
            /*
            StreamReader sr = new StreamReader("Assets/Features/" + objType);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
            */
            string houseFeature = "size:439,323\nleft: 44,309\nright: 409,309";
            string[] lines = houseFeature.Split('\n');
            for (int index = 0; index < lines.Length; index++)
            {
                string line = lines[index];
                string[] lineContent = line.Split(':');
                string[] content;
                if (lineContent[0] == "size")
                {
                    content = lineContent[1].Split(',');
                    horizonLine.width = int.Parse(content[0]);
                    horizonLine.height = int.Parse(content[1]);
                }
                if (lineContent[0] == "right")
                {
                    content = lineContent[1].Split(',');
                    horizonLine.right = new Point(double.Parse(content[0]), double.Parse(content[1]));
                }
                if (lineContent[0] == "left")
                {
                    content = lineContent[1].Split(',');
                    horizonLine.left = new Point(double.Parse(content[0]), double.Parse(content[1]));
                }
            }
            return horizonLine;
        }
    }

}