using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

//level 7
public class ParallelCircuitWithTwoBattery : MonoBehaviour 
{
	[HideInInspector]
	public bool isParallelCircuitWithTwoBattery=false;
	private bool isCircuitAnimationPlayed=false;
	private List<GameObject> batteryList = null;
	private List<GameObject> switchList = null;
	private GameObject clickBattery =null;


	void OnEnable () 
	{
		isParallelCircuitWithTwoBattery=false;
		isCircuitAnimationPlayed=false;
	}
	

	void Update () 
	{
		if (isParallelCircuitWithTwoBattery) 
		{
			batteryList = PhotoRecognizingPanel._instance.batteryList;
			switchList = PhotoRecognizingPanel._instance.switchList;
			for (int i = 0; i < switchList.Count; i++) 
			{
				CurrentFlow._instance.switchOnOff (int.Parse(switchList [i].tag), switchList [i].GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);
				CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);//使用新的circuititems
			}

//			if (transform.Find("arrow").GetComponent<UISprite>().alpha==1) 
//			{
//				isCircuitAnimationPlayed = true;
//			}
			isCircuitAnimationPlayed=CircuitPowerdOrNot();
			if (isCircuitAnimationPlayed) //电池可以被点击
			{
				clickBattery = batteryList[1];//识别部分设定是ID为0的不能点击，为1的可以点击
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(clickBattery.transform.localPosition);
				clickBattery.AddComponent<UIButton> ();//给随机的电池添加button组件和BatteryCtrl组件来实现点击事件
				clickBattery.AddComponent<BatteryCtrl> ();

				BussinessLogic BL = new BussinessLogic ();
				if (clickBattery.GetComponent<BatteryCtrl> ().isSemiTrans) //电池变成半透明，1个电池工作
				{ 
					BL.BatteryClick (CurrentFlow._instance.circuitItems, true);// for test
					if (PhotoRecognizingPanel._instance.finger) 
					{
						Destroy (PhotoRecognizingPanel._instance.finger);
					}
				} 
				else 
				{
					BL.BatteryClick (CurrentFlow._instance.circuitItems, false);
				}
			}
		}
	}
		
	/// <summary>
	/// if the circuit is powerd and show animation
	/// </summary>
	/// <returns><c>true</c>, if powerd or not was circuited, <c>false</c> otherwise.</returns>
	bool CircuitPowerdOrNot()
	{

		foreach (var item in CurrentFlow._instance.circuitItems) 
		{
			if (item.powered)
			{
				return true;
				break;
			}

		}
		return false;
	}

}
