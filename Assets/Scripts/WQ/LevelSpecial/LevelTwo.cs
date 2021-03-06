﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;
// level 2
public class LevelTwo : MonoBehaviour 
{
	public static LevelTwo _instance;
	[HideInInspector]
	public bool isRemoveLine=false;

	[Range(0,2)]
	public float radius;

	[HideInInspector]
	public bool CanRemoveLine=false;

	private bool isCircuitPowered_cur;
	private Vector3 randomPos;


	void Awake()
	{

		_instance=this;
	}
	void OnEnable()
	{
		CanRemoveLine=false;
		isRemoveLine=false;

		isCircuitPowered_cur=true;

		if (PhotoRecognizingPanel.Instance) 
		{
			PhotoRecognizingPanel.Instance.isNeedToCreateArrow = true;
		}
	}

	void Update () 
	{
		if (isRemoveLine) 
		{

			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);

			randomPos = CommonFuncManager._instance.ChooseMiddlePointOnLine (PhotoRecognizingPanel.Instance.lines);
			GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(randomPos);//动画播放3秒后，在电线上的任意随机点位置出现小手

			if (CanRemoveLine) 
			{
				TouchToDestroyLine ();
			}
			if (isCircuitPowered_cur) 
			{
				transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";

			}
			else
			{
				transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
			}
		}
	}

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
				if (hit.collider.gameObject.name=="MaskBg" || hit.collider.gameObject.name=="Btn") 
				{
					return ;	
				}
				else
				{
					DestroyLine(hit.point,Constant.DESTROYLINE_RADIUS/*radius*/);
				}
			}
		}
		#elif UNITY_IPHONE 
		if (Input.touchCount>0 /*&& Input.GetTouch (0).phase == TouchPhase.Moved*/) //如果有移动触摸
		{
			Ray ray = transform.parent.Find ("Camera").GetComponent<Camera> ().ScreenPointToRay (Input.GetTouch(0).position);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{
				if (hit.collider.gameObject.name=="MaskBg" || hit.collider.gameObject.name=="Btn") 
				{
					return ;	
				}
				else
				{
					DestroyLine(hit.point,0.03f);
				}
			}
		}
		#endif

	}



	void DestroyLine(Vector3 center, float radius)
	{
		Collider[] hitColliders=Physics.OverlapSphere(center,radius);
		for (int k = 0; k < hitColliders.Length; k++) 
		{
			GameObject tempGo=hitColliders[k].gameObject;
			if (tempGo.tag!="mask" && tempGo.name.Contains("lineNew"))
			{
				int index=int.Parse(tempGo.tag);
				//擦除线是应该判断线有没有电流，如果有电流则消除线并且断开电流，如果没有则只消除线,不用断开电流
				if (GetImage._instance.itemList[index].powered) 
				{
					Destroy(hitColliders[k].gameObject);
					BreakCircuit();
					isCircuitPowered_cur=false;
				}
				else
				{
					Destroy(hitColliders[k].gameObject);
				}
			}
		}
	}
		

	void BreakCircuit()
	{
		isCircuitPowered_cur=false;

		PhotoRecognizingPanel temp = GetComponent<PhotoRecognizingPanel> ();
		if (temp.finger)
		{
			Destroy (temp.finger);
		}
//		transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
		temp.StopCreateArrows();
		for (int i = 0; i < temp.arrowList.Count; i++) 
		{
			Destroy (temp.arrowList[i]);
		}
	}


}
