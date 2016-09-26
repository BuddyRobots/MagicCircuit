using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

//level 7
public class ParallelCircuitWithTwoBattery : MonoBehaviour 
{

	private bool isCircuitAnimationPlayed=false;
	public bool isParallelCircuitWithTwoBattery=false;
	private int animationPlayedTimes=0;
	private List<GameObject> batteryList = null;
	private List<GameObject> switchList = null;
	private GameObject clickBattery =null;
	//第7关，两节电池 + 开关 + 灯泡    	  —灯泡更亮
	//两节电池+ 开关 + 小喇叭    	 —喇叭更响
	//电流播放一会后，在电池的位置出现小手提示电池是可以点击的，（两个电池要确保有一个是在工作的，只能有一个能被点击变成半透明），
	//点击电池后，小手消失，电池变成半透明，电池的位置出现线连接原来的线的端点，电流也要走到半透明的电池之上，灯泡亮度变暗一点，喇叭声音变小；
	//电流继续播放两三秒后，在透明电池上出现小手，再点击透明电池的时候，小手消失，电池还原，电池位置的线消失，电池上面的电流也消失，灯泡亮度变更亮，喇叭声音变大；


	void OnEnable () 
	{
		isParallelCircuitWithTwoBattery=false;
		animationPlayedTimes=0;
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

			if (transform.Find("arrow").GetComponent<UISprite>().alpha==1) 
			{
				isCircuitAnimationPlayed = true;
			}
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

//			batteryList = PhotoRecognizingPanel._instance.batteryList;
//			switchList = PhotoRecognizingPanel._instance.switchList;
//			if (!PhotoRecognizingPanel._instance.isArrowShowDone) 
//			{
//
//
//
//				for (int i = 0; i < switchList.Count; i++) 
//				{
//					if (!switchList[i].GetComponent<SwitchCtrl>().isSwitchOn)//闭合
//					{
//						//如果控制灯泡的开关闭合，to do ...
//						transform.Find("bulb").GetComponent<UISprite>().spriteName="bulbSpark";
//
//						//如果控制小喇叭的开关闭合，走电流 to do...
//						transform.Find("loudspeaker").GetComponent<AudioSource>().Play();
//						transform.Find("loudspeaker").GetComponent<AudioSource>().volume=1f;
//
//						PhotoRecognizingPanel._instance.ArrowShowLineByLine (PhotoRecognizingPanel._instance.lines,0);//走电流
//
//						PhotoRecognizingPanel._instance.isArrowShowDone = true;
//						animationPlayedTimes = 1;
//
//					}
//				}
//			}
//			if (animationPlayedTimes == 1) //电流流通过一次后
//			{
//				clickBattery = batteryList[1];//识别部分设定是ID为0的不能点击，为1的可以点击
//				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(clickBattery.transform.localPosition);
//				clickBattery.AddComponent<UIButton> ();//给随机的电池添加button组件和BatteryCtrl组件来实现点击事件
//				clickBattery.AddComponent<BatteryCtrl> ();
//
//				// 两个电池   +  两个开关都闭合---------------------------------灯泡发光亮，喇叭正常响
//				// 两个电池   +  1个开关闭合--------如果闭合的开关是控制灯泡的------灯泡发光亮，喇叭不响  
//				//								 如果闭合的开关是控制喇叭的------灯泡不亮，喇叭正常响
//
//				// 1个电池    +  两个开关都闭合---------------------------------灯泡正常亮，喇叭声音变小
//				// 1个电池    + 1的开关闭合---------如果闭合的开关是控制灯泡的------灯泡正常亮，喇叭不响
//				//							     如果闭合的开关是控制喇叭的------喇叭正常响，灯泡不亮
//
//
//
//				if (clickBattery.GetComponent<BatteryCtrl> ().isSemiTrans) //电池变成半透明，1个电池工作
//				{ 
//
//					//如果电池是半透明状态，玩家在3秒之内没有再次点击电池，则在半透明电池出现小手提示玩家点击，如果玩家在3秒之内点击了半透明电池，就不用出现小手
//					// to do...
//
//					Destroy (PhotoRecognizingPanel._instance.finger);
//
//					//如果一个开关断开，一个开关闭合--------1-灯泡开关闭合，灯泡亮度变弱
//					//								   2-喇叭开关闭合---喇叭声音变小
//					//如果两个开关都闭合，点击电池，电池变半透明，灯亮度减弱，喇叭声音变小；
//					for (int i = 0; i < switchList.Count; i++) 
//					{
//						if (!batteryList [i].GetComponent<SwitchCtrl> ().isSwitchOn) 
//						{
//
//							//如果开关控制的是灯泡   to do...
//							transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
//							//如果开关控制的是喇叭  to do...
//							transform.Find ("loudspeaker").GetComponent<AudioSource> ().volume = 0.5f;
//						} 
//						else 
//						{
//							//如果开关控制的是灯泡   to do...
//							transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
//							//如果开关控制的是喇叭  to do...
//							transform.Find ("loudspeaker").GetComponent<AudioSource> ().volume = 0;
//						}
//					}
//
//				} 
//				else//如果电池是正常状态且是半透明之后被点击后转成正常状态------2个电池工作
//				{
//					if (clickBattery.GetComponent<BatteryCtrl> ().clickCount>1)
//					{
//						for (int i = 0; i < switchList.Count; i++) 
//						{
//							if (!batteryList[i].GetComponent<SwitchCtrl>().isSwitchOn) 
//							{
//
//								//如果开关控制的是灯泡   to do...
//								transform.Find("bulb").GetComponent<UISprite>().spriteName="bulbSpark";
//								//如果开关控制的是喇叭  to do...
//								transform.Find("loudspeaker").GetComponent<AudioSource>().volume=1f;
//							}
//							else 
//							{
//								//如果开关控制的是灯泡   to do...
//								transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
//								//如果开关控制的是喇叭  to do...
//								transform.Find ("loudspeaker").GetComponent<AudioSource> ().volume = 0;
//							}
//						}
//					}
//				}
//			}	

		}
	}






}
