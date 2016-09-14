using UnityEngine;
using System.Collections;

public class BatteryCtrl : MonoBehaviour {





	[HideInInspector]
	public bool isSemiTrans = false;


	

	void Update () 
	{
		if (isSemiTrans) 
		{//如果是半透明状态
			gameObject.GetComponent<UISprite>().spriteName="semiTransBattery";

		} 
		else//如果是正常状态 
		{
			gameObject.GetComponent<UISprite>().spriteName="battery";
		
		}
	
	}




	void OnClick()
	{

		isSemiTrans = !isSemiTrans;

	}
}
