using UnityEngine;
using System.Collections;

public class BulbCtrl : MonoBehaviour 
{

	[HideInInspector]
	public bool isSemiTrans = false;

	void Update () 
	{
		
		if (isSemiTrans) 
		{//如果是半透明状态
			gameObject.GetComponent<UISprite>().spriteName="semiTransBulb";

		} 
		else//如果是正常状态 
		{
			gameObject.GetComponent<UISprite>().spriteName="bulbOn";
		}
	}

	void OnClick()
	{
		isSemiTrans = !isSemiTrans;
	}
}
