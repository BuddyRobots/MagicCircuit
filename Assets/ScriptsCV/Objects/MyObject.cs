using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using OpenCVForUnity;

namespace OpenCVForUnitySample
{

    public abstract class MyObject
    {
        public Feature feature;
        public Feature stdFeature;
        public List<Point> imgPts;
        public Mat oriImg;
        public Mat img;
        public Mat transImg;
        public Mat blurImg;
        public Mat erodeImg;
        public int expand;

        public abstract void GetLocation(ref float x, ref float y);
        public abstract void GetRadius(ref float radius);

        public MyObject(int width, int height, List<Point> imgPts)
        {
            img = FigProc.GenImgFromPts(width, height, imgPts);
            // expand = img.rows() > img.cols() ? img.rows() : img.cols();
            expand = 0;
            int expand_rows = expand + img.rows();
            int expand_cols = expand + img.cols();
            transImg = new Mat(expand_rows, expand_cols, CvType.CV_8UC1, Scalar.all(255));
            Mat sub = new Mat(transImg, new OpenCVForUnity.Rect(expand / 2, expand / 2, img.cols(), img.rows()));
            img.copyTo(sub);
            blurImg = new Mat(transImg.height(), transImg.width(), CvType.CV_8UC1);
            Imgproc.GaussianBlur(transImg, blurImg, new Size(9, 9), 2, 2);

            int erosion_type = Imgproc.MORPH_CROSS;
            int erosion_size = 2;
            Mat element = Imgproc.getStructuringElement(erosion_type,
                new Size(2 * erosion_size + 1, 2 * erosion_size + 1),
                new Point(erosion_size, erosion_size));
            erodeImg = new Mat(transImg.height(), transImg.width(), CvType.CV_8UC1);
            Imgproc.erode(transImg, erodeImg, element);

            imgPts = new List<Point>();
            FigProc.Img2Pts(this.transImg, ref this.imgPts);
        }

        public void CalcFeature()
        {
            this.feature.Calculate();
        }
    }
}