using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using OpenCVForUnity;
using System.IO;

namespace OpenCVForUnitySample
{

    public class CircularDist : Feature
    {
        public Point center;
        public float radius;
        public List<float> distAry;
        private int binNum;

        public CircularDist(MyObject obj)
        {
            m_Obj = obj;
            binNum = 36;
            distAry = new List<float>();
        }

        public override void Calculate()
        {
            FigProc.FindHullCircle(m_Obj.imgPts, ref center, ref radius);
            // distAry = FigProc.FindCircularDist(m_Obj.transImg, binNum, center);
        }

        public static Feature LoadStdFeature(string objType)
        {
            CircularDist dist = new CircularDist(null);
            /*
            StreamReader sr = new StreamReader("Assets/Features/" + objType);
            string line;
            while ((line = sr.ReadLine()) != null)
            */
            string appleFeature = "size:375,315\ncenter: 187,178\nradius:145.248\ndistAry: 0.026057,0.0255654,0.0299902,0.0280236,0.0290069,0.0290069,0.0349066,0.0280236,0.0358899,0.0373648,0.0998033,0.0447394,0.0290069,0.0285152,0.0255654,0.0270403,0.0245821,0.0235988,0.0216323,0.0196657,0.0221239,0.0211406,0.0196657,0.0211406,0.020649,0.0186824,0.0176991,0.0186824,0.0221239,0.0265487,0.0240905,0.0231072,0.0240905,0.0240905,0.0245821,0.0235988";
            string[] lines = appleFeature.Split('\n');
            for (int index = 0; index < lines.Length; index++)
            {
                string line = lines[index];
                string[] lineContent = line.Split(':');
                string[] content;
                if (lineContent[0] == "size")
                {
                    content = lineContent[1].Split(',');
                    dist.width = int.Parse(content[0]);
                    dist.height = int.Parse(content[1]);
                }
                if (lineContent[0] == "center")
                {
                    content = lineContent[1].Split(',');
                    dist.center = new Point(double.Parse(content[0]), double.Parse(content[1]));
                }
                if (lineContent[0] == "radius")
                {
                    dist.radius = float.Parse(lineContent[1]);
                }
                if (lineContent[0] == "distAry")
                {
                    content = lineContent[1].Split(',');
                    for (int i = 0; i < content.Length; i++)
                    {
                        dist.distAry.Add(float.Parse(content[i]));
                    }
                }
            }
            return dist;
        }
    }
}
