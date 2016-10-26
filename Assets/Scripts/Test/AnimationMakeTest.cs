using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationMakeTest : MonoBehaviour
{
	float fireRate = 0.1f;
	/// <summary>
	/// 图片的下标，默认为0，最大为 图片数量-1
	/// </summary>
	int index = 0;

	float nextFire=0;
	// player0001  -- 0023
	private List<string> spriteNameList=new List<string>();
	[HideInInspector]
	public  bool isPlayAni=false;

	private UIAtlas atlas_pre;
	private UIAtlas atlas_ani;
	//public UIAtlas atlas;
	public string atlasPath = "";
	public string sname = "player00";
	public int spriteNum = 0;
	public float speed = 1;
	public bool isLoop = true;
	public List<float> time;

	private int currentSpriteID = 0;

	void Start()
	{
		atlas_pre = transform.GetComponent<UISprite> ().atlas;
		atlas_ani = Resources.Load<GameObject> (atlasPath).GetComponent<UIAtlas>();
	}


	void Update()
	{
		if (isPlayAni) 
		{
			atlas_pre = atlas_ani;
			for (int i = 0; i < spriteNum; i++) 
			{
				string currentSpriteName = sname + i;
			}

			if (isLoop )//如果循环播放
			{
				if (Time.time > time[index])
				{
					nextFire = Time.time + fireRate;
					this.GetComponent<UISprite>().spriteName= spriteNameList[index % spriteNum];
					index++;
				}
			} 
			else if(index < spriteNum)//如果不循环播放
			{
				if (Time.time > nextFire)
				{
					nextFire = Time.time + fireRate;
					this.GetComponent<UISprite>().spriteName= spriteNameList[index];
					index++;
				}
			}

		}
	}
}
