using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using OpenCVForUnity;

namespace OpenCVForUnitySample
{
    public class House : MyObject
    {
        public House(int width, int height, List<Point> imgPts, bool std) : base(width, height, imgPts)
        {
            this.feature = new HorizonLine(this);
            if (!std)
                this.stdFeature = HorizonLine.LoadStdFeature(House.ObjName());
        }

        public static string ObjName()
        {
            return "house";
        }

        public override void GetLocation(ref float x, ref float y)
        {
            x = (float)(((HorizonLine)this.feature).left.x + ((HorizonLine)this.feature).right.x) / 2 - this.expand / 2;
            y = (float)(((HorizonLine)this.feature).left.y + ((HorizonLine)this.feature).right.y) / 2 - this.expand / 2;
        }

        public override void GetRadius(ref float radius)
        {
            radius = (float)FigProc.FindLen(((HorizonLine)this.feature).left, ((HorizonLine)this.feature).right) / 2.0f;
        }
    }
}