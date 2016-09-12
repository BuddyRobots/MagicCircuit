using UnityEngine;
using System.Collections;

public class ParallelCircuitWithTwoBattery : MonoBehaviour 
{


	public bool isParallelCircuitWithTwoBattery=false;

	//第7关，两节电池 + 开关 + 灯泡    	  —灯泡更亮
	//两节电池+ 开关 + 小喇叭    	 —喇叭更响
	//电流播放一会后，在电池的位置出现小手提示电池是可以点击的，（两个电池要确保有一个是在工作的，只能有一个能被点击变成半透明），
	//点击电池后，小手消失，电池变成半透明，电池的位置出现线连接原来的线的端点，电流也要走到半透明的电池之上，灯泡亮度变暗一点，喇叭声音变小；
	//电流继续播放两三秒后，在透明电池上出现小手，再点击透明电池的时候，小手消失，电池还原，电池位置的线消失，电池上面的电流也消失，灯泡亮度变更亮，喇叭声音变大；

	// Use this for initialization
	void OnEnable () 
	{
		isParallelCircuitWithTwoBattery=false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isParallelCircuitWithTwoBattery) 
		{
			if (!PhotoRecognizingPanel._instance.isArrowShowDone) 
			{


				//如果控制灯泡的开关闭合，灯泡亮，走电流
				//如果控制小喇叭的开关闭合，小喇叭响，走电流
				
			}
			
		}
	
	}
}
