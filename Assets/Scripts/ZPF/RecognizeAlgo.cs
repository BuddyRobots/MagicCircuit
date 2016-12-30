using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenCVForUnity;

namespace MagicCircuit
{
	public class RecognizeAlgo
	{
		private static MatOfPoint2f modelImageSizePoint;
		private LineDetector lineDetector;


	    public RecognizeAlgo()
	    {
	        lineDetector = new LineDetector();

			modelImageSizePoint = new MatOfPoint2f(new Point[4]{
				new Point(0, 0),
				new Point(Constant.MODEL_IMAGE_SIZE, 0),
				new Point(Constant.MODEL_IMAGE_SIZE, Constant.MODEL_IMAGE_SIZE),
				new Point(0, Constant.MODEL_IMAGE_SIZE)});
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



			///
			int startTime_1 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
			///



	        // Thresholding
			// Works best with Imgproc.THRESH_BINARY
	        Imgproc.cvtColor(frameImg, grayImg, Imgproc.COLOR_BGR2GRAY);
			Imgproc.adaptiveThreshold(grayImg, binaryImg, 255, Imgproc.ADAPTIVE_THRESH_GAUSSIAN_C, Imgproc.THRESH_BINARY, Constant.CARD_ADPTTHRES_KERNEL, Constant.CARD_ADPTTHRES_SUB);



			///
			/*return binaryImg;
			// See the result of findContours
			Mat draw = new Mat(binaryImg.rows(), binaryImg.cols(), CvType.CV_8UC3);
			List<MatOfPoint> contours = new List<MatOfPoint>();

			Imgproc.findContours(binaryImg, contours, new Mat(), Imgproc.RETR_LIST, Imgproc.CHAIN_APPROX_SIMPLE, new Point(0, 0));
			for (var i = 0; i < contours.Count; i++)
				Imgproc.drawContours(draw, contours, i, new Scalar(i*240/contours.Count, i*240/contours.Count, i*240/contours.Count));
			return draw;*/
			///
			/*Mat labelImg = new Mat();//(binaryImg.rows(), binaryImg.cols(), CvType.CV_16UC1);
			Mat visualLabel = new Mat();
			Mat stats = new Mat();
			int numLabel = Imgproc.connectedComponentsWithStats(binaryImg, labelImg, stats, new Mat(), 8, CvType.CV_16U);
			Debug.Log("RecognizeAlgo.cs process() : numLabel = " + numLabel);
			Core.normalize(labelImg, visualLabel, 0, 255, Core.NORM_MINMAX, CvType.CV_8U);


			Debug.Log("RecognizeAlgo.cs process() : stats.rows() = " + stats.rows() + " cols() = " + stats.cols());
			for (var i = 0; i < numLabel; i++)
			{
				double[] stat_area = stats.get(i, Imgproc.CC_STAT_AREA);
				double[] stat_height = stats.get(i, Imgproc.CC_STAT_HEIGHT);
				double[] stat_width = stats.get(i, Imgproc.CC_STAT_WIDTH);
				//Debug.Log("RecognizeAlgo.cs process() : stat_area[0] = " + stat_area[0]);
				Debug.Log("RecognizeAlgo.cs process() : stats["+i+"] area = " + stat_area[0] + " height = " + stat_height[0] + " width = " + stat_width[0]);
			}
			return visualLabel;*/
			///
			//CardDetector_new.findSquares(binaryImg);
			///



	        // Get all the squares
	        List<List<Point>> squareList = new List<List<Point>>();
			List<List<Point>> outerSquareList = new List<List<Point>>();
			squareList = CardDetector_new.findSquares(binaryImg);
	        outerSquareList = CardDetector.computeOuterSquare(squareList);



			///
			Debug.Log("RecognizeAlgo.cs process : squareList.Count = " + squareList.Count);
			Debug.Log("RecognizeAlgo.cs process : outerSquareList.Count = " + outerSquareList.Count);
			///



	        for (int i = 0; i < squareList.Count; i++)
	        {
	            // Perspective transform
	            Mat homography = Calib3d.findHomography(new MatOfPoint2f(squareList[i].ToArray()), modelImageSizePoint);
	            Imgproc.warpPerspective(frameImg, frameTransImg, homography, new Size());

				// predictClass
	            // Input  : Mat cardTransImg.submat(0, imageSize, 0, imageSize);
				// Output : int klass;
				string name = "name";
	            ItemType type = new ItemType();            
				Mat cardImg = frameTransImg.submat(0, Constant.MODEL_IMAGE_SIZE, 0, Constant.MODEL_IMAGE_SIZE);
				double[] cardArray = mat2array(cardImg);
				int klass = 0;

				// predictDirection
				// Input  : int klass,
				//           Mat cardImg;
				// Output : int direction (1, 2, 3, 4)
				//int direction = 4;
				int direction = 0;
				correctDirection(ref direction, klass);

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

				// Add to listItem
				tmpItem = new CircuitItem(klass, name, type, showOrder++);
	            tmpItem.extractCard(direction, outerSquareList[i]);
	            itemList.Add(tmpItem);
	        }
			// ReOrder listItem
			reOrder(ref itemList);



			///
			int time_1 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
			int elapse_1 = time_1 - startTime_1;
			//Debug.Log("RecognizeAlgo.cs DetectCards Time elapse : " + elapse_1);

	        /// Detect Lines =============================================================        
			//Debug.Log("RecognizeAlgo.cs DetectLine Start!");
			int startTime_2 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
			///



			List<List<List<Point>>> lineGroupList = new List<List<List<Point>>>();
			List<OpenCVForUnity.Rect> boundingRectList = new List<OpenCVForUnity.Rect>();
			resultImg = lineDetector.detectLine(frameImg, lineGroupList, boundingRectList, outerSquareList);

			return resultImg;

	        // Add to CircuitItem
			for (var i = 0; i < lineGroupList.Count; i++)
	            for (var j = 0; j < lineGroupList[i].Count; j++)
	            {
	                tmpItem = new CircuitItem(showOrder, "CircuitLine", ItemType.CircuitLine, showOrder++);
	                tmpItem.extractLine(lineGroupList[i][j], boundingRectList[i]);
	                itemList.Add(tmpItem);
	            }



			///
			int time_2 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
			int elapse_2 = time_2 - startTime_2;
			//Debug.Log("RecognizeAlgo DetectLines Time elapse : " + elapse_2);
			///



	        return resultImg;
	    }





		private double[] mat2array(Mat img)
		{
			///
			int startTime = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
			///



			double[] sample = new double[28*28*3];

			int pointer = 0;
			for (var y = 0; y < img.rows(); y++)
				for (var x = 0; x < img.cols(); x++)
				{
					double[] value = new double[3];
					value = img.get(x, y);

					sample[pointer]        = value[0] / 255;
					sample[pointer + 784]  = value[1] / 255;
					sample[pointer + 1568] = value[2] / 255;
					pointer++;
				}



			///
			int currentTime = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
			int elapseTime = currentTime - startTime;
			//Debug.Log("RecognizeAlgo.cs mat2array() : Time elapse : " + elapseTime);
			///



			return sample;
		}


		private void reOrder(ref List<CircuitItem> listItem)
		{
			List<CircuitItem> tmpList = new List<CircuitItem>();

			for (var i = 1; i <= Constant.NUM_OF_CLASS; i++)
				for (var j = 0; j < listItem.Count; j++)
					if (listItem[j].ID == i)
						tmpList.Add(listItem[j]);
			for (var i = 0; i < tmpList.Count; i++)
				tmpList[i].ID = i;
			
			listItem = tmpList;
		}


		// TODO
		// Need to correct predicted direction due to unknown, strange bug
		private void correctDirection(ref int direction, int klass)
		{
			if (klass == 1 || klass == 8)
			{
				switch (direction)
				{
				case 1:
					direction = 2; break;
				case 2:
					direction = 1; break;
				case 3:
					direction = 4; break;
				case 4:
					direction = 3; break;
				}
			}
			else if (klass == 9)
			{
				switch (direction)
				{
				case 1:
					direction = 2; break;
				case 2:
					direction = 1; break;
				case 3:
					direction = 4; break;
				case 4:
					direction = 3; break;
				}
			}
			else
			{
				switch (direction)
				{
				case 1:
					direction = 4; break;
				case 2:
					direction = 3; break;
				case 3:
					direction = 2; break;
				case 4:
					direction = 1; break;
				}
			}
		}
	}
}