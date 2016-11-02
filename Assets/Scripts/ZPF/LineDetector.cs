﻿using UnityEngine;
using OpenCVForUnity;
using System.Collections.Generic;

namespace MagicCircuit
{
    public class LineDetector
    {
		private const int STEP_SMALL    = 10;
		private const int STEP_MEDIUM   = 15;
		private const int STEP_LARGE    = 20;
		private const int MIN_POINT_NUM = 3;

        public void detectLine(Mat frameImg, ref List<List<List<Point>>> lineGroupList, ref List<OpenCVForUnity.Rect> boundingRectList)
        {
			Debug.Log("LineDetector : detectLine Start");


			List<Mat> roiList = new List<Mat>();

            getLines(frameImg, ref roiList, ref boundingRectList);



			Debug.Log("LineDetector : roiList.Count = " + roiList.Count);




            for (var i = 0; i < roiList.Count; i++)
            {
                lineGroupList.Add(vectorize(roiList[i]));
            }


			Debug.Log("flag 3");
        }

		private void getLines(Mat frameImg, ref List<Mat> roiList, ref List<OpenCVForUnity.Rect> rectList)
		{
			int h_min = 0, h_max = 180;
			int s_min = 0, s_max = 255;
			int v_min = 0, v_max = 100;

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
				Debug.Log("LineDetector getLine Imgproc.contourArea(contours[i]) = " + Imgproc.contourArea(contours[i]));




				if (Imgproc.contourArea(contours[i]) > Constant.LINE_REGION_MIN_AREA)
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

        private List<List<Point>> vectorize(Mat lineImg)
        {
            List<Point> myLine = new List<Point>();
            List<Point> line = new List<Point>();
            List<List<Point>> listLine = new List<List<Point>>();
            Queue<Point> allQueue = new Queue<Point>();
            Queue<Point> itrQueue = new Queue<Point>();
            Queue<Point> intQueue = new Queue<Point>();

            // Skeletonize the input first.
            Mat skel = new Mat(lineImg.size(), CvType.CV_8UC1);
            skel = skeletonization(lineImg);
            
            // Pick an init point on the line
            Point firstPoint = findFirstPoint(skel);
            if (firstPoint.x != 0 && firstPoint.y != 0)
            {
                myLine.Add(firstPoint);
                line.Add(firstPoint);
            }
            if (line.Count == 0) return listLine; // If we don't have any point

            bool one_time_flag = true;
            bool merge_first_line = false;
            //@
            while (true)
            {
                Point interPoint = new Point();
                // @allQueue : all intersect points
                // @itrQueue : intersect points in one iteration
                // @intQueue : intersect point to be added to line
                findLine(skel, firstPoint, ref line, ref itrQueue);  
                
                // Determine whether to merge the first two lines or not
                if(one_time_flag)
                {
                    if (itrQueue.Count == 2)
                        merge_first_line = true;
                    one_time_flag = false;
                }

                // Add intersect point to the end of this line
                if (itrQueue.Count > 0)
                {
                    Queue<Point> tmpQueue = new Queue<Point>(itrQueue);
                    interPoint = findIntersectPoint(tmpQueue);
                    line.Add(interPoint);
                }

                // If we have more than @point_num points on one line, add this line
                if (line.Count > MIN_POINT_NUM)
                    listLine.Add(line);

                // Enqueue the intersect points
                while (itrQueue.Count > 0)
                {
                    allQueue.Enqueue(itrQueue.Dequeue());
                    intQueue.Enqueue(interPoint);
                }

                // Break when all intersect points are processed
                if (allQueue.Count == 0)
                    break; 
                 
                // Preperation for next iteration
                firstPoint = allQueue.Dequeue();
                line = new List<Point>();
                line.Add(intQueue.Dequeue());
                line.Add(firstPoint);
            }

            // Merge the first two lines
            if (merge_first_line)
                mergeFirstLine(ref listLine);

            // Merge mis-detected lines
            mergeLine(ref listLine);

            return listLine;
        }

        private Mat skeletonization(Mat grayImg)
        {
            Mat skel = new Mat(grayImg.size(), CvType.CV_8UC1, new Scalar(0, 0, 0));
            Mat temp = new Mat();
            Mat eroded = new Mat();

            Mat element = Imgproc.getStructuringElement(Imgproc.MORPH_CROSS, new Size(3, 3));

            for (var i = 0; i < 200; i++)
            {
                Imgproc.erode(grayImg, eroded, element);
                Imgproc.dilate(eroded, temp, element); // temp = open(grayImg)
                Core.subtract(grayImg, temp, temp);
                Core.bitwise_or(skel, temp, skel);
                grayImg = eroded.clone();

                if (Core.countNonZero(grayImg) == 0)   // done.
                    break;
            }

            //Imgproc.GaussianBlur(skel, skel, new Size(5, 5), 0);
            //pointFilter(skel, 2, 5);
            element = Imgproc.getStructuringElement(Imgproc.MORPH_CROSS, new Size(2, 2));
            Imgproc.dilate(skel, skel, element);

            return skel;
        }

        private Point findFirstPoint(Mat skel)
        {
            for (var i = 1; i < skel.rows(); i++)
                for (var j = 1; j < skel.cols(); j++)
                    if (skel.get(i, j)[0] > 125)
                    {                        
                        return new Point(j, i);
                    }
            return new Point(0, 0);
        }

        private Queue<Point> findNextPoints(Mat skel, Point current, int delta)
        {
            Queue<Point> temp = new Queue<Point>();
            Point temPoint_1 = new Point();
            Point temPoint_2 = new Point();
            Queue<Point> result = new Queue<Point>();

            // Point Range[0, rows() - 1][0, cols() - 1]          
            int _xl = Mathf.Max((int)current.x - delta, 0);
            int _xr = Mathf.Min((int)current.x + delta, skel.cols() - 1);
            int _yu = Mathf.Max((int)current.y - delta, 0);
            int _yd = Mathf.Min((int)current.y + delta, skel.rows() - 1);

            // left
            for (var y = _yu + 1; y < _yd; y++)
                if (skel.get(y, _xl)[0] > 125)
                    temp.Enqueue(new Point(_xl, y));
            if (temp.Count > 0)
            {
                temPoint_1 = temp.Dequeue();
                result.Enqueue(temPoint_1);
                while(temp.Count > 0)
                {
                    temPoint_2 = temp.Dequeue();
                    if (temPoint_1.y - temPoint_2.y > 1) // if the new point is not connected to the old point
                        result.Enqueue(temPoint_2);
                    temPoint_1 = temPoint_2;
                }
            }
            temp.Clear();
            // right
            for (var y = _yu + 1; y < _yd; y++)
                if (skel.get(y, _xr)[0] > 125)
                    temp.Enqueue(new Point(_xr, y));
            if (temp.Count > 0)
            {
                temPoint_1 = temp.Dequeue();
                result.Enqueue(temPoint_1);
                while (temp.Count > 0)
                {
                    temPoint_2 = temp.Dequeue();
                    if (temPoint_1.y - temPoint_2.y > 1) // if the new point is not connected to the old point
                        result.Enqueue(temPoint_2);
                    temPoint_1 = temPoint_2;
                }
            }
            temp.Clear();
            // up
            for (var x = _xl; x <= _xr; x++)
                if (skel.get(_yu, x)[0] > 125)
                    temp.Enqueue(new Point(x, _yu));
            if (temp.Count > 0)
            {
                temPoint_1 = temp.Dequeue();
                result.Enqueue(temPoint_1);
                while (temp.Count > 0)
                {
                    temPoint_2 = temp.Dequeue();
                    if (temPoint_1.x - temPoint_2.x > 1) // if the new point is not connected to the old point
                        result.Enqueue(temPoint_2);
                    temPoint_1 = temPoint_2;
                }
            }
            temp.Clear();
            // down
            for (var x = _xl; x <= _xr; x++)
                if (skel.get(_yd, x)[0] > 125)
                    temp.Enqueue(new Point(x, _yd));
            if (temp.Count > 0)
            {
                temPoint_1 = temp.Dequeue();
                result.Enqueue(temPoint_1);
                while (temp.Count > 0)
                {
                    temPoint_2 = temp.Dequeue();
                    if (temPoint_1.x - temPoint_2.x > 1) // if the new point is not connected to the old point
                        result.Enqueue(temPoint_2);
                    temPoint_1 = temPoint_2;
                }
            }
            temp.Clear();

            // Delete detected region
            removeBox(skel, _xl, _xr, _yu, _yd);
            return result;
        }

        private Point findIntersectPoint(Queue<Point> pointQueue)
        {
            if (pointQueue.Count == 0)
                return new Point(0, 0);

            double x = 0;
            double y = 0;
            int count = pointQueue.Count;

            while (pointQueue.Count > 0)
            {
                Point p = pointQueue.Dequeue();
                x += p.x;
                y += p.y;
            }

            return new Point(x / count, y / count);
        }

        private void findLine(Mat skel, Point current, ref List<Point> line, ref Queue<Point> pointQueue)
        {
            while(true)
            {
                Queue<Point> myPoint = findNextPoints(skel, current, STEP_SMALL);

                if (myPoint.Count != 1)
                {
                    // increase radius
                    myPoint = findNextPoints(skel, current, STEP_MEDIUM);

                    if (myPoint.Count != 1)
                    {
                        // increase radius
                        myPoint = findNextPoints(skel, current, STEP_LARGE);

                        if (myPoint.Count != 1)
                        {
                            // intersect point or deadend
                            pointQueue = myPoint; // Save pointQueue
                            return; 
                        }
                    }                    
                }

                // current = next
                current = myPoint.Dequeue();

                line.Add(current);
            }
        }

        private void removeBox(Mat img, int xl, int xr, int yu, int yd)
        {
            for(var i = yu; i <= yd; i++)
                for(var j = xl; j <= xr; j++)
                {
                    img.put(i, j, 0);
                }
        }

        private void mergeFirstLine(ref List<List<Point>> listLine)
        {
            if (listLine.Count >= 2)
            {
                listLine[0].Reverse();
                listLine[0].AddRange(listLine[1]);

                listLine.RemoveAt(1);
            }
        }

        /// <summary>
        /// If a point in @end has and only has one match in @start, 
        /// merge these two lines.
        /// </summary>
        /// <param name="listLine"></param>
        /// 
        private void mergeLine(ref List<List<Point>> listLine)
        {
            List<Point> start = new List<Point>();
            List<Point> end = new List<Point>();

            // Construct @start and @end
            for (var i = 0; i < listLine.Count; i++)
            {
                start.Add(listLine[i][0]);
                end.Add(listLine[i][listLine[i].Count - 1]);
            }

            // Compare
            for (var i = 0; i < end.Count; i++)
            {
                int count = 0;
                int end_idx = i;
                int start_idx = 0;

                for (var j = 0; j < start.Count && count < 2; j++)
                {
                    if (start[j].x == end[i].x && start[j].y == end[i].y)
                    {
                        count++;
                        start_idx = j;
                    }
                }

                if (count == 1)
                {
                    // merge start_idx and end_idx
                    listLine[end_idx].AddRange(listLine[start_idx]);

                    listLine.RemoveAt(start_idx);
                    start.RemoveAt(start_idx);
                    end.RemoveAt(end_idx);
                }
            }
        }
    }
}