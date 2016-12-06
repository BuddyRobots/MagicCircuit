using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

//level 8-----并联电路，2个电池+3个开关+1个灯泡+1个音响+1个电磁炉（3个开关分别控制灯泡，音响，电磁炉；电池不可点击）
public class LevelEight: MonoBehaviour 
{
	[HideInInspector]
	public bool isLevelEight = false;

	void OnEnable () 
	{
		isLevelEight = false;
	}
	

	void Update () 
	{
		if (isLevelEight) 
		{
			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
		}
	
	}
}
