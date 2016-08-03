using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhotoRecognizingPanel : MonoBehaviour {

	//面板上的固定按钮
	private GameObject homeBtn;
	private GameObject musicOnBtn;
	private GameObject musicOffBtn;
	private GameObject helpBtn;

	private UILabel levelNameLabel;

	//需要显示的图标---灯泡，电池，开关 
	private GameObject bulb;
	private GameObject battery;
	private GameObject switchOn;

	private GameObject startPanel;

	//private UIAtlas atalas;

	private bool isMusicOn = true;
	private bool isShowDone=false;//图标是否显示完的标志


	//需要一张纹理来存储拍摄截取的图像
	private UISprite image;
	//需要计时器，控制背景图片的渐变  ..... to do 
	//private float imageTimer=0;
	//private float toGrayTime=0;


	//显示图标的计时器
	private float timer = 0f;//计时器
	private float intervalTime = 1f;//间隔时间


	private List<bool> isFirstList=new List<bool>();

	private int itemCount=0;


	private List<CircuitItem> list=new List<CircuitItem>();//图标的集合
	private List<Vector3> points=new List<Vector3>();//线上点的集合

	private bool isCorrect = false;//电路图是否正确的标志

	//line drawing......
//	public List<Transform> pos=new List<Transform>();//所有的点的集合
//	public int index;
//	public GameObject linePrefab;
//	public bool isFinised = true;
	//Vector3[] paths={ new Vector3(0.5f,-0.1f,0),new Vector3(-0.4f,0,0),new Vector3(0.1f,0.4f,0),new Vector3(0.1f,0.2f,0)};

	void Start () 
	{
		
		homeBtn = transform.Find ("HomeBtn").GetComponent<UIButton> ().gameObject;
		musicOnBtn = transform.Find ("MusicOnBtn").GetComponent<UIButton> ().gameObject;
		musicOffBtn = transform.Find ("MusicOffBtn").GetComponent<UIButton> ().gameObject;
		helpBtn = transform.Find ("HelpBtn").GetComponent<UIButton> ().gameObject;
		levelNameLabel = transform.Find ("LevelNameBg/Label").GetComponent<UILabel> ();
		startPanel = transform.parent.Find ("StartPanel").gameObject;

		image = transform.Find ("Bg/Image").GetComponent<UISprite> ();
		//atalas = Resources.Load ("Atalas/Item/Item")as UIAtlas;


		UIEventListener.Get (homeBtn).onClick = OnHomeBtnClick;
		UIEventListener.Get (musicOnBtn).onClick = OnMusicOnBtnClick;
		UIEventListener.Get(musicOffBtn).onClick = OnMusicOffBtnClick;
		UIEventListener.Get (helpBtn).onClick = OnHelpBtnClick;


		bulb = (GameObject)Resources.Load ("Bulb",typeof(GameObject));
		battery = (GameObject)Resources.Load ("Battery",typeof(GameObject));
		switchOn = (GameObject)Resources.Load ("SwitchOn",typeof(GameObject));

//		Debug.Log("switchOn position===="+transform.Find("SwitchOn").position);
//		Debug.Log("switchOn localposition===="+transform.Find("SwitchOn").localPosition);
//
//		Debug.Log("battery position===="+transform.Find("Battery").position);
//		Debug.Log("battery localposition===="+transform.Find("Battery").localPosition);
//
//		Debug.Log("bulb position===="+transform.Find("Bulb").position);
//		Debug.Log("bulb transform position===="+GameObject.Find("UI Root/PhotoRecognizingPanel/Bulb").transform.position);
//
//		Debug.Log("bulb localpositon===="+transform.Find("Bulb").localPosition);
//
//		Debug.Log("panel.transform.positon===="+transform.position);
//		Debug.Log("panel.transform.localpositon===="+transform.localPosition);

		list=CircuitItemManager._instance.itemList;
		//toGrayTime = list.Count;//有多少个item，背景照片变灰的过程就有多少秒
		//ShowItem ();

		Debug.Log (image.color);


		//index = 1;


	}


	void Update () 
	{
		timer += Time.deltaTime;
		if (timer > intervalTime) 
		{
			//每隔一秒创建一个
			Create ();
			timer = 0;
		}
		//显示完线路以后，如果线路正确就显示箭头动画
		//if(isShowDone  && isCorrect)
		if (isShowDone = true && isCorrect == true) 
		{
		
		
		}

	}

//	public void SetLine()
//	{
//		GameObject  go=GameObject.Instantiate(linePrefab) as GameObject;
//
//
//	}

	void Create()
	{
		if (isShowDone == false) 
		{
			CreateItem (list [itemCount]);
			itemCount++;
		}
		if (itemCount >= list.Count) 
		{
			itemCount = 0;
			isShowDone = true;
		}
	
	}



//	public void ShowItem()
//	{
//		for (int i = 0; i < list.Count; ++i) 
//		{
//			CreateItem (list[i]);
//		
//		}
//
//	}

	//创建图标----传入图标的类型和坐标
	public void CreateItem(CircuitItem circuitItem)
	{

		GameObject item = null;
		GameObject lineStart = null;
		GameObject lineEnd = null;

		switch (circuitItem.type) 
		{
		case ItemType.Battery://如果是电池，则克隆电池的图标
			item = GameObject.Instantiate (battery, circuitItem.list [0], Quaternion.identity) as GameObject;
			lineStart = item;
				item.name = "battery"; 
				break;

		case ItemType.Bulb:
			item = GameObject.Instantiate (bulb, circuitItem.list [0], Quaternion.identity) as GameObject;
			lineEnd = item;
				item.name = "bulb";
				break;

			case ItemType.Switch:
				item = GameObject.Instantiate (switchOn, circuitItem.list[0], Quaternion.identity) as GameObject;
				item.name = "switchOn"; 
				break;

		case ItemType.CircuitLine:
			Debug.Log ("this is a line...");
			//如果是线的话，就显示线  to do...


			//首先要知道线上面的点的坐标
			DrawLine (circuitItem.list);

				break;
			default:
				break;

			}
		if (item!=null) 
		{
			item.transform.parent = GameObject.Find ("PhotoRecognizingPanel").transform; // 将自己的父物体设置成“PhotoRecognizingPanel”
			
			item.transform.position = circuitItem.list [0]; 
			item.transform.localScale = new Vector3 (1, 1, 1); 
		}
	}
		
	void DrawLine(List <Vector3> vec)
	{
		//首先要知道线上的点的位置
		//points = vec;
		LineRenderer lr = transform.Find("Line").gameObject.GetComponent<LineRenderer> ();
	
		lr.SetWidth (0.1f, 0.1f);


		//lr.useWorldSpace = false;
		lr.SetVertexCount (vec.Count);
		for (int i = 0; i < vec.Count; ++i) {
		
			lr.SetPosition (i, vec[i]);
		}

	}

//	GameObject go = Resources.Load("Bulb") as GameObject ;
//	void OnDrawGizmos()
//	{
//		Gizmos.color = Color.red;
//		Gizmos.DrawLine (Vector3.zero, new Vector3 (0, 0, 1));
//		LineRenderer.Instantiate (go, Vector3.up, Quaternion.identity);
//
//	}


	void AnimationShow()
	{

		//在电池的旁边位置出现箭头，需要有一个定时器计时，比如每0.5秒出现一个，或者箭头在n秒之内绕完两圈后动画结束



		//动画播完后切换到welldone界面

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


	public void SetLevelValue(string levelName)
	{
		
		levelNameLabel.text = levelName;


	}


}



