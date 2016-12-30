using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using MagicCircuit;
using System.Collections.Generic;

public class StartPanel :MonoBehaviour// SceneSinglton<StartPanel>
{

	private GameObject nextBtn;
	private GameObject helpBtn;
	private GameObject manager;

	public static StartPanel Instance;

	void Awake()
	{
		Instance=this;
		//code for test...
//		PlayerPrefs.SetInt ("LevelID",0);
//		PlayerPrefs.SetInt ("LevelProgress",0);

		LevelManager.Instance.ParseLevelItemInfo();
		LevelManager.Instance.LoadLocalLevelProgressData();

	}

	void Start () 
	{		
		nextBtn= transform.Find ("NextBtn").gameObject;
		helpBtn=transform.Find("HelpBtn").gameObject;
		manager=GameObject.Find("Manager");

		UIEventListener.Get(nextBtn).onClick =OnNextBtnClick;
		UIEventListener.Get(helpBtn).onClick =OnHelpBtnClick;




		///
//		List<CircuitItem> a=new List<CircuitItem>();
//		a.Add(new CircuitItem(1,"a",ItemType.Battery,1));
//		List<CircuitItem> b=new List<CircuitItem>();	
//		b.Add(a[0]);
//		b[0].ID=2;
//		Debug.Log("a[0].ID-------"+a[0].ID);
		///


	}

	/// <summary>
	/// enter into the next panel
	/// </summary>
	/// <param name="btn">参数是点击的按钮对象</param>
	void OnNextBtnClick(GameObject btn)
	{
		
		if (PlayerPrefs.HasKey("isEnterGameFirstTime") && PlayerPrefs.GetInt("isEnterGameFirstTime")==1) //不是第一次进入游戏
		{
			SceneManager.LoadSceneAsync("scene_LevelSelect");
		}
		else//是第一次进入游戏
		{
			PlayerPrefs.SetInt("isEnterGameFirstTime",1);
			PlayerPrefs.SetInt("toDemoPanelFromPanel",(int)FromPanelFlag.START);
			PlayerPrefs.SetInt("toDemoPanelFromBtn",1);

			SceneManager.LoadSceneAsync("scene_DemoShow");
		}
		GameObject.DontDestroyOnLoad(manager);
	}

	void OnHelpBtnClick(GameObject btn)
	{
		PlayerPrefs.SetInt("toDemoPanelFromPanel",(int)FromPanelFlag.START);//标记是从哪个界面进入帮助界面的
		PlayerPrefs.SetInt("toDemoPanelFromBtn",2);
		SceneManager.LoadSceneAsync("scene_DemoShow");
		GameObject.DontDestroyOnLoad(manager);
	}
}
