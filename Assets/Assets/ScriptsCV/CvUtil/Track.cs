using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using OpenCVForUnity;

namespace OpenCVForUnitySample
{

    public class Track
    {

        public Track()
        {
            m_Ready = false;
            m_objType = "";
            m_CurObjType = "";
            m_RPos = new Vector3();
        }

        public string m_objType;
        public MyObject m_obj;

        public float m_Scale;
        public float m_X;
        public float m_Y;
        public float m_Z;
        public float m_rotX;
        public float m_rotY;
        public float m_rotZ;

        public float m_RScale;
        public Vector3 m_RPos;
        public float m_RrotX;
        public float m_RrotY;
        public float m_RrotZ;

        public bool m_Ready;

        public GameObject m_CurObj;
        public string m_CurObjType;

        public float m_MatX;
        public float m_MatY;
        public float m_MatRadius;

        public void Clear()
        {
            m_Ready = false;
            m_CurObjType = "";
            m_objType = "";
        }

        public void Set(string objType, float x, float z, float scale)
        {
            m_objType = objType;
            m_Scale = scale;
            m_X = x;
            m_Y = 0.0f;
            m_Z = z;
            m_rotX = 0.0f;
            m_rotY = 0.0f;
            m_rotZ = 0.0f;
        }

        private void Recgonize(List<Point> imgPts)
        {
            double min_x = 10000, min_y = 10000, max_x = 0, max_y = 0;

            for (int i = 0; i < imgPts.Count; i++)
            {
                if (imgPts[i].x < min_x)
                {
                    min_x = imgPts[i].x;
                }
                if (imgPts[i].x > max_x)
                {
                    max_x = imgPts[i].x;
                }
                if (imgPts[i].y < min_y)
                {
                    min_y = imgPts[i].y;
                }
                if (imgPts[i].y > max_y)
                {
                    max_y = imgPts[i].y;
                }
            }
            double y_span = max_y - min_y;
            double x_span = max_x - min_x;

            double ratio = x_span / y_span;

            double threshold = 1.4;

            if (ratio > threshold)
            {
                m_objType = "tree";
            }
            else if (ratio < 1.0 / threshold)
            {
                m_objType = "house";
            }
            else
            {
                m_objType = "apple";
            }
        }

        public void Calculate(int width, int height, List<Point> imgPts, BaseParams baseParams)
        {
            Recgonize(imgPts);

            switch (m_objType)
            {
                case "apple":
                    m_obj = new Apple(width, height, imgPts, false);
                    break;
                case "tree":
                    m_obj = new Tree(width, height, imgPts, false);
                    break;
                case "house":
                    m_obj = new House(width, height, imgPts, false);
                    break;
                default:
                    m_obj = new Apple(width, height, imgPts, false);
                    break;
            }

            m_obj.CalcFeature();

            m_obj.GetRadius(ref m_MatRadius);
            m_obj.GetLocation(ref m_MatX, ref m_MatY);

            m_Scale = m_MatRadius;
            m_X = (m_MatX * 1.0f - baseParams.m_VideoWidth / 2) * baseParams.m_SurfWidth / baseParams.m_VideoWidth;
            m_Y = 0.0f;
            m_Z = -(m_MatY * 1.0f - baseParams.m_VideoHeight / 2) * baseParams.m_SurfHeight / baseParams.m_VideoHeight;
            m_rotX = 0.0f;
            m_rotY = 0.0f;
            m_rotZ = 0.0f;
        }

        public void Update()
        {
            m_RScale = m_Scale;
            m_RPos.x = m_X;
            m_RPos.y = m_Y;
            m_RPos.z = m_Z;
            m_RrotX = m_rotX;
            m_RrotY = m_rotY;
            m_RrotZ = m_rotZ;
            m_Ready = true;
        }
    }
}
