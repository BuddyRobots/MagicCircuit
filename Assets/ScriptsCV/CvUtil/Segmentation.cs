using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using OpenCVForUnity;
using System.Diagnostics;
using System;

namespace OpenCVForUnitySample
{
    public class Segmentation
    {
        public Segmentation(Mat image)
        {
            m_Img = image.clone();
            m_ImgPts = new List<Point>();
        }

        public List<List<Point>> Do()
        {
            FigProc.Img2Pts(m_Img.clone(), ref m_ImgPts);
            List<List<Point>> clusterList = new List<List<Point>>();
            List<MatOfPoint> contours = new List<MatOfPoint>();
            Mat hierarchy = new Mat();

            // blur and convert to binary image
            Imgproc.GaussianBlur(m_Img, m_Img, new Size(7, 7), 7, 7);
            Imgproc.threshold(m_Img, m_Img, 253, 255, Imgproc.THRESH_BINARY);

            // find out the contours
            Imgproc.findContours(m_Img, contours, hierarchy, Imgproc.RETR_TREE, Imgproc.CHAIN_APPROX_SIMPLE, new Point(0, 0));

            // only filter out the outer contours
            List<MatOfPoint> outContours = new List<MatOfPoint>();
            for (int i = 0; i < contours.Count; i++)
            {
                if (hierarchy.get(0, i)[3] == 0)
                {
                    outContours.Add(contours[i]);
                    clusterList.Add(new List<Point>());
                }
            }

            if (outContours.Count == 0)
            {
                return clusterList;
            }

            List<MatOfPoint2f> tmpContours = new List<MatOfPoint2f>(outContours.Count);
            for (int i = 0; i < outContours.Count; i++)
            {
                MatOfPoint2f t = new MatOfPoint2f();
                outContours[i].convertTo(t, CvType.CV_32FC2);
                tmpContours.Add(t);
            }
            int lastContourIndex = 0;
            double contain;
            MatOfPoint2f tt = new MatOfPoint2f();
            for (int i = 0; i < m_ImgPts.Count; i++)
            {
                tt = tmpContours[lastContourIndex];
                contain = Imgproc.pointPolygonTest(tt, m_ImgPts[i], false);
                if (contain >= 0)
                {
                    clusterList[lastContourIndex].Add(m_ImgPts[i]);
                    continue;
                }
                for (int j = 0; j < outContours.Count; j++)
                {
                    if (j == lastContourIndex)
                    {
                        continue;
                    }
                    contain = Imgproc.pointPolygonTest(tmpContours[j], m_ImgPts[i], false);
                    if (contain >= 0)
                    {
                        clusterList[j].Add(m_ImgPts[i]);
                        lastContourIndex = j;
                        break;
                    }
                }
            }

            /*
            Profiler.BeginSample("Part 4");
            MatOfPoint2f tmp = new MatOfPoint2f();
            for (int i = 0; i < m_ImgPts.Count; i++)
            {
                for (int j = 0; j < outContours.Count; j++)
                {
                    outContours[j].convertTo(tmp, CvType.CV_32FC2);
                    double contain = Imgproc.pointPolygonTest(tmp, m_ImgPts[i], false);
                    if (contain >= 0)
                    {
                        clusterList[j].Add(m_ImgPts[i]);
                    }
                }
            }
            Profiler.EndSample();
            */

            return clusterList;
        }



        private Mat m_Img;
        private int width;
        private int height;
        private List<Point> m_ImgPts;
    }
}
