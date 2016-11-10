using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class DemoShowPanel : MonoBehaviour 
{

	public static DemoShowPanel _instance;
	private GameObject preBtn;
	private GameObject nextBtn;
	private GameObject confirmBtn;

//	private UISprite currDemoSprite;
	private UITexture currDemoTex;

	List<string> levelDemoPics=new List<string>();
	List<int> levelDemoIndex=null;



	private enum  FromPanelFlag
	{
		START=1,
		LEVELSELECT,
		PHOTOTAKING,
		PHOTORECOGNIZE

	}



	void Awake()
	{
		_instance = this;
//		currDemoSprite=transform.Find("DemoPic").GetComponent<UISprite>();
		currDemoTex=transform.Find("DemoPic").GetComponent<UITexture>();

		preBtn=transform.Find("PreBtn").GetComponent<UISprite>().gameObject;
		nextBtn=transform.Find("NextBtn").GetComponent<UISprite>().gameObject;
		confirmBtn=transform.Find("ConfirmBtn").GetComponent<UISprite>().gameObject;


		UIEventListener.Get (preBtn).onClick = OnPreBtnClick;
		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;
		UIEventListener.Get (confirmBtn).onClick = OnConfirmBtnClick;

		int flag = PlayerPrefs.GetInt("toDemoPanelFromPanel");
		//改成switch，条件用枚举---//也可以用全局静态变量来存储并判断
		if(flag == 1|| flag == 2)//如果是从开始界面或者选关界面进入的帮助界面
		{
			
			DemoDataManager.Instance.mDicDemoLevelPic.TryGetValue(0,out levelDemoPics);
			if (levelDemoPics!=null && levelDemoPics.Count>0) 
			{
			//	currDemoSprite.spriteName=levelDemoPics[0];
				currDemoTex.mainTexture.name=levelDemoPics[0];

			}


		} 
		else if(flag == 3|| flag==4)//如果是从拍摄界面或者识别界面进入的帮助界面
		{

			int levelNum = LevelManager.currentLevelData.LevelID;

			DemoDataManager.Instance.mDicDemoLevelPic.TryGetValue(levelNum,out levelDemoPics);


			if (levelDemoPics!=null && levelDemoPics.Count>0) 
			{
//				currDemoSprite.spriteName=levelDemoPics[0];
				currDemoTex.gameObject.GetComponent<UITexture>().name=levelDemoPics[0];
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
				if (currDemoTex.gameObject.GetComponent<UITexture>().name==levelDemoPics[0]) //如果当前图片就是demo图片集中的第一张，点击左键没反应
				{
					return;
				}
				else
				{
					//需要记录当前图片的名字
					for (int i = 1; i < levelDemoPics.Count; i++) 
					{
						if (currDemoTex.gameObject.GetComponent<UITexture>().name==levelDemoPics[i]) 
						{

							currDemoTex.gameObject.GetComponent<UITexture>().name=levelDemoPics[i-1];
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
				if (currDemoTex.gameObject.GetComponent<UITexture>().name==levelDemoPics[levelDemoPics.Count-1]) //如果当前图片就是demo图片集中的最后一张，点击右键没反应
				{
					return;
				}
				else
				{
					//需要记录当前图片的名字
					for (int i = 0; i < levelDemoPics.Count-1; i++) 
					{
						if (currDemoTex.gameObject.GetComponent<UITexture>().name==levelDemoPics[i]) 
						{
							currDemoTex.gameObject.GetComponent<UITexture>().name=levelDemoPics[i+1];
						}
					}

				}

			}

		}
		
	}




	void OnConfirmBtnClick(GameObject btn)
	{
		
		int flag= PlayerPrefs.GetInt("toDemoPanelFromPanel");
		switch (flag) 
		{

		case 1://FromPanelFlag.START://如果是从开始界面进来的帮助界面
			if (PlayerPrefs.HasKey("toDemoPanelFromBtn") && PlayerPrefs.GetInt("toDemoPanelFromBtn")==2) //从帮助按钮进来的
			{
				PanelTranslate.Instance.GetPanel(Panels.StartPanel);
			}
			else if ( PlayerPrefs.GetInt("toDemoPanelFromBtn")==1 ) //从开始按钮进来的
			{
				PanelTranslate.Instance.GetPanel(Panels.LevelSelectedPanel);

			}

			break;
		case 2://FromPanelFlag.LEVELSELECT:
			PanelTranslate.Instance.GetPanel(Panels.LevelSelectedPanel);
			break;
		case 3://FromPanelFlag.PHOTOTAKING:
			PanelTranslate.Instance.GetPanel(Panels.PhotoTakingPanel);
			break;
		case 4://FromPanelFlag.PHOTORECOGNIZE:
//			PanelTranslate.Instance.GetPanel(Panels.PhotoRecognizedPanel);
//			PanelTranslate.Instance.GetPanel(Panels.None);
			Destroy(gameObject);
			break;
		default:
			break;


		}
		PanelTranslate.Instance.DestoryThisPanel();

	}


	public void PanelOff()
	{
		gameObject.SetActive (false);
	}
}
