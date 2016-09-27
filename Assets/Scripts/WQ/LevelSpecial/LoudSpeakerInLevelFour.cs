using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;
//第4关
public class LoudSpeakerInLevelFour : MonoBehaviour 
{

	[HideInInspector]
	public bool isLoudSpeakerOccur = false;
	private List<GameObject> normalSwitchList =null;

	void OnEnable()
	{
		isLoudSpeakerOccur = false;
	}
		
	void Update () 
	{
		if(isLoudSpeakerOccur)
		{

			normalSwitchList = GetComponent<PhotoRecognizingPanel> ().switchList;

			for (int i = 0; i < normalSwitchList.Count; i++) 
			{
				CurrentFlow._instance.switchOnOff (int.Parse (normalSwitchList [i].tag), normalSwitchList [i].GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);

				CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);

			}
		}

	}
}
