using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class LevelUI : MonoBehaviour {


	//用来控制关卡列表，挂在LevelSelectPanel上面
	//包含很多个LevelItemUI---用列表来存储

	public static LevelUI _instance;
	public List<LevelItemUI> itemUIList = new List<LevelItemUI> ();//所有的格子


	private GameObject startPanel;
	private GameObject levelDescriptionPanel;

	public GameObject prefab;
	public GameObject grid;





	void Awake()
	{
		_instance = this;
		//grid = transform.Find ("Scroll View/Grid").GetComponent<UIGrid> ();

		//LevelManager._instance.OnLevelChange += this.OnLevelChange;
		//当LevelManger里边的list数据发生变化的时候通知 LevelUI也同时变化


	}
	void Start()
	{

		UpdateShow ();


	}


	public void UpdateShow01()
	{
		//根据levelmanager里的列表信息逐个给levelitemdata赋值初始化
		for (int i = 0; i < LevelManager._instance.levelItemDataList.Count; i++) 
		{
			//拿数据
			LevelItemData data = LevelManager._instance.levelItemDataList [i];
			//使用数据，初始化levelItemUI
			itemUIList[i].InitLevelItemUI(data);

		}
	}


	void UpdateShow()
	{

		for (int i = 0; i < LevelManager._instance.levelItemDataList.Count; i++) {
		
			//创建button，绑定父子关系
			var temp = Instantiate (prefab);
			temp.transform.parent = grid.transform;

			//temp.GetComponentInChildren
			//附上特有的数据
			LevelItemData data = LevelManager._instance.levelItemDataList [i];
			temp.GetComponent<LevelItemUI>().data=data;
			//根据给过去的数据进行显示
			temp.GetComponent<LevelItemUI>().InitLevelItemUI(data);



			//temp.transform.localScale = Vector3.one;
			temp.transform.localScale = new Vector3(0.3f,0.3f,1);

		}

		//让grid时刻更新item的位置
		grid.GetComponent<UIGrid>().repositionNow=true;
	}
//	void OnLevelChange() 
//	{
//		UpdateShow();
//	}



	public void PanelOn()
	{

		gameObject.SetActive(true);
	}

	public void PanelOff()
	{

		gameObject.SetActive (false);


	}

	public void EnterHomePage()
	{
		//关闭当前界面
		PanelOff ();
		//跳到主界面
		startPanel.SetActive (true);

	}

	public void MusicSet()
	{



	}

	public void Next()
	{
		//关闭当前界面
		PanelOff ();
		//跳到下一个界面，to do...

	}

	//点击下一步按钮触发的操作
	public void EnterLevelDescription()
	{
		gameObject.SetActive (false);
		levelDescriptionPanel.SetActive (true);
	}

	void OnDestroy()
	{
		//LevelManager._instance.OnLevelChange -= this.OnLevelChange;
	}

}
