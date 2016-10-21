using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

//level 7-----并联电路，2个电池+2个开关+1个灯泡+1个音响（两个开关分别控制灯泡和音响），电路通电流后有一个电池可以被点击（在透明和半透明之间切换）
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
			isCircuitAnimationPlayed=CircuitPowerdOrNot();
			//isCircuitAnimationPlayed = CommonFuncManager._instance.CircuitPowerdOrNot ();
			if (isCircuitAnimationPlayed) //电池可以被点击
			{
				clickBattery = batteryList[1];//识别部分设定是ID为0的不能点击，为1的可以点击
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(clickBattery.transform.localPosition);//show finger
				clickBattery.AddComponent<UIButton> ();//给随机的电池添加button组件和BatteryCtrl组件来实现点击事件
				clickBattery.AddComponent<BatteryCtrl> ();

				BussinessLogic BL = new BussinessLogic ();
				if (clickBattery.GetComponent<BatteryCtrl> ().isSemiTrans) //点击了电池，电池变成半透明，1个电池工作
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
	public bool CircuitPowerdOrNot()
	{

		foreach (var item in CurrentFlow._instance.circuitItems) 
		{
			if (item.powered && item.type==ItemType.Switch)
			{
				return true;
				//break;
			}

		}
		return false;
	}

}
