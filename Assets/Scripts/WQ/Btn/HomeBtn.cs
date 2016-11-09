using UnityEngine;
using System.Collections;


public class HomeBtn : SceneSinglton<HomeBtn>
{
	private GameObject startPanel;
	public VoidDelegate panelOff;

	void Awake () 
	{
		//startPanel = GameObject.Find("UI Root").transform.Find ("StartPanel").gameObject;
		UIEventListener.Get (gameObject).onClick += onClick1;
	}

	void onClick1(GameObject go)
	{
		//打开主界面,关闭父对象界面
		//startPanel.SetActive (true);
		PanelTranslate.Instance.GetPanel(Panels.StartPanel);
//		PanelTranslate.Instance.GetPanel(Panels.LevelSelectedPanel);
		Instance.panelOff();
	}
}
	

