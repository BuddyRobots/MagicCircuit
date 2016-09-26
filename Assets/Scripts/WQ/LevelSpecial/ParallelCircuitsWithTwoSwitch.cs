using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;
//level 6
public class ParallelCircuitsWithTwoSwitch : MonoBehaviour 
{
	[HideInInspector]
	public bool isParrallelCircuit = false;
	private bool isCircuitAnimationPlayed=false;
	private GameObject clickBattery =null;
	private List<GameObject> switchList = null; 

	void OnEnable ()
	{
		isParrallelCircuit = false;
	}
	

	void Update () 
	{
		if (isParrallelCircuit) 
		{
			switchList = PhotoRecognizingPanel._instance.switchList;	
			for (int i = 0; i < switchList.Count; i++) //点击开关，调用方法，circuitItems更新powered属性
			{
				CurrentFlow._instance.switchOnOff (int.Parse(switchList [i].tag), switchList [i].GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);

				CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);//使用新的circuititems

//				foreach (var item in CurrentFlow._instance.circuitItems) 
//				{
//					if (item.type == ItemType.CircuitLine) 
//					{
//						print (item.ID + ":" + item.powered);
//					}
//				}
			}
		}
	}
}






