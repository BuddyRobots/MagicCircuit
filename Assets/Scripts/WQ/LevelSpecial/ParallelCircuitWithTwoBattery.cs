using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallelCircuitWithTwoBattery : MonoBehaviour 
{


	public bool isParallelCircuitWithTwoBattery=false;
	private int animationPlayedTimes=0;
	private List<GameObject> batteryList = null;
	GameObject randomBattery =null;
	//第7关，两节电池 + 开关 + 灯泡    	  —灯泡更亮
	//两节电池+ 开关 + 小喇叭    	 —喇叭更响
	//电流播放一会后，在电池的位置出现小手提示电池是可以点击的，（两个电池要确保有一个是在工作的，只能有一个能被点击变成半透明），
	//点击电池后，小手消失，电池变成半透明，电池的位置出现线连接原来的线的端点，电流也要走到半透明的电池之上，灯泡亮度变暗一点，喇叭声音变小；
	//电流继续播放两三秒后，在透明电池上出现小手，再点击透明电池的时候，小手消失，电池还原，电池位置的线消失，电池上面的电流也消失，灯泡亮度变更亮，喇叭声音变大；

	// Use this for initialization
	void OnEnable () 
	{
		isParallelCircuitWithTwoBattery=false;
		animationPlayedTimes=0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isParallelCircuitWithTwoBattery) 
		{
			batteryList = PhotoRecognizingPanel._instance.batteryList;

			if (!PhotoRecognizingPanel._instance.isArrowShowDone) 
			{





				//如果控制灯泡的开关闭合，灯泡亮，走电流
				//如果控制小喇叭的开关闭合，小喇叭响，走电流

				//如果已经走了电流，在两个电池的随机一个位置出现小手，点击电池，电池变半透明，-----灯泡亮度减弱，喇叭声音变小
				//如果玩家3秒之内没有再次点击电池，则在半透明电池出现小手提示玩家点击，如果玩家在3秒之内点击了半透明电池，就不用出现小手

				animationPlayedTimes = 1;
			}
			if (animationPlayedTimes == 1) 
			{
				int randomIndex = Random.Range (0, 2);
				randomBattery = batteryList[randomIndex];
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(randomBattery.transform.localPosition);

				randomBattery.AddComponent<UIButton> ();
				randomBattery.AddComponent<BatteryCtrl> ();
			

				//如果两个开关都闭合，点击电池，电池变半透明，灯亮度减弱，喇叭声音变小；
				//如果两个开关断开，点击电池，电池变半透明
				//如果一个开关断开，一个开关闭合--------1-灯泡开关闭合，灯泡亮度变亮
				//								   2-喇叭开关闭合---喇叭声音变大


		
			}
			
		}
	
	}

}
