using UnityEngine;
using System.Collections;

using OpenCVForUnity;

namespace OpenCVForUnitySample
{

    public abstract class Feature
    {
        public abstract void Calculate();

        public int width;
        public int height;
        public MyObject m_Obj;

    }

}