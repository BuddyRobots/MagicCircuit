using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// level 2
public class RemoveLine : MonoBehaviour 
{
	[HideInInspector]
	public bool isRemoveLine=false;

	/// <summary>
	/// 有没有线段被擦除的标志
	/// </summary>
	private bool isLineRemove = false;

	void OnEnable()
	{
		isRemoveLine=false;
	}



	void Update () 
	{
		if (isRemoveLine) 
		{
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
				CommonFuncManager._instance.OpenCircuit ();	

			}
			Vector3	randomPos = GetComponent<PhotoRecognizingPanel> ().ChooseRandomPoint ();
			TouchToDestroyLine ();
			if (!isLineRemove) 
			{
				Debug.Log ("fingershow  before  isLineRemove value===" + isLineRemove);
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(randomPos);//动画播放3秒后，在电线上的任意随机点位置出现小手
			}
			//TouchToDestroyLine ();

		}
	}




	//private bool isLineRemove = false;


	/// <summary>
	/// 触摸移动销毁线条
	/// </summary>
	void TouchToDestroyLine()
	{
		#if UNITY_EDITOR 
		if (Input.GetMouseButtonDown(0)) 
		{
			Ray ray = transform.parent.Find ("Camera").GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{
				GameObject go = hit.collider.gameObject;
				if (go.name.Contains ("line")) 
				{ //如果碰到的是线，线就消失，电流消失
					Destroy (go);
					isLineRemove=true;
					Debug.Log("islineRemove value==="+isLineRemove);
					Destroy (GetComponent<PhotoRecognizingPanel> ().finger);
					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
					GetComponent<PhotoRecognizingPanel> ().StopCreateArrows();
					foreach (GameObject item in GetComponent<PhotoRecognizingPanel> ().arrowList)
					{
						Destroy (item);
					}
				}
			}
		}
		#elif UNITY_IPHONE 
		if (Input.touchCount>0 && Input.GetTouch (0).phase == TouchPhase.Moved) //如果有移动触摸
		{
		Ray ray = transform.parent.Find ("Camera").GetComponent<Camera> ().ScreenPointToRay (Input.GetTouch(0).position);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) 
		{
		GameObject go = hit.collider.gameObject;
		if (go.name.Contains ("line")) 
		{ 
		Destroy (go);
		Destroy (GetComponent<PhotoRecognizingPanel> ().finger);
		transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
		GetComponent<PhotoRecognizingPanel> ().StopCreateArrows();
		foreach (GameObject item in GetComponent<PhotoRecognizingPanel> ().arrowList)
		{
		Destroy (item);
		}
		}
		}

		}
		#endif

	}
}
