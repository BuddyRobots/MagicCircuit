using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HourGlassPlayAni : MonoBehaviour 
{

	public bool ActivateWait = true ;

	float fireRate = 0.1f;
	int index = 0;
	float nextFire=0;
	// player0001  -- 0023
	private List<string> spriteNameList=new List<string>();



	void Start()
	{



		for (int i = 0; i < 24; i++) 
		{
			string temp = null;
			if (i < 10) 
			{
				temp = "player000" + i.ToString ();
			} 
			else 
			{
				temp = "player00" + i.ToString ();
			}

			spriteNameList.Add (temp);
		}
		//		foreach (var item in spriteNameList) 
		//		{
		//			Debug.Log ("item=====" + item);
		//		}

	}




	void Update()
	{
		if (index < spriteNameList.Count)
		{
			if (Time.time > nextFire)
			{
				nextFire = Time.time + fireRate;
				this.GetComponent<UISprite>().spriteName= spriteNameList[index];
				index++;
			}
		}
		else
		{
			index = 0;
		}

	}

}



