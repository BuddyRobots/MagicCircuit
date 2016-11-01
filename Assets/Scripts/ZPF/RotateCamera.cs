using OpenCVForUnity;
using System.Collections.Generic;

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

			// Old point matching for small board
            /*ptsBoard.Add(new Point(60, 123));
            ptsBoard.Add(new Point(398, 125));
            ptsBoard.Add(new Point(119, 411));
            ptsBoard.Add(new Point(337, 411));

            ptsWindow.Add(new Point(7, 28));
            ptsWindow.Add(new Point(478, 28));
            ptsWindow.Add(new Point(7, 570));
            ptsWindow.Add(new Point(478, 570));*/

            ptsBoard.Add(new Point(133, 433));
			ptsBoard.Add(new Point(133, 24));
			ptsBoard.Add(new Point(418, 359));
			ptsBoard.Add(new Point(418, 99));

			ptsWindow.Add(new Point(8, 24));
			ptsWindow.Add(new Point(631, 24));
			ptsWindow.Add(new Point(8, 431));
			ptsWindow.Add(new Point(631, 431));

            Mat rectBrd = Converters.vector_Point2f_to_Mat(ptsBoard);
            Mat rectWin = Converters.vector_Point2f_to_Mat(ptsWindow);

            homo = Imgproc.getPerspectiveTransform(rectBrd, rectWin);
        }

        public void rotate(ref Mat img)
        {
			Mat tmp = transform(img);

            //Core.transpose(tmp, tmp);

            img = new Mat(tmp, new Rect(ptsWindow[0], ptsWindow[3]));
        }

        public Mat transform(Mat img)
        {
            Mat rst = new Mat();

            Imgproc.warpPerspective(img, rst, homo, img.size());

            return rst;
        }
    }
}