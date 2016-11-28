using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;
//第4关
public class LevelFour : MonoBehaviour 
{

	[HideInInspector]
	public bool isLoudSpeakerOccur = false;

	void OnEnable()
	{
		isLoudSpeakerOccur = false;
	}
		
	void Update () 
	{
		if(isLoudSpeakerOccur)
		{
			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
		}
	}
}
