using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoShowPanel : MonoBehaviour 
{

	public static DemoShowPanel _instance;
	private GameObject preBtn;
	private GameObject nextBtn;

	private UISprite currDemoSprite;

	List<string> levelDemoPics=new List<string>();
	List<int> levelDemoIndex=null;

	void Awake()
	{
		_instance = this;
		currDemoSprite=transform.Find("DemoPic").GetComponent<UISprite>();

		preBtn=transform.Find("PreBtn").GetComponent<UISprite>().gameObject;
		nextBtn=transform.Find("NextBtn").GetComponent<UISprite>().gameObject;



		UIEventListener.Get (preBtn).onClick = OnPreBtnClick;
		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;


		int flag = PlayerPrefs.GetInt("toDemoPanelFromPanel");
		//改成switch，条件用枚举---//也可以用全局静态变量来存储并判断
		if(flag == 1)
		{
			return;

		} else if(flag == 2)
		{

			int levelNum = LevelManager.currentLevelData.LevelID;

			DemoDataManager.Instance.mDicDemoLevelPic.TryGetValue(levelNum,out levelDemoPics);


			if (levelDemoPics!=null && levelDemoPics.Count>0) 
			{
				currDemoSprite.spriteName=levelDemoPics[0];
			}

		}



	}

	void OnEnable()
	{
		HomeBtn.Instance.panelOff = PanelOff;
	}	

	void OnPreBtnClick(GameObject btn)
	{
		if (levelDemoPics!=null) 
		{

			if (levelDemoPics.Count<=1) //如果这个关卡只有一张demo图片，点击按钮没反应
			{
				return;
			}
			else//如果这个关卡不止一张demo图片
			{
				if (currDemoSprite.spriteName==levelDemoPics[0]) //如果当前图片就是demo图片集中的第一张，点击左键没反应
				{
					return;
				}
				else
				{
					//需要记录当前图片的名字
					for (int i = 1; i < levelDemoPics.Count; i++) 
					{
						if (currDemoSprite.spriteName==levelDemoPics[i]) 
						{

							currDemoSprite.spriteName=levelDemoPics[i-1];
						}
					}

				}

			}
			
		}


		
		
	}

	void OnNextBtnClick(GameObject btn)
	{

		if (levelDemoPics!=null) 
		{

			if (levelDemoPics.Count<=1) //如果这个关卡只有一张demo图片，点击按钮没反应
			{
				return;
			}
			else//如果这个关卡不止一张demo图片
			{
				if (currDemoSprite.spriteName==levelDemoPics[levelDemoPics.Count-1]) //如果当前图片就是demo图片集中的最后一张，点击右键没反应
				{
					return;
				}
				else
				{
					//需要记录当前图片的名字
					for (int i = 0; i < levelDemoPics.Count-1; i++) 
					{
						if (currDemoSprite.spriteName==levelDemoPics[i]) 
						{

							currDemoSprite.spriteName=levelDemoPics[i+1];
						}
					}

				}

			}

		}
		
	}

	public void PanelOff()
	{
		gameObject.SetActive (false);
	}
}
