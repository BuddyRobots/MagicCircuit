using UnityEngine;
using System.Collections;
using OpenCVForUnity;
using System.Collections.Generic;

public class RotateCamera {

	List<Point> ptsBoard;
	List<Point> ptswindow;

	Mat homo;

	public RotateCamera()
	{
		ptsBoard = new List<Point>();
		ptswindow = new List<Point>();

		ptsBoard.Add(new Point(60, 123));
		ptsBoard.Add(new Point(398, 125));
		ptsBoard.Add(new Point(119, 411));
		ptsBoard.Add(new Point(337, 411));

		ptswindow.Add(new Point(7, 28));
		ptswindow.Add(new Point(478, 28));
		ptswindow.Add(new Point(7, 570));
		ptswindow.Add(new Point(478, 570));

		Mat rectBrd = Converters.vector_Point2f_to_Mat(ptsBoard);
		Mat rectWin = Converters.vector_Point2f_to_Mat(ptswindow);

		homo = Imgproc.getPerspectiveTransform(rectBrd, rectWin);
	}

	public void rotate(ref Mat img)
	{
		Core.transpose(img, img);

		Mat tmp = transform(img);

		img = tmp.clone();
	}

	public Mat transform(Mat img)
	{        
		Mat rst = new Mat();

		Imgproc.warpPerspective(img, rst, homo, img.size());

		return rst;
	}
}
