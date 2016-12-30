using UnityEngine;
using System.Collections;
//level 1
public class LevelOne : MonoBehaviour 
{
	/// <summary>
	///执行该脚本中update函数中的方法的标志
	/// </summary>
	[HideInInspector]
	public bool isPlayCircuitAnimation=false;

	void Update () 
	{
		if (isPlayCircuitAnimation)
		{
			OpenCircuit ();

		}
	}




	public void OpenCircuit()
	{
		//		PhotoRecognizingPanel.Instance.transValue = 1;
		//		for (int i = 0; i < PhotoRecognizingPanel.Instance.arrowList.Count; i++) 
		//		{
		//			if (PhotoRecognizingPanel.Instance.arrowList [i]) 
		//			{
		//				//PhotoRecognizingPanel._instance.arrowList [i].GetComponent<ArrowCtrl> ().speed *=2;
		//				PhotoRecognizingPanel.Instance.arrowList [i].GetComponent<UISprite> ().alpha = 1;//显示电流
		//			}
		//
		//		}
		CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
		if (transform.Find ("bulb").GetComponent<UISprite> ().spriteName != "bulbOn") 
		{
			transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";//灯亮 
		}
			
	}

}
