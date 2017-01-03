using UnityEngine;
using System.Collections.Generic;
using OpenCVForUnity;
using MagicCircuit;
using System;

public class CardDetector_old
{	
    public static List<List<Point>> findSquares(Mat _binaryImg)
    {
		Mat binaryImg = _binaryImg.clone();
        List<List<Point>> squares = new List<List<Point>>();
        List<MatOfPoint> contours = new List<MatOfPoint>();

		Imgproc.findContours(binaryImg, contours, new Mat(), Imgproc.RETR_LIST, Imgproc.CHAIN_APPROX_SIMPLE, new Point(0, 0));

        List<MatOfPoint2f> contours2f = new List<MatOfPoint2f>(contours.Count);

        for (int i = 0; i < contours.Count; i++)
        {
            MatOfPoint2f t = new MatOfPoint2f();
            contours[i].convertTo(t, CvType.CV_32FC2);
            contours2f.Add(t);
        }

        MatOfPoint2f approx2f = new MatOfPoint2f();
        for (int i = 0; i < contours.Count; i++)
        {
            Imgproc.approxPolyDP(contours2f[i], approx2f, Imgproc.arcLength(contours2f[i], true)*0.05, true);

            MatOfPoint approx = new MatOfPoint();
            approx2f.convertTo(approx, CvType.CV_32S);

            if (approx.size().height == 4 && Imgproc.contourArea(approx) > 1000 && Imgproc.isContourConvex(approx))
            {
                double maxCosine = 0;
                for (int j = 2; j < 5; j++)
                {
                    double cosine = Mathf.Abs(calcAngle(approx.toArray()[j%4], approx.toArray()[j - 2], approx.toArray()[j - 1]));
                    maxCosine = maxCosine > cosine ? maxCosine : cosine;
                }
                if (maxCosine < 0.8)
                    squares.Add(approx.toList());
            }
        }
        return filterSquares(squares);
    }


	private static float calcAngle(Point pt1, Point pt2, Point pt0)
	{
		double dx1 = pt1.x - pt0.x;
		double dy1 = pt1.y - pt0.y;
		double dx2 = pt2.x - pt0.x;
		double dy2 = pt2.y - pt0.y;
		return (float)(dx1 * dx2 + dy1 * dy2) / Mathf.Sqrt((float)((dx1 * dx1 + dy1 * dy1) * (dx2 * dx2 + dy2 * dy2) + 1e-10));
	}


    public static List<List<Point>> computeOuterSquare(List<List<Point>> squareList)
    {
		List<List<Point>> outerSquareList = new List<List<Point>>();

        for (var i = 0; i < squareList.Count; i++)
        {
			List<Point> tmpSquare = new List<Point>();
			Point squareCenter = new Point((squareList[i][0].x + squareList[i][2].x)/2, (squareList[i][0].y + squareList[i][2].y)/2);

            for (var j = 0; j < 4; j++)
            {
				double x = Constant.CARD_OUTER_SQUARE_RATIO * (squareList[i][j].x - squareCenter.x) + squareCenter.x;
				double y = Constant.CARD_OUTER_SQUARE_RATIO * (squareList[i][j].y - squareCenter.y) + squareCenter.y;
                tmpSquare.Add(new Point(x, y));
            }
            outerSquareList.Add(tmpSquare);
        }
        return outerSquareList;
    }


    private static List<List<Point>> filterSquares(List<List<Point>> squareList)
    {
		///
		Debug.Log("CardDetector.cs filterSquares() : num of squares = " + squareList.Count);
		///



        List<List<Point>> filteredSquares = new List<List<Point>>();

        for (int j = 0; j < squareList.Count; j++)
        {
            double len, curMaxLen = 0, curMinLen = 10000;

            for (int i = 0; i < 3; i++)
            {
                len = calcLength(squareList[j][i % 4], squareList[j][(i + 1) % 4]);
                curMaxLen = len > curMaxLen ? len : curMaxLen;
                curMinLen = len < curMinLen ? len : curMinLen;
            }

			if (curMaxLen > Constant.CARD_MAX_SQUARE_LEN || curMinLen < Constant.CARD_MIN_SQUARE_LEN || curMaxLen / curMinLen > Constant.CARD_MAX_SQUARE_LEN_RATIO)
                continue;



			///
			Debug.Log("CardDetector.cs filterSquares() : curMaxLen = " + curMaxLen + " curMinLen = " + curMinLen + " ratio = " + curMaxLen / curMinLen);
			for (var k = 0; k < squareList[j].Count; k++)
				Debug.Log("CardDetector.cs filterSquares() : squares["+j+"]["+k+"] = " + squareList[j][k]);
			///



			if (!isSquareClockwise(squareList[j]))
				filteredSquares.Add(squareList[j]);
        }

		//deleteOverlaySquares(filteredSquares);



		///
		Debug.Log("CardDetector.cs filterSquares() : after deleteOverlaySquares : num of squares = " + filteredSquares.Count);
		///

			

        return filteredSquares;
    }


    private static double calcLength(Point p1, Point p2, bool sqrt_v = true)
    {
        float v = Mathf.Pow((float)(p1.x - p2.x), 2) + Mathf.Pow((float)(p1.y - p2.y), 2);
        return sqrt_v ? Mathf.Sqrt(v) : v;
    }


	private static void deleteOverlaySquares(List<List<Point>> squareList)
	{
		List<Point> centerList = new List<Point>();
		for (var i = 0; i < squareList.Count; i++)
			centerList.Add(calcCenterPoint(squareList[i]));
		
		for (var i = 0; i < squareList.Count - 1; i++)
			for (var j = i + 1; j < squareList.Count; j++)
				if (isOverlayed(centerList[i], centerList[j]))
				{
					centerList.RemoveAt(j);
					squareList.RemoveAt(j);
				}
	}


	private static Point calcCenterPoint(List<Point> square)
	{
		Point center = new Point((square[0].x + square[2].x)/2, (square[0].y + square[2].y)/2);
		return center;
	}


	private static bool isOverlayed(Point center_1, Point center_2)
	{
		if (calcDistance(center_1, center_2) < Constant.CARD_MAX_SQUARE_LEN)
			return true;
		else
			return false;
	}


	private static double calcDistance(Point center_1, Point center_2)
	{
		return Math.Sqrt(Math.Pow((center_1.x - center_2.x), 2) + Math.Pow((center_1.y - center_2.y), 2));
	}				


	// Deprecated
	private static bool isSquareClockwise(List<Point> square)
    {
        bool clockwise;
        int direction;

        if (Mathf.Abs((float)(square[0].x - square[1].x)) > Mathf.Abs((float)(square[0].y - square[1].y)))
        {
            direction = square[1].x > square[0].x ? 0 : 1;
        }
        else
        {
            direction = square[1].y > square[0].y ? 2 : 3;
        }

        if (direction == 0 || direction == 1)
        {
            int second_direction = square[2].y > square[1].y ? 2 : 3;
            clockwise = (direction == 0 && second_direction == 2) || (direction == 1 && second_direction == 3);
        }
        else
        {
            int second_direction = square[2].x > square[1].x ? 0 : 1;
            clockwise = (direction == 2 && second_direction == 1) || (direction == 3 && second_direction == 0);
        }
        return clockwise;
    }
}