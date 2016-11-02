﻿using OpenCVForUnity;
using System.Collections.Generic;

using UnityEngine;

namespace MagicCircuit
{
    public enum ItemType
    {
        Battery,
        Switch,
        Bulb,		
		Loudspeaker,

		/// <summary>
		/// single pole double throw switch  单刀双掷开关
		/// </summary>
		SPDTSwitch,

		/// <summary>
		/// voice operated switch  声控开关
		/// </summary>
		VoiceOperSwitch,

		/// <summary>
		/// light activated switch  光敏开关
		/// </summary>
		LightActSwitch,	

		/// <summary>
		/// time-delay switch      声控延时开关
		/// </summary>
		VoiceTimedelaySwitch,	

		/// <summary>
		/// 电磁炉
		/// </summary>
		InductionCooker,

		/// <summary>
		/// line 线
		/// </summary>
        CircuitLine                             //如果是线的话，则是点的集合
    }
    
	public class CircuitItem                        //图标管理类 (id,名字，类型，坐标)
    {
        public int           ID        {get; set;}
        public string        name      {get; set;}
        public ItemType      type      {get; set;}  //图标类型
        public List<Vector3> list      {get; set;}  //图标的坐标
		public double        theta     {get; set;}  //图标的朝向（单位：角度）
		public int           showOrder {get; set;}  //显示顺序 从0开始（图标的显示顺序是灯泡）
		public bool          powered   {get; set;}  //元件是否通电

		public enum PowerStatus
		{
			noBattery,
			oneBattery,
			twoBattery
		}
		[HideInInspector]
		public PowerStatus powerStatus = PowerStatus.noBattery;
			
        public Vector2 connect_left;            // Connect point on card
        public Vector2 connect_right;			// For lines : connect_left == start, connect_right == end
		public Vector2 connect_middle;	

		public CircuitItem()
        {}

        // @Override
        public CircuitItem(int _id, string _name, ItemType _type, List<Point> _list, double _theta, int _order, bool _p = false)
        {
            ID = _id;
            name = _name;
            type = _type;
            list = points2vector3(_list);
            theta = _theta;
            showOrder = _order;
            powered = _p;

            connect_left = new Vector2();
            connect_right = new Vector2();
			connect_middle= new Vector2();
        }

        // @Override
        public CircuitItem(int _id, string _name, ItemType _type, int _order, bool _p = false)
        {
            ID = _id;
            name = _name;
            type = _type;
            showOrder = _order;
            powered = _p;
            list = new List<Vector3>();
            theta = 0;

            connect_left = new Vector2();
            connect_right = new Vector2();
			connect_middle= new Vector2();
        }

        // @Override  
        // Constructor for reading in Xml		
        public CircuitItem(XmlCircuitItem src)
        {
            ID = src.ID;

            name = src.name;
            type = src.type;
            list = src.list;

            theta = src.theta;
            showOrder = src.showOrder;
            powered = src.powered;
            connect_left = src.connect_left;
            connect_right = src.connect_right;
			connect_middle= src.connect_middle;
        }		

        // @Override
        public void extractCard(int direction, List<Point> outer_square)
        {
            Point center = new Point((outer_square[0].x + outer_square[2].x) / 2, (outer_square[0].y + outer_square[2].y) / 2);



//			Debug.Log("Utils : extractCard : center.x = " + center.x + " y = " + center.y);

            list.Add(cordinateMat2Unity(center.x, center.y));

            double angle = Mathf.Atan2((float)(outer_square[0].y - outer_square[1].y), (float)(outer_square[0].x - outer_square[1].x));
            theta = Mathf.PI / 2 * direction + angle; // 0 < theta < 2 * PI

            // Calculate connect_left & connect_right
            double width = Mathf.Sqrt(Mathf.Pow((float)(outer_square[0].x - outer_square[1].x), 2) + Mathf.Pow((float)(outer_square[0].y - outer_square[1].y), 2));


            double x = width / 2 / (1 + Mathf.Tan((float)theta));
            double y = x * Mathf.Tan((float)theta);

            connect_left = new Vector2((float)(center.x - x), (float)(center.y - y));
            connect_right = new Vector2((float)(center.x + x), (float)(center.y + y));

            theta = theta * Mathf.Rad2Deg; // 0 < theta < 360
        }

        public void extractLine(List<Point> line, OpenCVForUnity.Rect rect)
        {
            Point center = new Point(rect.tl().x, rect.tl().y);

            for (var i = 0; i < line.Count; i++)
            {
                list.Add(cordinateMat2Unity((line[i].x + center.x), (line[i].y + center.y)));
            }
			connect_left = new Vector2(list[0].x, list[0].y);
            connect_right = new Vector2(list[list.Count - 1].x, list[list.Count - 1].y);
        } 

		private List<Vector3> points2vector3(List<Point> src)
        {
            List<Vector3> res = new List<Vector3>();
            for (var i = 0; i < src.Count; i++)
            {
                res.Add(cordinateMat2Unity(src[i].x, src[i].y));
            }
            return res;
        }

        private Vector3 cordinateMat2Unity(double x, double y)
        {            


//			Debug.Log("Utils : cordinateMat2Unity : x = " + x + " y = " + y);
//			Debug.Log("Utils : cordinateMat2Unity : x' = " + (float)(x + Constant.CAM_QUAD_ORIGINAL_POINT_X) + " y' = " + (float)(Constant.CAM_QUAD_ORIGINAL_POINT_Y - y));





			return new Vector3((float)(x + Constant.CAM_QUAD_ORIGINAL_POINT_X), (float)(Constant.CAM_QUAD_ORIGINAL_POINT_Y - y));
        }
    }
}
 