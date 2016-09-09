using UnityEngine;
using System.Collections;

public class SPDTswitchCtrl : MonoBehaviour 
{

	[HideInInspector]
	public bool isRightOn = true;
	private UISprite SPDTsprite;


	void Start () 
	{
		SPDTsprite = GetComponent<UISprite> ();
	
	}

	void Update () 
	{
		if (isRightOn) 
		{
			SPDTsprite.spriteName = "SPDTRight";
		}
		if (!isRightOn) 
		{
			SPDTsprite.spriteName = "SPDTLeft";
		
		}
	}

	void OnClick()
	{


		isRightOn = !isRightOn;
	}
}
