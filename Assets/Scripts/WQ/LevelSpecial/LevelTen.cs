﻿using UnityEngine;
using System.Collections;

public class LevelTen : MonoBehaviour 
{
	[HideInInspector]
	public bool isLevelTen=false;


	void OnEnable () 
	{
		isLevelTen=false;
	}
	

	void Update () 
	{
		if (isLevelTen) 
		{
			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
		}
	
	}
}
