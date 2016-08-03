using UnityEngine;
using System.Collections;

public class WelldonePanel : MonoBehaviour {


	private GameObject replayBtn;
	private GameObject nextBtn;
	private GameObject photoTakingPanel;

	private string curLevelName;//记录当前的关卡名
	private string nextLevelName;//记录下一关的关卡名



	void Start () {
	
		replayBtn = transform.Find ("ReplayBtn").GetComponent<UIButton> ().gameObject;
		nextBtn = transform.Find ("NextBtn").GetComponent<UIButton> ().gameObject;
		photoTakingPanel = transform.parent.parent.Find ("PhotoTakingPanel").gameObject;

		UIEventListener.Get (replayBtn).onClick = OnReplayBtnClick;
		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;

	}


	void OnReplayBtnClick(GameObject btn)//重玩
	{
		Debug.Log ("GameObject:" + btn.name + " is  clicked" );
		//关闭当前界面
		PanelOff();

		//重玩，返回到当前关卡的（开始拍摄界面）
		photoTakingPanel.SetActive(true);
		//把当前关卡名传递过去
		photoTakingPanel.GetComponent<PhotoTakingPanel> ().SetLevelValue (curLevelName);

	}

	public void PanelOff()
	{
		gameObject.SetActive(false);
		transform.parent.gameObject.SetActive (false);
	}

	void OnNextBtnClick(GameObject btn)
	{
		//Debug.Log ("GameObject:" + btn.name+ " is  clicked" );
		//返回到选择关卡界面/关卡描述界面
		//标记当前关卡的进度为Done    to do .....
		PanelOff();

		transform.parent.parent.Find ("LevelSelectPanel").gameObject.SetActive (true);

	}
}
