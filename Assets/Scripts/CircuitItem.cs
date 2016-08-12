using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum ItemType
{
	Battery,
	Switch,
	Bulb,
	CircuitLine//如果是线的话，则是点的集合

}

public class CircuitItem  
{
	//图标管理类     id,名字，类型，坐标

	public int ID{ get; set;}
	public string name{ get; set;}
	public ItemType type{ get; set;}
	public  List<Vector3> list{ get; set; }

	public int showOrder;//显示顺序

}
