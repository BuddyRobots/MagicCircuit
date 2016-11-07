using UnityEngine;
using System.Collections;

public class LevelTen : MonoBehaviour {
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

			Transform normalSwitch=transform.Find("switch");


//			GetImage._instance.cf.switchOnOff (int.Parse (normalSwitch.tag), normalSwitch.GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);
//			CommonFuncManager._instance.CircuitItemRefresh (	GetImage._instance.itemList);

			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
		}
	
	}
}
