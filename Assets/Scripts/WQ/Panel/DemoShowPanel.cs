using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoShowPanel : MonoBehaviour 
{

	public static DemoShowPanel _instance;
	private GameObject preBtn;
	private GameObject nextBtn;
	private GameObject confirmBtn;
	private UISprite nextBtnSprite;
	private UISprite preBtnSprite;

	private enum  FromPanelFlag
	{
		START=1,
		LEVELSELECT,
		PHOTOTAKING,
		PHOTORECOGNIZE

	}
	private bool isPreBtnEfective=true;
	private bool isNextBtnEffective=true;


	void Awake()
	{
		_instance = this;

		preBtn=transform.Find("PreBtn").GetComponent<UISprite>().gameObject;
		nextBtn=transform.Find("NextBtn").GetComponent<UISprite>().gameObject;
		confirmBtn=transform.Find("ConfirmBtn").GetComponent<UISprite>().gameObject;

		isPreBtnEfective=true;
		isNextBtnEffective=true;
			
		UIEventListener.Get (preBtn).onClick = OnPreBtnClick;
		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;
		UIEventListener.Get (confirmBtn).onClick = OnConfirmBtnClick;
	}
	void Start()
	{


		nextBtnSprite=nextBtn.GetComponent<UISprite>();
		preBtnSprite=preBtn.GetComponent<UISprite>();
	}

	void Update()
	{

		if (!isNextBtnEffective) 
		{
			if (nextBtnSprite.spriteName!="nextBtnDark") 
			{
				nextBtnSprite.spriteName="nextBtnDark";
			}
		}
		else
		{
			if (nextBtnSprite.spriteName!="nextBtnNormal") 
			{
				nextBtnSprite.spriteName="nextBtnNormal";
			}
		}
		if (!isPreBtnEfective) 
		{
			if (preBtnSprite.spriteName!="PreBtnDark") {
				preBtnSprite.spriteName="PreBtnDark";
			}

		}
		else
		{
			if (preBtnSprite.spriteName!="PreBtnNormal") {
				preBtnSprite.spriteName="PreBtnNormal";
			}

		}

	}


	void OnPreBtnClick(GameObject btn)
	{
		if (!transform.Find("DemoPic").GetComponent<HelpDataShow>().Back()) 
		{
			//如果上一张按钮不能点了，
			preBtnSprite.spriteName="PreBtnDark";
			isPreBtnEfective=false;
			isNextBtnEffective=true;
		}
		else
		{
			isPreBtnEfective=true;
			isNextBtnEffective=true;
		}
	}

	void OnNextBtnClick(GameObject btn)
	{
		if (!transform.Find("DemoPic").GetComponent<HelpDataShow>().Next())
		{
			nextBtnSprite.spriteName="nextBtnDark";
			isNextBtnEffective=false;
			isPreBtnEfective=true;
		}
		else
		{
			isNextBtnEffective=true;
			isPreBtnEfective=true;
		}
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
