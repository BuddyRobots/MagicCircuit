using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;
//level 6
public class ParallelCircuitsWithTwoSwitch : MonoBehaviour 
{
	[HideInInspector]
	public bool isParrallelCircuit = false;
	private List<GameObject> switchList = null; 
	private List<GameObject> bulbList = null;

	private int animationPlayedTimes=0;

	void OnEnable ()
	{
		isParrallelCircuit = false;
		animationPlayedTimes=0;
	}
	

	void Update () 
	{
		if (isParrallelCircuit) 
		{
			switchList = PhotoRecognizingPanel._instance.switchList;
			bulbList = PhotoRecognizingPanel._instance.bulbList;
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
				for (int i = 0; i < switchList.Count; i++) 
				{
					for (int j = 0; j < bulbList.Count; j++) 
					{

						if (switchList[i].tag==bulbList[j].tag && !switchList[i].GetComponent<SwitchCtrl>().isSwitchOn) //开关和元件配对,并且开关闭合
						{
							
							//调用接口，返回值为list<CircuitItem>
							//CurrentFlow cf = new CurrentFlow ();
							//cf.switchOnOff(switchList[i].tag, true);
							//获得新的List<CircuitItem> circuitItems;判断




							//灯亮，走电流
							switchList[i].GetComponent<UISprite>().spriteName="bulbOn";
							GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);//走电流
							GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;//标记已经播放电流
							animationPlayedTimes++;
						}

					}
					
				}

			}
			 
			if (animationPlayedTimes>0) 
			{

				for (int i = 0; i < switchList.Count; i++) 
				{
					for (int j = 0; j < bulbList.Count; j++) 
					{

//						if (switchList[i].tag==bulbList[j].tag && switchList[i].GetComponent<SwitchCtrl>().isSwitchOn) //开关和元件配对,并且开关断开
//						{
//							switchList[i].GetComponent<UISprite>().spriteName="bulbOff";
//							//该分支的电流消失  to do...
//						}
//						else if (switchList[i].tag==bulbList[j].tag && !switchList[i].GetComponent<SwitchCtrl>().isSwitchOn) 
//						{
//							switchList[i].GetComponent<UISprite>().spriteName="bulbOn";
//							//该分支的电流流通  to do...
//						}
						if (switchList[i].tag==bulbList[j].tag) //开关和元件配对
						{
							if (switchList[i].GetComponent<SwitchCtrl>().isSwitchOn) //开关断开
							{
								switchList[i].GetComponent<UISprite>().spriteName="bulbOff";
								//switchOnOff(int ID, bool state);//调用接口，返回值为list<CircuitItem>
								//返回的值是一个list，每个CircuitItem上有个powered值，这里遍历所有线的powered值，为true的话走电流，false的不走电流
							}
							else//开关闭合
							{
								switchList[i].GetComponent<UISprite>().spriteName="bulbOn";
								//该分支的电流流通  to do...

							}
						}
					}
				}

			}
		}
	}

}
