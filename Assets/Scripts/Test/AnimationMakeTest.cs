using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationMakeTest : MonoBehaviour
{
	public bool ActivateWait = true ;

	float fireRate = 0.2f;
	int i = 0;
	float nextFire=0;
	// player0001  -- 0023
	private List<string> spriteNameList=new List<string>();

	string[] ActivatorTexture = new string[] { "player0001", "player0002", "player0003", "player0004", "player0005", "player0006",
		"player0007", "player0008", "player0009", "player0010", "player0011", "player0012" };      

	void Awake()
	{
		//this.GetComponent<UISprite>().enabled = false;
	}

	// Use this for initialization
	void Start()
	{
		for (int i = 0; i < 24; i++) {
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

	}

	// Update is called once per frame
	void Update()
	{
		if (ActivateWait)
		{
			
			if (i < spriteNameList.Count)
			{
				if (Time.time > nextFire)
				{
					nextFire = Time.time + fireRate;
					this.GetComponent<UISprite>().spriteName= spriteNameList[i];
					i++;
				}
			}
			else
			{
				i = 0;
			}
		}
		else
		{
			//this.GetComponent<UISprite>().enabled = false;
		}
	}


}
