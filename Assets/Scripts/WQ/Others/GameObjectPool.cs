using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameObjectPool: MonoBehaviour 
{
	private static GameObjectPool instance;
	public static GameObjectPool Instance
	{
		get
		{
			return GameObjectPool.instance;
		}
		set
		{
			GameObjectPool.instance=value;
		}

	}
	private  static Dictionary<string,ArrayList> pool=new Dictionary<string, ArrayList>{};

	void Start () 
	{
		instance=this;
	}
	
	public static Object GetFromPool(string prefabName)
	{
		string key=prefabName+"(Clone)";
		Object o;
		//池中存在，则从池中取出
		if (pool.ContainsKey(key) && pool[key].Count>0) 
		{
			ArrayList list=pool[key];
			do {
				if(list.Count <= 0)
				{
					o = GetFromPool(prefabName);
				}
				else
				{
					o=list[0] as Object;
					list.Remove(o);

				}
			} while (o==null);

			//重新初始化相关状态
			(o as GameObject).SetActive(true);
		}
		//池中没有则实例化gameobejct
		else
		{
			o=Instantiate(Resources.Load(prefabName));
			(o as GameObject).name = key;
		}
		return o;
	}

	/// <summary>
	/// put the unused object  back to pool
	/// </summary>
	public static Object ReturnToPool(GameObject o)
	{
		string key=o.name;

		if (pool.ContainsKey(key)) 
		{
			ArrayList list=pool[key];
			list.Add(o);
		}
		else
		{
			pool[key]=new ArrayList(){o};
		}
		o.SetActive(false);
		return o;
	}

}
