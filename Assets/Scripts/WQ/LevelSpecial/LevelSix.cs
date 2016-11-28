using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

//level 6-----并联电路，1个电池+2个开关+1个灯泡+1个音响（两个开关分别控制灯泡和音响）
public class LevelSix : MonoBehaviour 
{
	[HideInInspector]
	public bool isParrallelCircuit = false;

	void OnEnable ()
	{
		isParrallelCircuit = false;
	}
	

	void Update () 
	{
		if (isParrallelCircuit) 
		{
			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
		}
	}
}






