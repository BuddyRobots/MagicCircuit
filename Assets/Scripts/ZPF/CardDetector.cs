using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity;
using MagicCircuit;


public static class CardDetector_new {
	public static List<List<Point>> findSquares(Mat binaryImg)
	{
		// TODO : need to test binaryImg after mergeComponent
		mergeConnectedComponents(ref binaryImg);

		List<List<Point>> squareList = filterSquares(binaryImg);

		return squareList;
	}


	private static void mergeConnectedComponents(ref Mat binaryImg)
	{
		Imgproc.morphologyEx(binaryImg, binaryImg, Imgproc.MORPH_ERODE,
			Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(Constant.CARD_ERODE_KERNEL, Constant.CARD_ERODE_KERNEL)));

		Mat labeledImg = new Mat();
		Mat stats = new Mat();

		int numOfLabels = Imgproc.connectedComponentsWithStats(binaryImg, labeledImg, stats, new Mat(), 8, CvType.CV_16U);

		List<int> componentToDeleteList = new List<int>();

		for (var i = 1; i < numOfLabels; i++)
		{
			double stat_height = stats.get(i, Imgproc.CC_STAT_HEIGHT)[0];
			double stat_width = stats.get(i, Imgproc.CC_STAT_WIDTH)[0];
			//Debug.Log("RecognizeAlgo.cs process() : width = " + stat_width + "height = " + stat_height);

			if (stat_width < Constant.CARD_MAX_SQUARE_LEN && stat_height < Constant.CARD_MAX_SQUARE_LEN)
				componentToDeleteList.Add(i);
		}

		binaryImg = deleteComponents(binaryImg, labeledImg, componentToDeleteList);

		Imgproc.morphologyEx(binaryImg, binaryImg, Imgproc.MORPH_CLOSE,
			Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(Constant.CARD_CLOSE_KERNEL, Constant.CARD_CLOSE_KERNEL)));
	}


	private static Mat deleteComponents(Mat binaryImg, Mat labeledImg, List<int> componentToDeleteList)
	{
		byte[] binaryArray = new byte[binaryImg.rows()*binaryImg.cols()];
		binaryImg.get(0, 0, binaryArray);
		short[] labeledArray = new short[labeledImg.rows()*labeledImg.cols()];
		labeledImg.get(0, 0, labeledArray);

		if (binaryArray.Length != labeledArray.Length)
		{
			Debug.Log("CardDetector.cs deleteComponent() : ERROR: Mismatching length!!!! " +
				"binaryArray.Length = " + binaryArray.Length + " labeledArray.Length = " + labeledArray.Length);
			return binaryImg;
		}

		for (var i = 0; i < labeledArray.Length; i++)
		{
			if (isInDeleteList(labeledArray[i], componentToDeleteList))
				binaryArray[i] = 0;
		}

		binaryImg.put(0, 0, binaryArray);

		return binaryImg;
	}


	private static bool isInDeleteList(int idx, List<int> deleteList)
	{
		for (var i = 0; i < deleteList.Count; i++)
			if (idx == deleteList[i])
				return true;
		return false;
	}


	private static List<List<Point>> filterSquares(Mat binaryImg)
	{
		List<List<Point>> squareList = findPolygon(binaryImg);

		List<List<Point>> filteredSquares = new List<List<Point>>();

		for (int j = 0; j < squareList.Count; j++)
		{
			double len, curMaxLen = 0, curMinLen = 10000;

			for (int i = 0; i < 3; i++)
			{
				len = calcLength(squareList[j][i%4], squareList[j][(i + 1)%4]);
				curMaxLen = len > curMaxLen ? len : curMaxLen;
				curMinLen = len < curMinLen ? len : curMinLen;
			}

			if (curMaxLen > Constant.CARD_MAX_SQUARE_LEN || curMinLen < Constant.CARD_MIN_SQUARE_LEN || curMaxLen/curMinLen > Constant.CARD_MAX_SQUARE_LEN_RATIO)
				continue;

			filteredSquares.Add(squareList[j]);
		}



		///
		Debug.Log("CardDetector_new.cs filterSquares() : filteredSquares.Count = " + filteredSquares.Count);
		///



		return filteredSquares;
	}


	private static List<List<Point>> findPolygon(Mat binaryImg)
	{		
		List<MatOfPoint> contours = new List<MatOfPoint>();
		Imgproc.findContours(binaryImg, contours, new Mat(), Imgproc.RETR_LIST, Imgproc.CHAIN_APPROX_SIMPLE, new Point(0, 0));

		List<MatOfPoint2f> contours2f = new List<MatOfPoint2f>(contours.Count);

		for (int i = 0; i < contours.Count; i++)
		{
			MatOfPoint2f t = new MatOfPoint2f();
			contours[i].convertTo(t, CvType.CV_32FC2);
			contours2f.Add(t);
		}

		List<List<Point>> squareList = new List<List<Point>>();
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
					double cosine = Mathf.Abs(calcAngle(approx.toArray()[j % 4], approx.toArray()[j - 2], approx.toArray()[j - 1]));
					maxCosine = maxCosine > cosine ? maxCosine : cosine;
				}
				if (maxCosine < 0.8)
					squareList.Add(approx.toList());
			}
		}
		return squareList;
	}


	private static float calcAngle(Point pt1, Point pt2, Point pt0)
	{
		double dx1 = pt1.x - pt0.x;
		double dy1 = pt1.y - pt0.y;
		double dx2 = pt2.x - pt0.x;
		double dy2 = pt2.y - pt0.y;
		return (float)(dx1 * dx2 + dy1 * dy2) / Mathf.Sqrt((float)((dx1 * dx1 + dy1 * dy1) * (dx2 * dx2 + dy2 * dy2) + 1e-10));
	}	


	private static double calcLength(Point p1, Point p2, bool sqrt_v = true)
	{
		float v = Mathf.Pow((float)(p1.x - p2.x), 2) + Mathf.Pow((float)(p1.y - p2.y), 2);
		return sqrt_v ? Mathf.Sqrt(v) : v;
	}
}