using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using OpenCVForUnity;

namespace OpenCVForUnitySample
{
    public class Apple : MyObject
    {
        public Apple(int width, int height, List<Point> imgPts, bool std) : base(width, height, imgPts)
        {
            this.feature = new CircularDist(this);
            if (!std)
                this.stdFeature = CircularDist.LoadStdFeature(Apple.ObjName());
        }

        public static string ObjName()
        {
            return "apple";
        }

        public override void GetLocation(ref float x, ref float y)
        {
            x = (float)((CircularDist)this.feature).center.x - this.expand / 2;
            y = (float)((CircularDist)this.feature).center.y - this.expand / 2;
        }

        public override void GetRadius(ref float radius)
        {
            radius = ((CircularDist)this.feature).radius;
        }
    }
}