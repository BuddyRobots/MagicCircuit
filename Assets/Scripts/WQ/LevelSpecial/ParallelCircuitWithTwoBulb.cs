using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

// level 9-----并联电路，2个电池+3个开关+2个灯泡+1个音响+1个电磁炉（2个开关分别控制2个灯泡，音响，电磁炉；电池不可点击）
public class ParallelCircuitWithTwoBulb : MonoBehaviour 
{
	[HideInInspector]
	public bool isParallelCircuitWithTwoBulb = false;
	private List<GameObject> switchList = null; 
	private bool isCircuitAnimationPlayed=false;
	private GameObject clickBulb =null;
	private List<GameObject> bulbList = null;

	void OnEnable ()
	{
		isParallelCircuitWithTwoBulb = false;
		isCircuitAnimationPlayed=false;
	}
	

	//电流播放一会后，在灯泡的位置出现小手提示灯泡是可以点击的，（两个灯泡要确保有一个是在工作的，只能有一个能被点击变成半透明），
	//点击灯泡后，小手消失，灯泡变成半透明，另外一个灯泡变得更亮；再点击透明灯泡，灯泡复原成不透明，另外一个灯泡变正常亮
	void Update () 
	{
		if (isParallelCircuitWithTwoBulb) 
		{
			bulbList = PhotoRecognizingPanel._instance.bulbList;
			switchList = PhotoRecognizingPanel._instance.switchList;

			for (int i = 0; i < switchList.Count; i++) //点击开关，调用方法，circuitItems更新powered属性
			{
				CurrentFlow._instance.switchOnOff (int.Parse(switchList [i].tag), switchList [i].GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);
				CommonFuncManager._instance.CircuitResetWithTwoBattery (CurrentFlow._instance.circuitItems);//使用新的circuititems
			}
			isCircuitAnimationPlayed = CircuitPowerdOrNot ();
			if (isCircuitAnimationPlayed) //灯泡可以被点击
			{
				clickBulb = bulbList[1];//识别部分设定是ID为0的不能点击，为1的可以点击
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(clickBulb.transform.localPosition);//show finger
				clickBulb.AddComponent<UIButton> ();//给随机的电池添加button组件和BatteryCtrl组件来实现点击事件
				clickBulb.AddComponent<BatteryCtrl> ();

				BussinessLogic BL = new BussinessLogic ();
				if (clickBulb.GetComponent<BulbCtrl> ().isSemiTrans) //1个灯泡变成半透明
				{ 
					if (PhotoRecognizingPanel._instance.finger) 
					{
						Destroy (PhotoRecognizingPanel._instance.finger);
					}
					bulbList[0].GetComponent<UISprite>().spriteName="bulbOn";


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
			if (item.powered && item.type==ItemType.Bulb)
			{
				return true;
				//break;
			}

		}
		return false;
	}
}
