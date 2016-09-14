using UnityEngine;
using System.Collections;


// level 9
public class ParallelCircuitWithTwoBulb : MonoBehaviour 
{
	public bool isParallelCircuitWithTwoBulb = false;

	// Use this for initialization
	void OnEnable ()
	{
		isParallelCircuitWithTwoBulb = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isParallelCircuitWithTwoBulb) 
		{
			if (!PhotoRecognizingPanel._instance.isArrowShowDone) 
			{



				//





				//两个灯泡，灯泡可以点击其中一个
				//电流播放一会后，在灯泡的位置出现小手提示电池是可以点击的，（两个灯泡要确保有一个是在工作的，只能有一个能被点击变成半透明），
				//点击灯泡后，小手消失，灯泡变成半透明，灯泡的位置出现线连接原来的线的端点，电流也要走到半透明的灯泡之上，灯泡亮度变暗一点，喇叭声音变小；
				//电流继续播放两三秒后，在透明灯泡上出现小手，再点击透明灯泡的时候，小手消失，灯泡还原，灯泡位置的线消失，灯泡上面的电流也消失，灯泡亮度变更亮，喇叭声音变大；











			}
			
		}
	
	}
}
