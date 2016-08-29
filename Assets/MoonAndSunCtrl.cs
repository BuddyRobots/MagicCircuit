using UnityEngine;
using System.Collections;

public class MoonAndSunCtrl : MonoBehaviour {


	private bool isLightOn = true;
	private GameObject sunBtn;
	private GameObject moonBtn;

	void Start () 
	{
	
		sunBtn = transform.Find ("SunBtn").gameObject;
		moonBtn = transform.Find ("MoonBtn").gameObject;

		UIEventListener.Get (sunBtn).onClick = OnSunBtnClick;
		UIEventListener.Get (moonBtn).onClick = OnMoonBtnClick;

	}
	

	void Update () 
	{
	
	}

	void OnSunBtnClick()
	{


	}

	void OnMoonBtnClick()
	{


	}
}
