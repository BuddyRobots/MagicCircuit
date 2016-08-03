using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitItemManager :MonoBehaviour
{
	//坐标管理类

	public static CircuitItemManager _instance;

	//private CircuitItem item;

	public List<CircuitItem> itemList=new List<CircuitItem>();
	public List<CircuitItem> itemListTest=new List<CircuitItem>();

	void Awake()
	{
		_instance = this;

	}

	void Start () 
	{
		
		initCircuitItemListData();
		GetCircuitVec (itemListTest);
	}

	//初始化电路测试数据-----for test....
	void initCircuitItemListData()
	{


		CircuitItem item1 = new CircuitItem();
		item1.ID = 0;
		item1.name = "Battery";
		item1.type = ItemType.Battery;
		Vector3 vec1 = new Vector3 (0.5f,-0.1f,0);
		List<Vector3> vecs1 = new List<Vector3> ();
		vecs1.Add (vec1);
		item1.list=vecs1;
		// (new Vector3(247f,-62f,0));
		item1.showOrder = 0;

		CircuitItem item2 = new CircuitItem();
		item2.ID = 1;
		item2.name = "Bulb";
		item2.type = ItemType.Bulb;
		Vector3 vec2 = new Vector3 (-0.4f,0,0);
		List<Vector3> vecs2 = new List<Vector3> ();
		vecs2.Add (vec2);
		item2.list=vecs2;
		// (new Vector3(-252f,-69f,0));
		item2.showOrder = 1;

		CircuitItem item3 = new CircuitItem();
		item3.ID = 2;
		item3.name = "Switch";
		item3.type = ItemType.Switch;
		Vector3 vec3 = new Vector3 (0.1f,0.4f,0);
		List<Vector3> vecs3 = new List<Vector3> ();
		vecs3.Add (vec3);
		item3.list=vecs3;
		//item3.list.Add (new Vector3(-19f,-51f,0));
		item3.showOrder = 2;

		CircuitItem item4 = new CircuitItem();
		item4.ID = 3;
		item4.name = "CircuitLine";
		item4.type = ItemType.CircuitLine;
		Vector3 vec01 = new Vector3 (-0.3f, 0, 0);
		Vector3 vec02 = new Vector3 (-0.2f, 0, 0);
		Vector3 vec03 = new Vector3 (-0.1f, 0, 0);
		Vector3 vec04 = new Vector3 (0, 0, 0);
		Vector3 vec05 = new Vector3 (0.1f, 0, 0);
		Vector3 vec06 = new Vector3 (0.2f, 0, 0);
		Vector3 vec07 = new Vector3 (0.3f, 0, 0);
		Vector3 vec08 = new Vector3 (0.4f, 0, 0);
		Vector3 vec09 = new Vector3 (0.5f, -0.1f, 0);
		List<Vector3> vecs4 = new List<Vector3> ();
		vecs4.Add (vec01);
		vecs4.Add (vec02);
		vecs4.Add (vec03);
		item4.list = vecs4;
		item4.showOrder = 3;

		itemListTest.Add(item1);
		itemListTest.Add(item2);
		itemListTest.Add(item3);
		itemListTest.Add(item4);

		//Debug.Log ("itemListTest.count====" + itemListTest.Count);

	}


	//获取坐标,数据填充
	public void GetCircuitVec(List<CircuitItem> circuitItemList)
	{
		/*
		//拷贝传进来的值
		for(int i=0;i<circuitItemList.Count;++i)
		{
			//判断传进来的list是否是按showOrder从小到大排序的
			if (circuitItemList [i].showOrder == i)
			{
				CircuitItem item= circuitItemList [i];
				itemList.Add(item);
			}

		}
		*/

		//for test...
		//拷贝传进来的值
		for(int i=0;i<itemListTest.Count;++i)
		{
			//判断传进来的list是否是按showOrder从小到大排序的
			if (circuitItemList[i].showOrder == i)
			{
				CircuitItem item= itemListTest [i];
				itemList.Add(item);
			}

		}
			
	}

}
