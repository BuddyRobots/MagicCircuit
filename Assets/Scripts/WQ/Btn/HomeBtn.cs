using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HomeBtn : MonoBehaviour//SceneSinglton<HomeBtn>
{
	public static HomeBtn _instance;
	private GameObject startPanel;
//	public VoidDelegate panelOff;
	private GameObject manager;


	void Awake()
	{

		_instance=this;
	}

	void Start() 
	{
		manager=GameObject.Find("Manager");
		UIEventListener.Get (gameObject).onClick += onClick1;

	}

	void onClick1(GameObject go)
	{
		//跳转到选关界面
//		PanelTranslate.Instance.GetPanel(Panels.LevelSelectedPanel);
//		PanelTranslate.Instance.DestoryAllPanel();

		SceneManager.LoadSceneAsync("scene_LevelSelect");
		GameObject.DontDestroyOnLoad(manager);

	}
}
	

