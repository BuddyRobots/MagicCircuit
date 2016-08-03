using UnityEngine;
using System.Collections;

public class StartPanel : MonoBehaviour {
	
	public static StartPanel _instance;


	private GameObject nextBtn;
	private GameObject musicOnBtn;
	private GameObject musicOffBtn;
	private GameObject levelSelectPanel;
	private GameObject levelDescriptionPanel;


	private bool isMusicOn = true;

	void Awake()
	{
		_instance = this;

	}

	void Start () 
	{		
		
		nextBtn= transform.Find ("NextBtn").gameObject;
		musicOnBtn = transform.Find ("MusicOnBtn").GetComponent<UIButton> ().gameObject;
		musicOffBtn = transform.Find ("MusicOffBtn").GetComponent<UIButton> ().gameObject;


		levelSelectPanel=transform.parent.Find("LevelSelectPanel").gameObject;
		levelDescriptionPanel=transform.parent.Find("DescriptionPanel").gameObject;


		UIEventListener.Get(musicOnBtn).onClick = OnMusicOnBtnClick;
		UIEventListener.Get(musicOffBtn).onClick = OnMusicOffBtnClick;
		UIEventListener.Get(nextBtn).onClick =OnNextBtnClick;

	}

	void  OnMusicOffBtnClick(GameObject btn)  
	{
		isMusicOn = true;
		musicOffBtn.SetActive (false);
		musicOnBtn.SetActive (true);
		//打开音效  to do ...
	}
		

	void OnMusicOnBtnClick(GameObject btn)
	{
		isMusicOn=false;
		musicOffBtn.SetActive (true);
		musicOnBtn.SetActive (false);

		//关闭音效 to do ....
	}


	/// <summary>
	/// 进入到下一个界面
	/// </summary>
	/// <param name="btn">参数是点击的按钮对象</param>
	void OnNextBtnClick(GameObject btn)
	{
		//Debug.Log ("NextBtn Clicked");
		//关闭当前界面，切换到关卡选择界面
		PanelOff ();


		//显示选关界面
		levelSelectPanel.SetActive (true);
	}



	/// <summary>
	/// 关闭界面
	/// </summary>
	public void PanelOff()
	{
		gameObject.SetActive (false);

	}
		

}
