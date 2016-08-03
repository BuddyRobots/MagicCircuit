using UnityEngine;
using System.Collections;

public class DescriptionPanel : MonoBehaviour {


	public static DescriptionPanel _instance;

	private UILabel descriptionLabel;
	private UILabel levelNameLabel;

	private GameObject homeBtn;
	private GameObject musicOnBtn;
	private GameObject musicOffBtn;
	private GameObject nextBtn;
	private GameObject photoTakingPanel;
	private GameObject startPanel;

	private LevelItemData data;

	private bool isMusicOn = true;

	void Awake()
	{
		_instance = this;

	}

	void Start () 
	{
		homeBtn = transform.Find ("HomeBtn").GetComponent<UIButton> ().gameObject;
		musicOnBtn = transform.Find ("MusicOnBtn").GetComponent<UIButton> ().gameObject;
		musicOffBtn = transform.Find ("MusicOffBtn").GetComponent<UIButton> ().gameObject;
		nextBtn = transform.Find ("NextBtn").GetComponent<UIButton> ().gameObject;
	
		startPanel = transform.parent.Find ("StartPanel").gameObject;
		photoTakingPanel = transform.parent.Find ("PhotoTakingPanel").gameObject;
		descriptionLabel = transform.Find ("LabelBg/Label").GetComponent<UILabel> ();
		levelNameLabel = transform.Find ("LevelNameBg/Label").GetComponent<UILabel> ();

		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;
		UIEventListener.Get (homeBtn).onClick = OnHomeBtnClick;
		UIEventListener.Get(musicOnBtn).onClick = OnMusicOnBtnClick;
		UIEventListener.Get(musicOffBtn).onClick = OnMusicOffBtnClick;

	}

	/// <summary>
	/// 显示关卡描述文字
	/// </summary>
	/// <param name="data">传入的参数是关卡数据</param>
	public void Show(LevelItemData data) 
	{
		//Debug.Log ("show()");
		this.data = data;
		if(descriptionLabel == null)
		{
			descriptionLabel = transform.Find ("LabelBg/Label").GetComponent<UILabel> ();
		} 
		if(levelNameLabel == null)
		{
			levelNameLabel = transform.Find ("LevelNameBg/Label").GetComponent<UILabel> ();
		} 

		descriptionLabel.text = data.LevelDescription;
		levelNameLabel.text = data.LevelName;
	}


		

	/// <summary>
	/// 点击按钮进入到拍摄界面
	/// </summary>
	/// <param name="btn">Button.</param>
	void OnNextBtnClick(GameObject btn)
	{
		
		//关闭当前界面
		PanelOff ();

		//跳到拍摄界面，to do...
		photoTakingPanel.SetActive(true);
		//把关卡名称传递过去

		photoTakingPanel.transform.GetComponent<PhotoTakingPanel> ().SetLevelValue (levelNameLabel.text);

	}

	void OnMusicOnBtnClick(GameObject btn)
	{
		isMusicOn=false;
		//musicBtn.GetComponent<UISprite> ().spriteName = (isMusicOn ? "音乐" : "静音");
		musicOffBtn.SetActive (true);
		musicOnBtn.SetActive (false);

		//声音开关  to do ....
	}
	void  OnMusicOffBtnClick(GameObject btn)  
	{
		isMusicOn = true;
		musicOffBtn.SetActive (false);
		musicOnBtn.SetActive (true);

	}

	void OnHomeBtnClick(GameObject btn)
	{
		Debug.Log ("OnHomeBtnClick");
		//关闭当前界面
		PanelOff ();
		//返回主界面 
		startPanel.SetActive (true);
		
	}
		

	public void PanelOff()
	{
		gameObject.SetActive (false);

	}


}
