using OpenCVForUnity;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MagicCircuit
{
    public class RotateCamera
    {

        List<Point> ptsBoard;
        List<Point> ptsWindow;

        Mat homo;

        public RotateCamera()
        {
            ptsBoard = new List<Point>();
            ptsWindow = new List<Point>();
          
			ptsBoard.Add(new Point(133, 24));
			ptsBoard.Add(new Point(133, 433));
			ptsBoard.Add(new Point(418, 99));
			ptsBoard.Add(new Point(418, 359));

			ptsWindow.Add(new Point(0, 0));
			ptsWindow.Add(new Point(602, 0));
			ptsWindow.Add(new Point(0, 697));
			ptsWindow.Add(new Point(602, 697));

            Mat rectBrd = Converters.vector_Point2f_to_Mat(ptsBoard);
            Mat rectWin = Converters.vector_Point2f_to_Mat(ptsWindow);

            homo = Imgproc.getPerspectiveTransform(rectBrd, rectWin);
        }

        public void rotate(ref Mat img)
        {
			Mat tmp = transform(img);

            //Core.transpose(tmp, tmp);

            //img = new Mat(tmp, new Rect(ptsWindow[0], ptsWindow[3]));

			img = tmp.clone();
        }

        public Mat transform(Mat img)
        {
			Mat rst = new Mat(698, 603, img.type());

			Imgproc.warpPerspective(img, rst, homo, rst.size());

            return rst;
        }
    }
}