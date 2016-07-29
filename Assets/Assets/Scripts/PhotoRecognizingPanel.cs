using UnityEngine;
using System.Collections;

public class PhotoRecognizingPanel : MonoBehaviour {


	private GameObject homeBtn;
	private GameObject musicOnBtn;
	private GameObject musicOffBtn;
	private GameObject helpBtn;


	private GameObject startPanel;

	private UIAtlas atalas;

	private bool isMusicOn = true;
	//需要计时器，控制背景图片的渐变




//	private Vector3 pos1 = new Vector3 (325f, 498f, 0);
//	private Vector3 pos2 = new Vector3 (145, 292, 0);
//	private Vector3 pos3 = new Vector3 (524, 298, 0);

	void Start () {
		homeBtn = transform.Find ("HomeBtn").GetComponent<UIButton> ().gameObject;
		musicOnBtn = transform.Find ("MusicOnBtn").GetComponent<UIButton> ().gameObject;
		musicOffBtn = transform.Find ("MusicOffBtn").GetComponent<UIButton> ().gameObject;
		helpBtn = transform.Find ("HelpBtn").GetComponent<UIButton> ().gameObject;

		startPanel = transform.parent.Find ("StartPanel").gameObject;
		atalas = Resources.Load ("Atalas/Item/Item")as UIAtlas;


		UIEventListener.Get (homeBtn).onClick = OnHomeBtnClick;
		UIEventListener.Get (musicOnBtn).onClick = OnMusicOnBtnClick;
		UIEventListener.Get(musicOffBtn).onClick = OnMusicOffBtnClick;
		UIEventListener.Get (helpBtn).onClick = OnHelpBtnClick;
		CreateItem(325f, 498f, 0);
	}
	

	void Update () 
	{
		//Debug.Log (Input.mousePosition);


	
	}


	public void CreateItem(float x, float y, float z)
	{
	
		GameObject bulb = (GameObject)Resources.Load ("Bulb",typeof(GameObject));//报错？？？？？？？？？为空
		Debug.Log ("bulb name==="+bulb.name);
		GameObject newBulb= GameObject.Instantiate(bulb, new Vector3(x,y,z), Quaternion.identity) as GameObject;
		newBulb.name = "Tree001"; // 给这棵树起名字
		newBulb.transform.parent = GameObject.Find("PhotoRecognizingPanel").transform; // 将自己的父物体设置成“PhotoRecognizingPanel”
		newBulb.transform.position = newBulb.transform.parent.position; // 放在父物体的原点（相对坐标）
		newBulb.transform.localScale = new Vector3(1,1,1); // 设置这棵树的大小

//		GameObject tree = （GameObject）Resources.Load(“DemoTrees”, typeof(GameObject)); // 导入这棵树
//		GameObject aNewTree = GameObject.Instantiate(tree, new Vector3(x, y, z), Quaternion.identity) as GameObject; //把树克隆出来
//		aNewTree.name = "Tree001"; // 给这棵树起名字
//		aNewTree.transform.parent = GameObject.Find("某个路径").transform; // 将自己的父物体设置成“某个路径”
//		aNewTree.transform.position = new Vector3(aNewTree.transform.parent.x, aNewTree.transform.parent.y, aNewTree.transform.parent.z); // 放在父物体的原点（相对坐标）
//		aNewTree.transform.localScale = new Vector3(100， 100， 100); // 设置这棵树的大小
	}



	/// <summary>
	/// 显示图标
	/// </summary>
	void ShowItem()
	{

		//要显示图片的话需要传入坐标，先要获得图标，再根据坐标显示图标，假设坐标是下面这些
		//	private Vector3 pos1 = new Vector3 (325f, 498f, 0);
		//	private Vector3 pos2 = new Vector3 (145, 292, 0);
		//	private Vector3 pos3 = new Vector3 (524, 298, 0);
		//无背景图标电池  无背景图标闸刀
		//UISprite bulb=NGUITools.AddSprite(this,atalas,"无背景图标灯泡");


		//UISprite battery=NGUITools.AddSprite(this,atalas,"无背景图标电池");

		//UISprite OnOff=NGUITools.AddSprite(this,atalas,"无背景图标闸刀");


	}


	void OnMusicOnBtnClick(GameObject btn)
	{
		isMusicOn=false;
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

	void OnHelpBtnClick(GameObject btn)
	{
	//帮助界面
	
	}


	public void PanelOff()
	{
		gameObject.SetActive (false);

	}



}



