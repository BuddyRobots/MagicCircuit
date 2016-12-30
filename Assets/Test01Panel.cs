using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Test01Panel : MonoBehaviour {


	private GameObject comfirmBtn;


	void Start () 
	{
		comfirmBtn=transform.Find("ConfirmBtn").gameObject;
		UIEventListener.Get(comfirmBtn).onClick=OnConfirmBtnClick;
	}
	
	void OnConfirmBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("scene_test_0");//,LoadSceneMode.Single);

	}

}
