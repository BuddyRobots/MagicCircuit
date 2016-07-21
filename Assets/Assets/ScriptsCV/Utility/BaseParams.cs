using UnityEngine;
using System.Collections;

namespace OpenCVForUnitySample
{
    public struct BaseParams
    {
        public int m_VideoWidth;
        public int m_VideoHeight;

        // surface length in the x direction
        public float m_SurfWidth;
        // surface length in the y direction
        public float m_SurfHeight;

        public float m_SurfCenterX;
        public float m_SurfCenterY;

        public BaseParams(int VideoWidth, int VideoHeight, float SurfWidth, float SurfHeight)
        {
            this.m_VideoWidth = VideoWidth;
            this.m_VideoHeight = VideoHeight;
            this.m_SurfWidth = SurfWidth;
            this.m_SurfHeight = SurfHeight;
            this.m_SurfCenterX = 0.0f;
            this.m_SurfCenterY = 0.0f;
        }
    }
}