using UnityEngine;
using System.Collections;

public class BulbCtrl : MonoBehaviour 
{

	[HideInInspector]
	public bool isSemiTrans = false;

	private UISprite bulbSprite;

	void OnEnable()
	{
		bulbSprite=this.gameObject.GetComponent<UISprite>();

	}

	void OnClick()
	{
		isSemiTrans = !isSemiTrans;
	}

}
