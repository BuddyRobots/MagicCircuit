using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DescriptionPanel : MonoBehaviour//SceneSinglton<DescriptionPanel> 
{
	private UILabel descriptionLabel;
	private UILabel levelNameLabel;

	private GameObject nextBtn;
	private GameObject manager;

//	public  LevelItemData data;


	void Start () 
	{
		nextBtn = transform.Find ("NextBtn").GetComponent<UIButton> ().gameObject;
		manager=GameObject.Find("Manager");

		descriptionLabel = transform.Find ("LabelBg/Label").GetComponent<UILabel> ();
		levelNameLabel = transform.Find ("LevelNameBg/Label").GetComponent<UILabel> ();
		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;


		if (LevelManager.currentLevelData!=null) 
		{
			Show(LevelManager.currentLevelData);
		}


	}

	/// <summary>
	/// 显示关卡描述文字
	/// </summary>
	/// <param name="data">传入的参数是关卡数据</param>
	public void Show(LevelItemData data) 
	{
//		this.data = data;

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
		

//		PanelTranslate.Instance.GetPanel(Panels.PhotoTakingPanel);
//		PanelOff ();

		SceneManager.LoadSceneAsync("scene_PhotoTaking");
		GameObject.DontDestroyOnLoad(manager);

	}

//	public void PanelOff()
//	{
//		PanelTranslate.Instance.DestoryThisPanel();
//	}

}
