using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using OpenCVForUnity;
using System.IO;

namespace OpenCVForUnitySample
{

    public class Axis : Feature
    {
        public Point top;
        public Point bottom;

        public Axis(MyObject obj)
        {
            m_Obj = obj;
        }

        public override void Calculate()
        {
            MatOfInt hull = new MatOfInt();
            Imgproc.convexHull(new MatOfPoint(m_Obj.imgPts.ToArray()), hull, false);

            float cur_d = 0, d = 0;
            Point p1 = new Point(), p2 = new Point();
            for (int i = 0; i < hull.rows(); i++)
            {
                for (int j = 0; j < hull.rows(); j++)
                {
                    cur_d = Mathf.Pow((float)(m_Obj.imgPts[(int)hull.get(i, 0)[0]].x - m_Obj.imgPts[(int)hull.get(j, 0)[0]].x), 2)
                        + Mathf.Pow((float)(m_Obj.imgPts[(int)hull.get(i, 0)[0]].y - m_Obj.imgPts[(int)hull.get(j, 0)[0]].y), 2);
                    if (cur_d > d)
                    {
                        p1 = m_Obj.imgPts[(int)hull.get(i, 0)[0]];
                        p2 = m_Obj.imgPts[(int)hull.get(j, 0)[0]];
                        d = cur_d;
                    }
                }
            }

            float d1 = 0, d2 = 0;
            int nearP1 = 0, nearP2 = 0;
            for (int i = 0; i < m_Obj.imgPts.Count; i++)
            {
                d1 = Mathf.Pow((float)(m_Obj.imgPts[i].x - p1.x), 2) + Mathf.Pow((float)(m_Obj.imgPts[i].y - p1.y), 2);
                d2 = Mathf.Pow((float)(m_Obj.imgPts[i].x - p2.x), 2) + Mathf.Pow((float)(m_Obj.imgPts[i].y - p2.y), 2);
                if (d1 < d2)
                {
                    nearP1++;
                }
                else
                {
                    nearP2++;
                }
            }
            if (nearP1 < nearP2)
            {
                this.top = p1;
                this.bottom = p2;
            }
            else
            {
                this.top = p2;
                this.bottom = p1;
            }
        }

        public double AxisLen()
        {
            return Mathf.Sqrt(Mathf.Pow((float)(top.x - bottom.x), 2) + Mathf.Pow((float)(top.y - bottom.y), 2));
        }

        public static Feature LoadStdFeature(string objType)
        {
            Axis axis = new Axis(null);
            /*
            StreamReader sr = new StreamReader("Assets/Features/" + objType);
            string line;
            while ((line = sr.ReadLine()) != null)
            */
            string treeFeature = "size:210,239\ntop: 96,13\nbottom: 122,227";
            string[] lines = treeFeature.Split('\n');
            for (int index = 0; index < lines.Length; index++)
            {
                string line = lines[index];
                string[] lineContent = line.Split(':');
                string[] content;
                if (lineContent[0] == "size")
                {
                    content = lineContent[1].Split(',');
                    axis.width = int.Parse(content[0]);
                    axis.height = int.Parse(content[1]);
                }
                if (lineContent[0] == "top")
                {
                    content = lineContent[1].Split(',');
                    axis.top = new Point(double.Parse(content[0]), double.Parse(content[1]));
                }
                if (lineContent[0] == "bottom")
                {
                    content = lineContent[1].Split(',');
                    axis.bottom = new Point(double.Parse(content[0]), double.Parse(content[1]));
                }
            }
            return axis;
        }
    }
}
