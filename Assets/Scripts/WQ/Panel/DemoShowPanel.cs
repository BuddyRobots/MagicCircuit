using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoShowPanel : MonoBehaviour 
{

	public static DemoShowPanel _instance;
	private GameObject preBtn;
	private GameObject nextBtn;
	private GameObject confirmBtn;
	private UITexture currDemoTex;

	List<string> levelDemoPics=new List<string>();
	List<Texture> levelDemoTex=new List<Texture>();
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
		currDemoTex=transform.Find("DemoPic").GetComponent<UITexture>();

		preBtn=transform.Find("PreBtn").GetComponent<UISprite>().gameObject;
		nextBtn=transform.Find("NextBtn").GetComponent<UISprite>().gameObject;
		confirmBtn=transform.Find("ConfirmBtn").GetComponent<UISprite>().gameObject;

		UIEventListener.Get (preBtn).onClick = OnPreBtnClick;
		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;
		UIEventListener.Get (confirmBtn).onClick = OnConfirmBtnClick;
	}
		

	void OnPreBtnClick(GameObject btn)
	{
		transform.Find("DemoPic").GetComponent<HelpDataShow>().Back();
	}

	void OnNextBtnClick(GameObject btn)
	{

		transform.Find("DemoPic").GetComponent<HelpDataShow>().Next();
	}

	void OnConfirmBtnClick(GameObject btn)
	{
		int flag= PlayerPrefs.GetInt("toDemoPanelFromPanel");
		switch (flag) 
		{
		case (int)FromPanelFlag.START://如果是从开始界面进来的帮助界面
			if (PlayerPrefs.HasKey("toDemoPanelFromBtn") && PlayerPrefs.GetInt("toDemoPanelFromBtn")==2) //从帮助按钮进来的
			{
				PanelTranslate.Instance.GetPanel(Panels.StartPanel);
			}
			else if ( PlayerPrefs.GetInt("toDemoPanelFromBtn")==1 ) //从开始按钮进来的
			{
				PanelTranslate.Instance.GetPanel(Panels.LevelSelectedPanel);
			}
			break;
		case (int)FromPanelFlag.LEVELSELECT:
			PanelTranslate.Instance.GetPanel(Panels.LevelSelectedPanel);
			break;
		case (int)FromPanelFlag.PHOTOTAKING:
			PanelTranslate.Instance.GetPanel(Panels.PhotoTakingPanel);
			break;
		case (int)FromPanelFlag.PHOTORECOGNIZE:
			Destroy(gameObject);
			break;
		default:
			break;
		}
		PanelTranslate.Instance.DestoryThisPanel();

	}





}
