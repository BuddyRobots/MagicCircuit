using UnityEngine;
using System.Collections;


public enum ItemType
{
	Battery,
	Switch,
	Bulb,
	CircuitLine

}
public class CircuitItem  
{
	//图标管理类     id,名字，类型，坐标

	public int ID{ get; set;}
	public string name{ get; set;}
	public ItemType type{ get; set;}
	public Vector3 vec{ get; set; }





}
