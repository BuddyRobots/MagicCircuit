using OpenCVForUnity;
using MagicCircuit;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class RecognizeAlgo
{
	[DllImport("__Internal")]  
	private static extern int testLuaWithArr(double[] arr,int len);

    // Size of input image 1*28*28
    private const int imageSize = 28;
	private const int numOfClasses = 9;

    private LineDetector line_detector;
    //private myUtils util;


    public RecognizeAlgo()
    {
        line_detector = new LineDetector();
        //util = new myUtils();
    }
		
    public Mat process(Mat frameImg, ref List<CircuitItem> itemList)
    {
        Mat grayImg = new Mat();
        Mat binaryImg = new Mat();
        Mat frameTransImg = new Mat();
        Mat resultImg = frameImg.clone();

		int showOrder = 0;
        CircuitItem tmpItem;

        /// Detect Cards =============================================================





		int startTime_1 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;




		MatOfPoint2f point = new MatOfPoint2f(new Point[4]
            { new Point(0, 0), new Point(imageSize, 0), new Point(imageSize, imageSize), new Point(0, imageSize) });

        // Thresholding
        Imgproc.cvtColor(frameImg, grayImg, Imgproc.COLOR_BGR2GRAY);
        Imgproc.adaptiveThreshold(grayImg, binaryImg, 255, Imgproc.ADAPTIVE_THRESH_GAUSSIAN_C, Imgproc.THRESH_BINARY, 15, 1);

        // Get all the squares
        List<List<Point>> squares = new List<List<Point>>();
        List<List<Point>> outer_squares = new List<List<Point>>();
        squares = CardDetector.findSquares(binaryImg);
        outer_squares = CardDetector.computeOuterSquare(squares);

        for (int i = 0; i < squares.Count; i++)
        {
            // Draw lines on resultImg
            for (int j = 0; j < squares[i].Count - 1; j++)
                Imgproc.line(resultImg, squares[i][j], squares[i][j + 1], new Scalar(255, 0, 0), 3);
            Imgproc.line(resultImg, squares[i][squares[i].Count - 1], squares[i][0], new Scalar(255, 0, 0), 3);

            // Perspective transform
            Mat homography = Calib3d.findHomography(new MatOfPoint2f(squares[i].ToArray()), point);
            Imgproc.warpPerspective(frameImg, frameTransImg, homography, new Size());

            // TODO
			// @@predict
            // @Input  : Mat cardTransImg.submat(0, imageSize, 0, imageSize);
			// @Output : int klass;
			string name = "name";
            ItemType type = new ItemType();            
			Mat cardImg = frameTransImg.submat(0, imageSize, 0, imageSize);
			int klass = predict(cardImg);




			Debug.Log("RecognizeAlgo.process klass = " + klass);








			switch (klass)
			{
			case 1:
				name = "Battery";
				type = ItemType.Battery;
				break;
			case 2:
				name = "Switch";
				type = ItemType.Switch;
				break;
			case 3:
				name = "LightActSwitch";
				type = ItemType.LightActSwitch;
				break;
			case 4:
				name = "VoiceOperSwitch";
				type = ItemType.VoiceOperSwitch;
				break;
			case 5:
				name = "VoiceTimedelaySwitch";
				type = ItemType.VoiceTimedelaySwitch;
				break;
			case 6:
				name = "SPDTSwitch";
				type = ItemType.SPDTSwitch;
				break;
			case 7:
				name = "Bulb";
				type = ItemType.Bulb;
				break;
			case 8:
				name = "LoudSpeaker";
				type = ItemType.Loudspeaker;
				break;
			case 9:
				name = "InductionCooker";
				type = ItemType.InductionCooker;
				break;
			}

			// TODO
			// @@getDirection
			// @Input  : int klass,
			//           Mat cardImg;
			// @Output : int direction (0, 1, 2, 3)
			int direction = 1;
			// direction = getDirection(klass, cardImg);


            // Add to listItem
			tmpItem = new CircuitItem(klass, name, type, showOrder++, frameImg.size());
            tmpItem.extractCard(direction, outer_squares[i]);
            itemList.Add(tmpItem);
        }
		// ReOrder listItem
		reOrder(ref itemList);

        // Substract all outer_squares from frameImg
        CardDetector.removeCard(ref frameImg, outer_squares);






		int time_1 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
		int elapse_1 = time_1 - startTime_1;
		Debug.Log("RecognizeAlgo DetectCards Time elapse : " + elapse_1);





        /// Detect Lines =============================================================
        
		Debug.Log("DetectLine Start");
		int startTime_2 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;



		List<List<List<Point>>> listLineGroup = new List<List<List<Point>>>();
		List<OpenCVForUnity.Rect> rect = new List<OpenCVForUnity.Rect>();
        line_detector.detectLine(frameImg, ref listLineGroup, ref rect);

        /*for (var i = 0; i < listLine.Count; i++)
            util.drawPoint(resultImg, listLine[i], rect[i]);*/



		Debug.Log("DetectLine listLineGroup[0].Count = " + listLineGroup[0].Count);



        // Add to CircuitItem
		for (var i = 0; i < listLineGroup.Count; i++)
            for (var j = 0; j < listLineGroup[i].Count; j++)
            {
                tmpItem = new CircuitItem(showOrder, "CircuitLine", ItemType.CircuitLine, showOrder++, frameImg.size());
                tmpItem.extractLine(listLineGroup[i][j], rect[i]);
                itemList.Add(tmpItem);
            }






		int time_2 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
		int elapse_2 = time_2 - startTime_2;
		Debug.Log("RecognizeAlgo DetectLines Time elapse : " + elapse_2);
		for(var i = 0; i < itemList.Count; i++)
		{
			Debug.Log("RecogniazeAlgo itemList " + i + " : type = " + itemList[i].type);
		}







        return resultImg;
    }
		
	// Call lua to do classification
	private int predict(Mat card)
	{
		int startTime = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;





		double[] sample = new double[28*28*3];

		int pointer = 0;
		for (var y = 0; y < card.rows(); y++)
			for (var x = 0; x < card.cols(); x++)
			{
				double[] value = new double[3];
				value = card.get(x, y);

				sample[pointer]        = value[0] / 255;
				sample[pointer + 784]  = value[1] / 255;
				sample[pointer + 1568] = value[2] / 255;
				pointer++;
			}
		int prediction = testLuaWithArr(sample, sample.Length);





		int time_2 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
		int elapse_2 = time_2 - startTime;
		Debug.Log("RecognizeAlgo predict() Time elapse : " + elapse_2);



		return prediction;
	}

	private void reOrder(ref List<CircuitItem> listItem)
	{
		int counter = 0;
		List<CircuitItem> tmpList = new List<CircuitItem>();

		for (var i = 1; i <= numOfClasses; i++)
			for (var j = 0; j < listItem.Count; j++)
				if (listItem[j].ID == i)
				{
					listItem[j].ID = counter++;
					tmpList.Add(listItem[j]);
				}
		listItem = tmpList;
	}

	/*public List<Mat> createDataSet(Mat frameImg)
    {
		
        Mat grayImg = new Mat();
        Mat binaryImg = new Mat();
        Mat cardTransImg = new Mat();
        Mat cardImg = new Mat();
        List<Mat> result = new List<Mat>();

        /// Detect Cards =============================================================
        MatOfPoint2f point = new MatOfPoint2f(new Point[4]
            { new Point(0, 0), new Point(imageSize, 0), new Point(imageSize, imageSize), new Point(0, imageSize) });

        // Thresholding
        Imgproc.cvtColor(frameImg, grayImg, Imgproc.COLOR_BGR2GRAY);
        Imgproc.adaptiveThreshold(grayImg, binaryImg, 255, Imgproc.ADAPTIVE_THRESH_GAUSSIAN_C, Imgproc.THRESH_BINARY, 15, 1);

        // Get all the squares
        List<List<Point>> squares = new List<List<Point>>();
        squares = CardDetector.findSquares(binaryImg);

        for (int i = 0; i < squares.Count; i++)
        {
            // Perspective transform
            Mat homography = Calib3d.findHomography(new MatOfPoint2f(squares[i].ToArray()), point);
            Imgproc.warpPerspective(frameImg, cardTransImg, homography, new Size());

            cardImg = cardTransImg.submat(0, imageSize, 0, imageSize);

            result.Add(cardImg);
            //path = path + "/" + System.DateTime.Now.Ticks + ".jpg";

            //Imgcodecs.imwrite(path, cardImg);
        }
        return result;
    }*/
}