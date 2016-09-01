using OpenCVForUnity;
using System.Collections.Generic;

using UnityEngine;

namespace MagicCircuit
{
    public class myUtils
    {
        public void drawBoundingBox(Mat image, List<Point> bb)
        {
            for (var i = 0; i < bb.Count - 1; i++)
            {
                Imgproc.line(image, bb[i], bb[i + 1], new Scalar(0, 0, 255), 10);
            }
            Imgproc.line(image, bb[bb.Count - 1], bb[0], new Scalar(0, 0, 255), 10);

            Point center = new Point((bb[0].x + bb[2].x) / 2, (bb[0].y + bb[2].y) / 2);
            Point right = new Point((bb[3].x + bb[2].x) / 2, (bb[3].y + bb[2].y) / 2);

            Imgproc.line(image, center, right, new Scalar(0, 255, 0), 10);
        }

        // @Override
        public void drawBoundingBox(Mat image, List<Point> bb, Scalar color)
        {
            for (var i = 0; i < bb.Count - 1; i++)
            {
                Imgproc.line(image, bb[i], bb[i + 1], color, 10);
            }
            Imgproc.line(image, bb[bb.Count - 1], bb[0], color, 10);

            Point center = new Point((bb[0].x + bb[2].x) / 2, (bb[0].y + bb[2].y) / 2);
            Point right = new Point((bb[3].x + bb[2].x) / 2, (bb[3].y + bb[2].y) / 2);

            Imgproc.line(image, center, right, color, 10);
        }

        // @Override
        public void drawBoundingBox(Mat image, List<Point> homo, OpenCVForUnity.Rect rect, Scalar color)
        {
            /*float radius = (float)rect.width / 2;
            float theta = Mathf.Atan2((float)(homo[2].y + homo[3].y), (float)(homo[2].x + homo[3].x));
            float _x = radius * Mathf.Cos(theta);
            float _y = radius * Mathf.Sin(theta);*/
            Point center = new Point((rect.tl().x + rect.br().x) / 2, (rect.tl().y + rect.br().y) / 2);

            double _x = (homo[3].x - homo[0].x) / 2;
            double _y = (homo[3].y - homo[0].y) / 2;

            Point right = new Point((center.x + _x), (center.y + _y));

            Imgproc.rectangle(image, rect.tl(), rect.br(), color, 10);
            Imgproc.line(image, center, right, color, 10);
        }

        public void drawPoint(Mat image, List<List<Point>> listLine, OpenCVForUnity.Rect rect)
        {
            System.Random rnd = new System.Random();

            Point center = new Point(rect.tl().x, rect.tl().y );
            
            for (var j = 0; j < listLine.Count; j++)
            {
                Scalar color = new Scalar(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                for (var i = 0; i < listLine[j].Count; i++)
                    Imgproc.circle(image, new Point(listLine[j][i].x + center.x, listLine[j][i].y + center.y), 5, color);
            }
            
        }

        public MatOfPoint2f kp2Point(List<KeyPoint> keypoints)
        {
            double[] points = new double[keypoints.Count * 2];
            for(var i = 0; i < keypoints.Count; i++)
            {
                points[2 * i] = keypoints[i].pt.x;
                points[2 * i + 1] = keypoints[i].pt.y;
            }

            Mat tmp = new Mat(keypoints.Count, 1, CvType.CV_32FC2);
            tmp.put(0, 0, points);

            return new MatOfPoint2f(tmp);
        }

        public List<Point> perTrans(List<Point> src, Mat h)
        {
            List<Point> res = new List<Point>();
            double[,] H = new double[3, 3];

            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                {
                    H[i, j] = h.get(i, j)[0];
                }

            for (var i = 0; i < src.Count; i++)
            {
                double x = (H[0, 0] * src[i].x + H[0, 1] * src[i].y + H[0, 2]) / (H[2, 0] * src[i].x + H[2, 1] * src[i].y + H[2, 2]);
                double y = (H[1, 0] * src[i].x + H[1, 1] * src[i].y + H[1, 2]) / (H[2, 0] * src[i].x + H[2, 1] * src[i].y + H[2, 2]);
                res.Add(new Point(x, y));
            }
            return res;
        }
    }

    public enum ItemType
    {
        Battery,
        Switch,
        Bulb,
		Loudspeaker,

		/// <summary>
		/// single pole double throw switch  单刀双掷开关
		/// </summary>
		DoubleDirSwitch,

		/// <summary>
		/// voice operated switch  声控开关
		/// </summary>
		VoiceOperSwitch,

		/// <summary>
		/// light activated switch  光敏开关
		/// </summary>
		LightActSwitch,	

		/// <summary>
		/// time-delay switch      声控延时开关
		/// </summary>
		VoiceTimedelaySwitch,	

		/// <summary>
		/// 电磁炉
		/// </summary>
		InductionCooker,

		/// <summary>
		/// line 线
		/// </summary>
        CircuitLine                             //如果是线的话，则是点的集合
    }
    
    public class CircuitItem                    //图标管理类 id,名字，类型，坐标
    {
        public int ID { get; set; }
        public string name { get; set; }
        public ItemType type { get; set; }      //图标类型
        public List<Vector3> list { get; set; } //图标的坐标
		public double theta{ get; set; }        //图标的朝向（单位：角度）
		public int showOrder{ get; set; }       //显示顺序 从0开始（图标的显示顺序是灯泡）
		public bool powered{ get; set; }        //元件是否通电
		//private int appearTimes{get; set;}		//图标出现的次数

        public Vector2 connect_left;            //Connect point on card
        public Vector2 connect_right;

        private double x_shift;                 //Parameters for changing cordinates
        private double y_shift;

		public CircuitItem(){
		}

        public CircuitItem(int _id, string _name, ItemType _type, List<Point> _list, double _theta, int _order, Size _frameSize, bool _p = false)
        {
            ID = _id;
            name = _name;
            type = _type;
            list = points2vector3(_list);
            theta = _theta;
            showOrder = _order;
            powered = _p;

            connect_left = new Vector2();
            connect_right = new Vector2();

			x_shift = _frameSize.width / 2;
            y_shift = _frameSize.height / 2;
        }

        // @Override
        public CircuitItem(int _id, string _name, ItemType _type, int _order, Size _frameSize, bool _p = false)
        {
            ID = _id;
            name = _name;
            type = _type;
            showOrder = _order;
            powered = _p;
            list = new List<Vector3>();
            theta = 0;

            connect_left = new Vector2();
            connect_right = new Vector2();

            x_shift = _frameSize.width / 2;
            y_shift = _frameSize.height / 2;
        }

        private List<Vector3> points2vector3(List<Point> src)
        {
            List<Vector3> res = new List<Vector3>();
            for (var i = 0; i < src.Count; i++)
            {
                res.Add(cordinateMat2Tex(src[i].x, src[i].y));
            }
            return res;
        }

        public void extractCard(List<Point> bb, OpenCVForUnity.Rect rect)
        {
            Point center = new Point((rect.tl().x + rect.br().x) / 2, (rect.tl().y + rect.br().y) / 2);

            double _x = (bb[3].x - bb[0].x) / 2;
            double _y = (bb[3].y - bb[0].y) / 2;

            Point right = new Point((center.x + _x), (center.y + _y));

            theta = Mathf.Atan2((float)(right.y - center.y), (float)(right.x - center.x)); // thera in radians
            
            list.Add(cordinateMat2Tex(center.x, center.y));

            // Compute the cordinate of connect points
            // Make sure rect is a square in detect part to make this work right.
            _x = rect.width / (1 + Mathf.Tan((float)theta)) / 2;
            _y = _x * Mathf.Tan((float)theta);

            connect_left = new Vector2((float)(center.x - _x), (float)(center.y - _y));
            connect_right = new Vector2((float)(center.x + _x), (float)(center.y + _y));

            theta = theta * 180.0 / Mathf.PI; // theta in degrees
        }

        public void extractLine(List<Point> line, OpenCVForUnity.Rect rect)
        {
            Point center = new Point(rect.tl().x, rect.tl().y);

            for (var i = 0; i < line.Count; i++)
            {
                list.Add(cordinateMat2Tex((line[i].x + center.x), (line[i].y + center.y)));
            }
        }        

        private Vector3 cordinateMat2Tex(double x, double y)
        {            
            return new Vector3((float)(x - x_shift), (float)(y_shift - y));
        }
    }
}
 