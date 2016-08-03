using UnityEngine;
using System.Collections;

public class FailurePanel : MonoBehaviour {


	//有一个提示失败的图片

	private string levelName;//记录当前的关卡名

	private GameObject replayBtn;

	private GameObject photoTakingPanel;

	void Start () {
		levelName = "the first level";// this value is set for test...
		replayBtn = transform.Find ("ReplayBtn").GetComponent<UIButton> ().gameObject;
		photoTakingPanel = transform.parent.parent.Find ("PhotoTakingPanel").gameObject;
		UIEventListener.Get (replayBtn).onClick = OnReplayBtnClick;
	
	}
	

	void OnReplayBtnClick(GameObject btn)
	{
		//返回拍摄界面，需要把关卡等级也一起返回显示在拍摄界面上
		this.gameObject.SetActive(false);
		transform.parent.gameObject.SetActive (false);
		photoTakingPanel.SetActive (true);
		photoTakingPanel.GetComponent<PhotoTakingPanel> ().SetLevelValue (levelName);

	}
}
