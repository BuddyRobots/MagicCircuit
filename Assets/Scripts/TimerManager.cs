using UnityEngine;
using System;
using System.Collections.Generic;
///

/// 移动管理

///

public class TimerManager
{

	public static float time;

	public static Dictionary<object,TimerItem> timerList = new Dictionary<object,TimerItem>();

	public static void Run()

	{
		// 设置时间值

		TimerManager.time = Time.time;

		TimerItem[] objectList = new TimerItem[timerList.Values.Count];

		timerList.Values.CopyTo(objectList, 0);

		// 锁定

		foreach(TimerItem timerItem in objectList)

		{

			if(timerItem != null) timerItem.Run(TimerManager.time);

		}

	}

	public static void Register(object objectItem, float delayTime, Action callback)

	{

		if(!timerList.ContainsKey(objectItem))

		{

		TimerItem timerItem = new TimerItem(TimerManager.time, delayTime, callback);

		timerList.Add(objectItem, timerItem);

		}

	}

	public static void UnRegister(object objectItem)

	{

		if(timerList.ContainsKey(objectItem))

		{

		timerList.Remove(objectItem);

		}

	}

}
