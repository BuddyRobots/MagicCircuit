using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DemoShowPanel : MonoBehaviour//SceneSinglton<DemoShowPanel> 
{
	public static DemoShowPanel Instance;
	private GameObject preBtn;
	private GameObject nextBtn;
	private GameObject confirmBtn;
	private HelpDataShow helpDataShow;

	private UISprite nextBtnSprite;
	private UISprite preBtnSprite;
	private int panelFlag;


	private bool isPreBtnEfective=true;
	private bool isNextBtnEffective=true;


	void Awake()
	{
		Instance = this;

		preBtn=transform.Find("PreBtn").GetComponent<UISprite>().gameObject;
		nextBtn=transform.Find("NextBtn").GetComponent<UISprite>().gameObject;
		confirmBtn=transform.Find("ConfirmBtn").GetComponent<UISprite>().gameObject;
		helpDataShow=transform.Find("DemoPic").GetComponent<HelpDataShow>();

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

		panelFlag= PlayerPrefs.GetInt("toDemoPanelFromPanel");

		if (panelFlag==(int)FromPanelFlag.START || panelFlag==(int)FromPanelFlag.LEVELSELECT) 
		{
			helpDataShow.InitFromStart();
		}
		else
		{
			helpDataShow.InitFromLevel();

		}
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
		switch (panelFlag) 
		{
		case (int)FromPanelFlag.START://如果是从开始界面进来的帮助界面
			if (PlayerPrefs.HasKey("toDemoPanelFromBtn") && PlayerPrefs.GetInt("toDemoPanelFromBtn")==2) //从帮助按钮进来的
			{
				SceneManager.LoadSceneAsync("scene_Start");
			}
			else if ( PlayerPrefs.GetInt("toDemoPanelFromBtn")==1 ) //从开始按钮进来的
			{
				SceneManager.LoadSceneAsync("scene_LevelSelect");
			}
			break;
		case (int)FromPanelFlag.LEVELSELECT:
			SceneManager.LoadSceneAsync("scene_LevelSelect");
			break;
		case (int)FromPanelFlag.PHOTOTAKING:
			SceneManager.LoadSceneAsync("scene_PhotoTaking");
			break;
		case (int)FromPanelFlag.PHOTORECOGNIZE://加载场景并且不销毁当前场景中的对象
			SceneManager.UnloadScene("scene_Demoshow");
			break;
		default:
			break;
		}
		GameObject.DontDestroyOnLoad(GameObject.Find("Manager"));
	}


}
