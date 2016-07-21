using UnityEngine;
using System.Collections;

public class DescriptionPanel : MonoBehaviour {


	public static DescriptionPanel _instance;
	private GameObject photoTakingPanel;

	private UILabel descriptionLabel;
	private GameObject nextBtn;
	private GameObject startPanel;// for test..
	private LevelItemData data;

	void Awake()
	{
		_instance = this;
		//CommonPanel01.panelFlag = 2;


	}

	void Start () 
	{
		//startPanel = GameObject.Find ("StartPanel");// for test..
		descriptionLabel = transform.Find ("Bg/Label").GetComponent<UILabel> ();
		//photoTakingPanel = GameObject.Find ("UIRoot/PhotoTakingPanel");
		photoTakingPanel = transform.parent.parent.Find ("PhotoTakingPanel").gameObject;

		nextBtn = transform.Find ("NextBtn").GetComponent<UIButton> ().gameObject;

		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;

	}

	//显示从commonPannel01传递的数据
	public void Show(LevelItemData data) {
		//this.gameObject.SetActive(true);
		PanelOn();
		this.data = data;
		if(descriptionLabel == null){
			descriptionLabel = transform.Find ("Bg/Label").GetComponent<UILabel> ();
		} 
		descriptionLabel.text = data.LevelDescription;
	}

	public void PanelOn()
	{
		gameObject.SetActive(true);
	}

	public void PanelOff()
	{
		gameObject.SetActive (false);

	}
		


	void OnNextBtnClick(GameObject btn)
	{
		Debug.Log ("next btn clicled, ready to take photos");
		//关闭当前界面
		//PanelOff ();
		CommonPanel01._instance.PanelOff ();
		//跳到拍摄界面，to do...
		photoTakingPanel.SetActive(true);


	}


}
