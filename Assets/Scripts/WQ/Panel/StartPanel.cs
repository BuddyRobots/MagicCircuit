using UnityEngine;
using System.Collections;

public class StartPanel : MonoBehaviour {
	
	public static StartPanel _instance;


	private GameObject nextBtn;
//	private GameObject levelSelectPanel;
//	private GameObject levelDescriptionPanel;

	private bool isMusicOn = true;

	void Awake()
	{
		_instance = this;

	}

	void Start () 
	{		
		nextBtn= transform.Find ("NextBtn").gameObject;
//		levelSelectPanel=transform.parent.Find("LevelSelectPanel").gameObject;
//		levelDescriptionPanel=transform.parent.Find("DescriptionPanel").gameObject;
		UIEventListener.Get(nextBtn).onClick =OnNextBtnClick;

	}

	/// <summary>
	/// enter into the next panel
	/// </summary>
	/// <param name="btn">参数是点击的按钮对象</param>
	void OnNextBtnClick(GameObject btn)
	{
//		PanelOff ();
//		//显示选关界面
//		levelSelectPanel.SetActive (true);


//		Destroy(this.gameObject);
//		GameObject levelSelectPanel =Instantiate(Resources.Load("Prefabs/Panel/LevelSelectPanel"),Vector3.zero,Quaternion.identity) as GameObject;
//		levelSelectPanel.transform.parent = GameObject.Find("UI Root").transform;

		PanelTranslate.Instance.GetPanel(Panels.LevelSelectedPanel);

		PanelTranslate.Instance.DestoryThisPanel();

		//NGUITools.AddChild(GameObject.Find("UI Root"), levelSelectPanel);
	}
		
	/// <summary>
	/// 关闭界面
	/// </summary>
//	public void PanelOff()
//	{
//		gameObject.SetActive (false);
//	}
}
