using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using OpenCVForUnity;

namespace OpenCVForUnitySample
{

    public class Tree : MyObject
    {
        public Tree(int width, int height, List<Point> imgPts, bool std) : base(width, height, imgPts)
        {
            this.feature = new Axis(this);
            if (!std)
                this.stdFeature = Axis.LoadStdFeature(Tree.ObjName());
        }

        public static string ObjName()
        {
            return "tree";
        }

        public override void GetLocation(ref float x, ref float y)
        {
            x = (float)(((Axis)this.feature).bottom.x + ((Axis)this.feature).top.x) / 2 - this.expand / 2;
            y = (float)(((Axis)this.feature).bottom.y + ((Axis)this.feature).top.y) / 2 - this.expand / 2;
        }

        public override void GetRadius(ref float radius)
        {
            radius = (float)FigProc.FindLen(((Axis)this.feature).top, ((Axis)this.feature).bottom) / 2.0f;
        }

    }
}