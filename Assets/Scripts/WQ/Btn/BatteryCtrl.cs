using UnityEngine;
using System.Collections;

public class BatteryCtrl : MonoBehaviour 
{

	[HideInInspector]
	public bool isSemiTrans = false;
	private UISprite batterySprite;

	[HideInInspector]
	public int clickCount = 0;

	void Start()
	{

		batterySprite = GetComponent<UISprite> ();
	}

	void OnEnable()
	{
		clickCount = 0;
	}

	

	void Update () 
	{
		if (isSemiTrans) //如果是半透明状态
		{
			batterySprite.spriteName="semiTransBattery";


		} 
		else//如果是正常状态 
		{
			batterySprite.spriteName="battery";
		
		}
	}
		
	void OnClick()
	{
		
		isSemiTrans = !isSemiTrans;
		clickCount++;
		//点奇数次的时候是透明，点偶数次的时候是 不透明

	}
}
