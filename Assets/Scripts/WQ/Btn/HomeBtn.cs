using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HomeBtn : MonoBehaviour//SceneSinglton<HomeBtn>
{
//	public static HomeBtn _instance;
	private GameObject startPanel;
	private GameObject manager;


//	void Awake()
//	{
//		_instance=this;
//	}

	void Start() 
	{
		manager=GameObject.Find("Manager");
		UIEventListener.Get (gameObject).onClick = OnHomeBtnClick;

	}

	void OnHomeBtnClick(GameObject go)
	{

		SceneManager.LoadSceneAsync("scene_LevelSelect");
		GameObject.DontDestroyOnLoad(manager);

	}
}
	

