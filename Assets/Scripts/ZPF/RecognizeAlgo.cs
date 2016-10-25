using OpenCVForUnity;
using MagicCircuit;
using System.Collections.Generic;

public class RecognizeAlgo
{

    // Size of input image 1*28*28
    private const int imageSize = 28;

    private LineDetector line_detector;
    private myUtils util;

    public RecognizeAlgo()
    {
        line_detector = new LineDetector();
        util = new myUtils();
    }


    public Mat process(Mat frameImg, ref List<CircuitItem> listItem)
    {
        Mat grayImg = new Mat();
        Mat binaryImg = new Mat();
        Mat cardTransImg = new Mat();
        Mat resultImg = frameImg.clone();

        int ID = 0;
        CircuitItem tmpItem;

        /// Detect Cards =============================================================
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
            Imgproc.warpPerspective(frameImg, cardTransImg, homography, new Size());

            // TODO
            // @@Classification
            // @Input  : Mat cardTransImg.submat(0, imageSize, 0, imageSize);
            // @Output : string name,
            //           ItemType type,
            //           int direction;
            // FIXME: cardImg BGR2RGB ????
            string name = "name";
            ItemType type = new ItemType();
            int direction = 0;
            Mat cardImg = cardTransImg.submat(0, imageSize, 0, imageSize);
            //int class = predict(cardTransImg.submat(0, imageSize, 0, imageSize))


            // Add to listItem
            tmpItem = new CircuitItem(ID, name, type, ID++, frameImg.size());
            tmpItem.extractCard(direction, outer_squares[i]);
            listItem.Add(tmpItem);
        }

        // Substract all outer_squares from frameImg
        CardDetector.removeCard(ref frameImg, outer_squares);


        /// Detect Lines =============================================================
        List<List<List<Point>>> listLine = new List<List<List<Point>>>();
        List<Rect> rect = new List<Rect>();
        line_detector.detectLine(frameImg, ref listLine, ref rect);

        for (var i = 0; i < listLine.Count; i++)
            util.drawPoint(resultImg, listLine[i], rect[i]);

        // Add to CircuitItem
        for (var i = 0; i < listLine.Count; i++)
            for (var j = 0; j < listLine[i].Count; j++)
            {
                tmpItem = new CircuitItem(ID, "CircuitLine", ItemType.CircuitLine, ID++, frameImg.size());
                tmpItem.extractLine(listLine[i][j], rect[i]);
                listItem.Add(tmpItem);
            }
        return resultImg;
    }

    public List<Mat> createDataSet(Mat frameImg/*, string path*/)
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
    }
}