using UnityEngine;
using System.Collections;

public class StartPanel : MonoBehaviour 
{
	
	public static StartPanel _instance;

	private GameObject nextBtn;
	private GameObject helpBtn;
	private bool isMusicOn = true;

	void Awake()
	{
		_instance = this;
	}

	void Start () 
	{		
		nextBtn= transform.Find ("NextBtn").gameObject;
		helpBtn=transform.Find("HelpBtn").gameObject;
		UIEventListener.Get(nextBtn).onClick =OnNextBtnClick;
	}

	/// <summary>
	/// enter into the next panel
	/// </summary>
	/// <param name="btn">参数是点击的按钮对象</param>
	void OnNextBtnClick(GameObject btn)
	{
		if (PlayerPrefs.HasKey("isEnterGameFirstTime") && PlayerPrefs.GetInt("isEnterGameFirstTime")==1) 
		{
			PanelTranslate.Instance.GetPanel(Panels.LevelSelectedPanel);
			PanelTranslate.Instance.DestoryThisPanel();
		}
		else
		{
			PlayerPrefs.SetInt("isEnterGameFirstTime",1);

			PlayerPrefs.SetInt("toDemoPanelFromPanel",1);

			PanelTranslate.Instance.GetPanel(Panels.DemoShowPanel);
			PanelTranslate.Instance.DestoryAllPanel();

		}





	}

	void OnHelpBtnClick(GameObject btn)
	{

		PanelTranslate.Instance.GetPanel(Panels.DemoShowPanel);
		PanelTranslate.Instance.DestoryAllPanel();
	}
}
