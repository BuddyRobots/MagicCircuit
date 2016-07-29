using UnityEngine;
using System.Collections;

public class CircuitItemManager :MonoBehaviour
{
	
	//坐标管理类


	public static CircuitItemManager _instance;

	private CircuitItem item;




	void Awake()
	{
		_instance = this;

	}


	//private static CircuitItemManager _instance=null;
	//private CircuitItemManager(){}
	//先假设几个坐标



//	public static CircuitItemManager Instance
//	{
//
//		get
//		{ 
//			return _instance ?? (_instance = new CircuitItemManager ());
//		}
//	}

	//获取坐标,数据填充
	//先假设几个坐标
	public void GetVector()
	{


	}

	//坐标匹配

	//
	//
	//
	//

}
