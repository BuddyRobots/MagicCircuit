using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public enum Result
{
	/// <summary>
	/// 不确定是否匹配成功
	/// </summary>
	None,
/// <summary>
/// 匹配成功
/// </summary>
	Success,
	/// <summary>
	/// 匹配失败
	/// </summary>
	Fail
}


public class PhotoRecognizingPanel : MonoBehaviour {

	//不确定匹配是否成功
	[HideInInspector]
	public Result result;

	private GameObject helpBtn;
	private GameObject replayBtn;
	private GameObject nextBtn;

	private GameObject arrowPrefab;

	//需要显示的图标---灯泡，电池，开关 
	private GameObject bulb;
	private GameObject battery;
	private GameObject switchOn;

	//需要跳转连接的界面
	private GameObject commonPanel02;
	//private GameObject wellDonePanel;
	private GameObject failurePanel;
	//private GameObject photoTakingPanel;

	//private UIAtlas atalas;

	private UILabel levelNameLabel;

	/// <summary>
	/// 判断图标是否显示完的标志
	/// </summary>
	private bool isShowDone=false;

	//private bool isAnimationDone=false;//电流动画是否播完的标志

	/// <summary>
	/// 判断拍摄的照片是否显示完成的标志
	/// </summary>
	private bool isPhotoShowDone = false;





	/// <summary>
	/// 判断箭头电流动画是否完成的标志
	/// </summary>
	private bool isArrowShow=false;

	/// <summary>
	/// 判断线是否画完的标志
	/// </summary>
	private bool isCreate = false;

	private bool isCreate_Update=false;


	//需要一张纹理来存储拍摄截取的图像
	private UISprite image;
	//需要计时器，控制背景图片的渐变  ..... to do 
	//private float imageTimer=0;
	//private float toGrayTime=0;


	//显示图标的计时器
	private float timer = 0f;//计时器
	private float intervalTime = 1f;//间隔时间
	private float  animationTimer=0;//电流动画播放计时器
	private float animationTime=5f;//重玩按钮和下一步按钮在电流动画播放两秒后出现


	private int itemCount=0;//图标的数量计数器

	private Transform arrowOccurPos;
	private List<CircuitItem> list=new List<CircuitItem>();//图标的集合
	//private List<Vector3> points=new List<Vector3>();//线上点的集合
	private LevelItemData data;


	//画线--------------for test...
	private List<Vector3> pos=new List<Vector3>();

	private List<Vector3> posArrow=new List<Vector3>();//这个是箭头的坐标点集合

	/// <summary>
	/// 识别面板创建出来的对象，需要在关闭面板的时候进行销毁，以方便重新打开的时候新建的对象被清空
	/// </summary>
	private List<GameObject> goList=new List<GameObject>();

	private Vector3 fromPos;
	private Vector3 toPos;
	private Vector3 centerPos;

	private float distance = 0;
	private float angle = 0;

	private GameObject lineParent;
	private GameObject linePrefab;

	//private float lineTimer = 0;
	//private float time = 1f;

	private int index = 0;

	//--------------------


	void Awake()
	{
		levelNameLabel = transform.Find ("LevelNameBg/Label").GetComponent<UILabel> ();

		#region 画线
		//for test...点的集合
		pos.Add (new Vector3 (1f, 0, 0));
		pos.Add (new Vector3 (10, 1, 0));
		pos.Add (new Vector3 (20, 4, 0));
		pos.Add (new Vector3 (30, 10, 0));
		pos.Add (new Vector3 (40, 20, 0));
		pos.Add (new Vector3 (50, 30, 0));
		pos.Add (new Vector3 (60, 40, 0));
		pos.Add (new Vector3 (70, 50, 0));
		pos.Add (new Vector3 (80, 60, 0));
		pos.Add (new Vector3 (90, 70, 0));



		#endregion

		// for test....箭头位置的集合
		posArrow.Add(new Vector3 (-198, -97, 0));
		posArrow.Add(new Vector3 (-88, -118, 0));
		posArrow.Add(new Vector3 (-8, -124, 0));
		posArrow.Add(new Vector3 (86, -135, 0));
		posArrow.Add(new Vector3 (178, -139, 0));
		posArrow.Add(new Vector3 (300, -108, 0));
		posArrow.Add(new Vector3 (335, -45, 0));
		posArrow.Add(new Vector3 (242, 133, 0));
		posArrow.Add(new Vector3 (158, 204, 0));
		posArrow.Add(new Vector3 (35, 249, 0));
		posArrow.Add(new Vector3 (-74, 229, 0));
		posArrow.Add(new Vector3 (-178, 160, 0));
		posArrow.Add(new Vector3 (-242, 94, 0));
		posArrow.Add(new Vector3 (-259, -14, 0));


	}


	void Start () 
	{
		

	}

	void OnEnable()
	{
		levelNameLabel.text = LevelManager.currentLevelData.LevelName;

		//result = Result.Success;

		isShowDone=false;
		isPhotoShowDone = false;
		isArrowShow=false;
		isCreate = false;
		isCreate_Update=false;
		index = 0;
		//replayBtn.SetActive(false);
		//nextBtn.SetActive (false);
		result = Result.Success;//for test..
		data = LevelManager.currentLevelData;


		commonPanel02 = transform.parent.Find ("CommonPanel02").gameObject;
		//wellDonePanel=transform.parent.Find ("CommonPanel02/WelldonePanel").gameObject;
		failurePanel=transform.parent.Find ("CommonPanel02/FailurePanel").gameObject;
		helpBtn = transform.Find ("HelpBtn").GetComponent<UIButton> ().gameObject;
		replayBtn=transform.Find("ReplayBtn").GetComponent<UIButton> ().gameObject;
		nextBtn=transform.Find("NextBtn").GetComponent<UIButton> ().gameObject;

		image = transform.Find ("Bg/Image").GetComponent<UISprite> ();//for test..调用图像识别部分的一个接口（该接口返回的是一个UITexture）

		bulb = Resources.Load ("Bulb",typeof(GameObject))  as GameObject;
		battery = Resources.Load ("Battery",typeof(GameObject))  as GameObject;
		switchOn = Resources.Load ("SwitchOn",typeof(GameObject))  as GameObject;
		arrowPrefab=Resources.Load("Arrow") as GameObject;
		linePrefab = Resources.Load ("Line") as GameObject;
		lineParent = this.gameObject;

		list=CircuitItemManager._instance.itemList;


		//两点之间画线，需要知道两点之间的距离，线段的中心点，以及角度------思想是把要显示的图片放在中心点的位置，然后把图片的宽度拉伸到和线段一样长，再依照角度旋转
//		distance = Vector3.Distance (fromPos, toPos);
//		centerPos = Vector3.Lerp (fromPos, toPos, 0.5f);
//		angle = TanAngle (fromPos, toPos);

		UIEventListener.Get (helpBtn).onClick = OnHelpBtnClick;
		UIEventListener.Get (replayBtn).onClick = OnReplayBtnClick;
		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;
		replayBtn.SetActive(false);
		nextBtn.SetActive (false);

		StartCoroutine (PhotoShow ());//进入识别界面的第一步是显示拍摄的照片

	}





	/// <summary>
	/// 显示拍摄的图片
	/// </summary>
	/// <returns>The show.</returns>
	IEnumerator PhotoShow()
	{
		//Debug.Log ("PhotoShow");

		image.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		isPhotoShowDone = true;
	}

	void Update () 
	{
		if (isPhotoShowDone) 
		{
			
			if(isCreate_Update==false)
			{
				StartCoroutine (Create ());
				isCreate_Update =true;
			}
			if (isCreate) 
			{
				isShowDone = true;
			}
			//如果所有图标都显示完了，且匹配成功，就播放动画，跳转到welldone界面
			if (isShowDone && result == Result.Success) 
			{
				if (isArrowShow == false) //false表示电流动画没有播放过，true表示已经播放了，保证动画只播放一次
				{
					StartCoroutine (ArrowShow ());
					//动画播放2秒后 label的文字变化（并开始闪动），弹出两个按钮

					isArrowShow = true;
					//Debug.Log ("ArrowShow ()");
				}

			}
			else if (isShowDone && result == Result.Fail) 
			{
				//如果所有图标都显示完了,且匹配失败，跳转到失败界面
				Fail ();
			}
		}

	}
	/// <summary>
	/// 创建图标
	/// </summary>
	IEnumerator Create()
	{
		for (int i = 0; i < list.Count; i++) 
		{
			isCreate = false;//只要进来了就说明线没画完
			CreateItem (list [i]);
			yield return new WaitForSeconds (1);
		}
	}


	//创建图标----传入图标的类型和坐标
	public void CreateItem(CircuitItem circuitItem)
	{

		GameObject item = null;

		switch (circuitItem.type) 
		{
		case ItemType.Battery://如果是电池，则克隆电池的图标
			item = GameObject.Instantiate (battery, circuitItem.list [0], Quaternion.identity) as GameObject;
			goList.Add (item);
			arrowOccurPos = item.transform;//箭头从电池的位置出来
			//lineStart = item;
			item.name = "battery"; 
			break;

		case ItemType.Bulb:
			item = GameObject.Instantiate (bulb, circuitItem.list [0], Quaternion.identity) as GameObject;
			goList.Add (item);
			//lineEnd = item;
			item.name = "bulb";
			break;

		case ItemType.Switch:
			item = GameObject.Instantiate (switchOn, circuitItem.list[0], Quaternion.identity) as GameObject;
			goList.Add (item);
			item.name = "switchOn"; 
			break;

		case ItemType.CircuitLine:
			//goList.Add (item);
			//Debug.Log ("this is a line...");

			//首先要知道线上面的点的坐标
			//DrawLine (circuitItem.list);

			//StartCoroutine(DrawCircuit(circuitItem.list));
			StartCoroutine(DrawCircuit(pos));// for test..


			break;
		default:
			break;

		}
		if (item!=null) 
		{
			item.transform.parent = transform;//GameObject.Find ("PhotoRecognizingPanel").transform; // 将自己的父物体设置成“PhotoRecognizingPanel”

			item.transform.position = circuitItem.list [0]; 
			item.transform.localScale = new Vector3 (1, 1, 1); 
		}
	}




	/// <summary>
	/// 箭头出现
	/// </summary>
	/// <returns>The show.</returns>
	IEnumerator ArrowShow()
	{
		List<Vector3> arrowPos= posArrow;
		for (int i = 0; i < arrowPos.Count; i++)
		{
			GameObject arrow = Instantiate (arrowPrefab, new Vector3 (arrowPos[i].x, arrowPos[i].y, 0), Quaternion.identity) as GameObject;
			goList.Add (arrow);
			arrow.transform.parent = transform;
			arrow.name="arrow";
			arrow.transform.localPosition = arrowPos[i];
			arrow.transform.localScale = Vector3.one;

			if (i < arrowPos.Count - 2) 
			{
				Vector3 looknor = new Vector3 ((arrowPos [i + 1].x - arrowPos [i].x), (arrowPos [i + 1].y - arrowPos [i].y), 0f).normalized;
				arrow.transform.up = looknor;
			}
//			if (i == arrowPos.Count - 1) 
//			{
//				
//				WellDone ();
//				
//			}
			yield return new WaitForSeconds (0.2f);




			animationTimer++;
			if (animationTimer >= animationTime) 
			{
				animationTimer = 0;
				WellDone ();
			
			}
	
		}




	}


	public void Fail()
	{
		commonPanel02.SetActive (true);
		failurePanel.SetActive (true);
		PanelOff ();

	}


	public void WellDone()
	{
		levelNameLabel.text="Congratulations!";
		replayBtn.SetActive (true);
		nextBtn.SetActive (true);

	}
	/// <summary>
	/// 重玩当前关卡
	/// </summary>
	/// <param name="btn">Button.</param>
	void OnReplayBtnClick(GameObject btn)
	{

		transform.parent.Find ("PhotoTakingPanel").gameObject.SetActive (true);
		PanelOff();

	}

	/// <summary>
	/// 下一关
	/// </summary>
	/// <param name="btn">Button.</param>
	void OnNextBtnClick(GameObject btn)
	{
		//记录通关的关卡
		if(data.Progress != LevelProgress.Done){
			PlayerPrefs.SetInt ("LevelID",data.LevelID);
			Debug.Log ("data.LevelID====" + data.LevelID);
			PlayerPrefs.SetInt ("LevelProgress",2);
			LevelManager._instance.loadLocalLevelProgressData ();
		}

		
		transform.parent.Find ("LevelSelectPanel").gameObject.SetActive (true);


		PanelOff();

	}

	/// <summary>
	/// 将玩家的图和关卡图比较，返回bool值，true则播放动画，false则跳转到失败界面
	/// </summary>
	/// <returns><c>true</c>, if with answer was compared, <c>false</c> otherwise.</returns>
	public bool CompareWithAnswer()
	{
		return false;


	}
		
	/// <summary>
	/// 画线路
	/// </summary>
	/// <param name="pos">参数是线上的点的坐标</param>
	/// 
	IEnumerator DrawCircuit(List <Vector3> pos)
	{
		for (int i = 0; index < pos.Count - 1; i++)
		{
			DrawLine (pos);
			yield return new WaitForSeconds (0.2f);//画一条线，隔0.2秒再画一条

		}
		isCreate = true;//只要画完一条线就标志已经画完
	}
		
	private float TanAngle(Vector2 from, Vector2 to)
	{
		float xdis = to.x - from.x;//计算临边长度  
		float ydis = to.y - from.y;//计算对边长度  
		float tanValue = Mathf.Atan2(ydis, xdis);//反正切得到弧度  
		float nnangle = tanValue * Mathf.Rad2Deg;//弧度转换为角度  

		return nnangle;  

	}
	/// <summary>
	/// 两点之间画线
	/// </summary>
	/// <param name="pos">参数是点的集合</param>
	void DrawLine(List <Vector3> pos)
	{

		if (pos.Count == 1) 
		{

			fromPos = toPos = pos [0];
		} 
		else 
		{
			fromPos = pos [index];
			toPos = pos [index + 1];
		}
		#region 画线
		distance = Vector3.Distance (fromPos, toPos);
		centerPos = Vector3.Lerp (fromPos, toPos, 0.5f);
		angle = TanAngle (fromPos, toPos);

		GameObject lineGo = NGUITools.AddChild(lineParent, linePrefab);//生成新的连线  
		goList.Add(lineGo);
		UISprite lineSp = lineGo.GetComponent<UISprite>();//获取连线的 UISprite 脚本  
		lineSp.width = (int)distance;//将连线图片的宽度设置为上面计算的距离  
		lineGo.transform.localPosition = centerPos;//设置连线图片的坐标  
		lineGo.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);//旋转连线图片  

		//画完一条以后 fromPos后移，toPos后移


		#endregion
		index++;
	}



	void OnHelpBtnClick(GameObject btn)
	{
		//to do...
	
	}

	public void PanelOff()
	{
		foreach (GameObject item in goList) 
		{
			Destroy (item);
		}

		gameObject.SetActive (false);
	}
}



