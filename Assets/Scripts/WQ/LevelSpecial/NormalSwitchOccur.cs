using UnityEngine;
using System.Collections;

public class NormalSwitchOccur : MonoBehaviour {


	[HideInInspector]
	public bool isNormalSwitchOccur = false;

	private int animationPlayedTimes=0;

	void OnEnable()
	{
		animationPlayedTimes=0;

	}

	void Update () 
	{
		if (isNormalSwitchOccur) 
		{
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
				//在开关位置出现小手，点击开关小手消失，开关闭合，,灯泡变亮，走电流，
				//点击开关断开，电流消失，灯泡变暗
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(transform.Find("switch").localPosition);//出现小手

				if(transform.Find("switch").GetComponent<SwitchCtrl>().isSwitchOn==false)//开关闭合
				{
					//开关闭合，销毁小手，灯泡变亮，走电流
					Destroy (PhotoRecognizingPanel._instance.finger);
					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulb-on";
					GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);
					GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;
					animationPlayedTimes=1;//电流播放一次
				}	
			}

			if (animationPlayedTimes==1) //如果在播放电流的时候点击开关断开
			{
				if (transform.Find ("switch").GetComponent<SwitchCtrl> ().isSwitchOn)
				{
					Debug.Log ("is switchOn");
					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbDark";
					//GetComponent<PhotoRecognizingPanel> ().StopCreateArrows();
					foreach (GameObject item in GetComponent<PhotoRecognizingPanel> ().arrowList)
					{
						//电流应该停止走动，并隐藏，而不是销毁..to do ..
						//item.SetActive(false);
						Destroy(item);

					}

				}
				else 
				{
					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulb-on";

				}

			}

		}
	}
}
