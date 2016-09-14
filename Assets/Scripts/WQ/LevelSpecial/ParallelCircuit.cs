using UnityEngine;
using System.Collections;

public class ParallelCircuit : MonoBehaviour {



	[HideInInspector]
	public bool isParallelCircuit = false;



	void OnEnable ()
	{

		isParallelCircuit = false;
	}
	

	void Update () 
	{
		if (isParallelCircuit) {
			if (!PhotoRecognizingPanel._instance.isArrowShowDone)
			{
			
				//两个开关，一个开关控制灯，一个开关控制喇叭
				//如果控制灯的开关闭合，电流接通，灯亮
				//如果控制喇叭的开关闭合，电流接通，喇叭响，走电流
				//
				//
				//
			}
		}
	}
}
