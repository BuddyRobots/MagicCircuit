using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Test02Panel : MonoBehaviour {

	private GameObject replayBtn;


	void Start () 
	{
		replayBtn=transform.Find("ReplayBtn").gameObject;
		UIEventListener.Get(replayBtn).onClick=OnReplayBtnClick;
	}

	void OnReplayBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("scene_test_1");//,LoadSceneMode.Single);

	}
}
