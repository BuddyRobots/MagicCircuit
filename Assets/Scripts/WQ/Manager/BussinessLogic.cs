﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

public class BussinessLogic 
{
	/// <summary>
	/// click the battery to change the item on UI
	/// </summary>
	/// <param name="circuititems">Circuititems.</param>
	/// <param name="isBatteryTrans">If set to <c>true</c> whether the battery is transparent or not.</param>
	public void BatteryClick(List<CircuitItem> circuititems, bool isBatteryTrans)
	{
		foreach (var item in circuititems) 
		{
			if (!item.powered)//如果元件没有电，则不做任何操作
			{
				//Debug.Log ("item [" + item.ID + "] has no power, no need to change");
				return;
			} 
			else //如果元件有电-----根据isBatteryTrans的值来做处理
			{
				if (isBatteryTrans) // the battery is transparent,that is, one battery works
				{ 
					item.power = CircuitItem.PowerStatus.E1;
				} 
				else // two batterys work
				{
					item.power = CircuitItem.PowerStatus.E2;
				}
				AllItemFlush ();
			}
		}

	}
	/// <summary>
	/// refresh all the items on UI
	/// </summary>
	public void AllItemFlush()
	{
		foreach (var item in CurrentFlow._instance.circuitItems) 
		{
			SingleItemFlush (item);
		}
	}
		
	public void SingleItemFlush(CircuitItem item)
	{
		Debug.Log ("SingleItemFlush");
		string tag = item.ID.ToString();//save the ID as tag of gameObject
		GameObject tempGo = GameObject.FindWithTag (tag);
		UISprite tempSprite = GameObject.FindWithTag (tag).GetComponent<UISprite> ();
		switch (item.type) 
		{
		case ItemType.Bulb:
			switch (item.power) 
			{
			case CircuitItem.PowerStatus.E0:
				tempSprite.spriteName="bulbOff";
				break;
			case CircuitItem.PowerStatus.E1:
				tempSprite.spriteName = "bulbOn";
				Debug.Log ("one battery works, bulb is on");
				break;
			case CircuitItem.PowerStatus.E2:
				tempSprite.spriteName="bulbSpark";
				Debug.Log ("two battery work, bulb is spark");
				break;
			default:
				break;
			}

			break;
		case ItemType.InductionCooker:
			Animation tempAni = tempGo.GetComponent<Animation> ();
			switch (item.power) 
			{
			case CircuitItem.PowerStatus.E0:  //如果在播放动画，则停止播放动画 
				if (tempAni.isPlaying)
				{
					tempAni.Stop ();
				}
				break;
			case CircuitItem.PowerStatus.E1://播放蒸汽动画1  
				tempAni.Play ("steam01");
				break;
			case CircuitItem.PowerStatus.E2://播放蒸汽动画2 
				tempAni.Play ("steam02");
				break;
			default:
				break;
			}
			break;
		case ItemType.Loudspeaker:
			AudioSource tempAudio = tempGo.GetComponent<AudioSource> ();
			switch (item.power) 
			{
			case CircuitItem.PowerStatus.E0:
				if (tempAudio.isPlaying) //如果在播放声音，则停止播放声音   to do..
				{
					tempAudio.Stop ();
				}
				break;
			case CircuitItem.PowerStatus.E1:
				if (!tempAudio.isPlaying) //如果没有播放声音，则播放声音   音量小  to do...
				{
					tempAudio.Play ();
				} 
				tempAudio.volume = 0.5f;

				break;
			case CircuitItem.PowerStatus.E2://如果没有播放声音，则播放声音   音量大  to do...
				if (!tempAudio.isPlaying) 
				{
					tempAudio.Play ();
				} 
				tempAudio.volume = 1f;
				break;
			default:
				break;
			}
			break;
		default:
			break;
		}	

	}














}