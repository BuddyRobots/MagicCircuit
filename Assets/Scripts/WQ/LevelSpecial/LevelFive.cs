using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;


//level 5
public class LevelFive : MonoBehaviour 
{
	private List<GameObject> normalSwitchList =null;

	[HideInInspector]
	public bool isTwoSwitchInSeriesCircuit = false;

	void OnEnable () 
	{
		isTwoSwitchInSeriesCircuit = false;
	}


	void Update () 
	{
		if (isTwoSwitchInSeriesCircuit) 
		{
			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
		}
	}
}
