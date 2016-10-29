using UnityEngine;
using OpenCVForUnity;
using System.Collections.Generic;

namespace MagicCircuit
{
    public class ColorThreshold
    {
        private int h_min = 0, h_max = 180;
        private int s_min = 0, s_max = 255;
        private int v_min = 0, v_max = 100;

        private int area = 2000;


        public void getLines(Mat frameImg, ref List<Mat> roiList, ref List<OpenCVForUnity.Rect> rectList)
        {
            Mat hsvImg = new Mat();
			Mat binaryImg = new Mat();
            Mat lineImg = new Mat();

            if (roiList.Count  != 0) roiList.Clear();
            if (rectList.Count != 0) rectList.Clear();

            // Color Thresholding
            Imgproc.cvtColor(frameImg, hsvImg, Imgproc.COLOR_RGB2HSV);
            Core.inRange(hsvImg, new Scalar(h_min, s_min, v_min), new Scalar(h_max, s_max, v_max), binaryImg);
            Imgproc.morphologyEx(binaryImg, binaryImg, Imgproc.MORPH_OPEN, Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(3, 3)));
            Imgproc.morphologyEx(binaryImg, binaryImg, Imgproc.MORPH_CLOSE, Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(8, 8)));
            lineImg = binaryImg.clone();

            // Find Contours
            List<MatOfPoint> contours = new List<MatOfPoint>();
            Mat hierarchy = new Mat();
            Imgproc.findContours(binaryImg, contours, hierarchy, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE, new Point(0, 0));

            // Extract components using contour area
            for (int i = 0; i < contours.Count; i++)
            {
                if (Imgproc.contourArea(contours[i]) > area)
                {
                    OpenCVForUnity.Rect re = Imgproc.boundingRect(contours[i]);

                    // Extract only the correspoding component from frame using roi
                    // The size of roi is a variable
                    Mat roi = new Mat(lineImg, re);
                    roiList.Add(roi);
                    rectList.Add(re);
                }
            }
            return;
        }    
    }
}