using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

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
	
public class PhotoRecognizingPanel : MonoBehaviour 
{
	public static PhotoRecognizingPanel _instance;
	//匹配结果
	[HideInInspector]
	public Result result;

	//  btns on the panel
	private GameObject helpBtn;
	private GameObject replayBtn;
	private GameObject nextBtn;

	//  items need to show
	private GameObject bulb;
	private GameObject battery;
	private GameObject switchBtn;
	private GameObject loudspeaker;
	private GameObject voiceOperSwitch;
	private GameObject lightActSwitch;
	private GameObject voiceTimedelaySwitch;
	private GameObject doubleDirSwitch;
	private GameObject inductionCooker;

	private GameObject sunAndMoon;
	private GameObject microPhone;

	//需要跳转连接的界面
	private GameObject commonPanel02;
	private GameObject failurePanel;

	private GameObject labelBgTwinkle;//文字显示的发光背景
	private GameObject lineParent;
	private GameObject linePrefab;
	private GameObject fingerPrefab;
	private GameObject arrowPrefab;

	[HideInInspector]
	public GameObject voiceNoticeBg;//发声提示框

	private UILabel levelNameLabel;

	private UISprite image;//拍摄截取的图像
	private UISprite mask;//遮盖背景图片的蒙板，通过改变透明度来显示拍摄的照片
	[HideInInspector]
	public UISprite nightBg;



	/// <summary>
	/// 判断图标是否显示完的标志
	/// </summary>
	[HideInInspector]
	public  bool isItemShowDone=false;

	/// <summary>
	/// 判断拍摄的照片是否显示完成的标志
	/// </summary>
	private bool isPhotoShowDone = false;

	/// <summary>
	/// 判断箭头电流动画是否完成的标志
	/// </summary>
	[HideInInspector]
	public  bool isArrowShowDone=false;

	/// <summary>
	/// 保证创建图标的协同只走一遍的标志
	/// </summary>
	private bool isCreate_Update=false;

	/// <summary>
	/// 电路是否通电的标志
	/// </summary>
	private bool isEnergized = false;

	private bool isHasSwitch=false;

	/// <summary>
	/// 记录图标数量的信号量，为0时表示所有图标都显示完，可以显示箭头了
	/// </summary>
	private int iconCount = 1;

	/// <summary>
	/// the number of voiceOperSwitch 声控开关的个数，大于0时话筒按钮要伴随出现
	/// </summary>
	private int voiceOperSwitchNum = 0;

	/// <summary>
	/// the number of lightActSwitch 光敏开关的个数，大于0时太阳月亮切换按钮要伴随出现
	/// </summary>
	private int isLightActSwitchNum = 0;

	private float  animationTimer=0;//电流动画播放计时器
	private float animationTime=5f;//重玩按钮和下一步按钮在电流动画播放5秒后出现
	private float distance = 0;
	private float angle = 0;

	private static float maskTimer = 0f;//蒙板渐变计时器
	private float maskTime;//蒙板渐变的总时间=所有item开始显示到显示完成的总时间

	private LevelItemData data;

	private Vector3 fromPos;
	private Vector3 toPos;
	private Vector3 centerPos;

	/// <summary>
	/// 识别面板新创建出来的对象的集合（界面上新创建的对象需要在关闭面板的时候进行销毁，以保证重新打开界面时上一次操作中的新建的对象不会残留）
	/// </summary>
	private List<GameObject> goList=new List<GameObject>();
	[HideInInspector]
	public List<GameObject> switchList = new List<GameObject> ();//普通开关的集合
	private List<CircuitItem> itemsList=new List<CircuitItem>();//图标的集合
	[HideInInspector]
	public List< List<Vector3> > lines=new List<List<Vector3>>();//所有线条的集合
	[HideInInspector]
	public  List<GameObject> arrowList=new List<GameObject>();
	[HideInInspector]
	public  List<Vector3> linePointsList=new List<Vector3>();//线上的所有点的集合，方便第二关的消除线段的取点
	[HideInInspector]
	public  List<GameObject> batteryList = new List<GameObject> ();
	[HideInInspector]
	public  List<GameObject> bulbList = new List<GameObject> ();


	void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		helpBtn = transform.Find ("HelpBtn").GetComponent<UIButton> ().gameObject;
		commonPanel02 = transform.parent.Find ("CommonPanel02").gameObject;
		failurePanel=transform.parent.Find ("CommonPanel02/FailurePanel").gameObject;

		bulb = Resources.Load ("Bulb",typeof(GameObject))  as GameObject;
		battery = Resources.Load ("Battery",typeof(GameObject))  as GameObject;
		switchBtn = Resources.Load ("Switch",typeof(GameObject))  as GameObject;
		loudspeaker = Resources.Load ("Loudspeaker", typeof(GameObject)) as GameObject;
		voiceOperSwitch = Resources.Load ("VoiceOperSwitch", typeof(GameObject)) as GameObject;
		lightActSwitch = Resources.Load ("LightActSwitch", typeof(GameObject)) as GameObject;
		voiceTimedelaySwitch = Resources.Load ("VoiceTimedelaySwitch", typeof(GameObject)) as GameObject;
		doubleDirSwitch = Resources.Load ("DoubleDirSwitch", typeof(GameObject)) as GameObject;
		inductionCooker = Resources.Load ("InductionCooker", typeof(GameObject)) as GameObject;

		arrowPrefab=Resources.Load("Arrow") as GameObject;
		linePrefab = Resources.Load ("lineNew") as GameObject;
		fingerPrefab= Resources.Load ("Finger",typeof(GameObject)) as GameObject;

		UIEventListener.Get (helpBtn).onClick = OnHelpBtnClick;
		UIEventListener.Get (replayBtn).onClick = OnReplayBtnClick;
		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;
	}

		
	void OnEnable()
	{
		result = Result.Success;//for test..

		levelNameLabel = transform.Find ("LevelNameBg/Label").GetComponent<UILabel> ();
		levelNameLabel.text = LevelManager.currentLevelData.LevelName;
		image = transform.Find ("Bg/Image").GetComponent<UISprite> ();//for test..调用图像识别部分的一个接口（该接口返回的是一个UITexture）
		mask=transform.Find("Bg/DayBgMask").GetComponent<UISprite> ();
		//replayBtn=transform.Find("ReplayBtn").GetComponent<UIButton> ().gameObject;
		replayBtn=transform.Find("ReplayBtn").gameObject;
		nextBtn=transform.Find("NextBtn").gameObject;
		labelBgTwinkle = transform.Find ("LevelNameBgT").gameObject;
		voiceNoticeBg = transform.Find ("VoiceNotice").gameObject;
		sunAndMoon = transform.Find ("SunAndMoonWidget").gameObject;
		microPhone = transform.Find ("MicroPhoneBtn").gameObject;
		nightBg = transform.Find ("Bg/NightBg").GetComponent<UISprite> ();
		nightBg.gameObject.SetActive (true);
		nightBg.alpha = 0;

		lineParent = this.gameObject;

		image.gameObject.SetActive (false);
		mask.gameObject.SetActive (true);
		replayBtn.SetActive(false);
		nextBtn.SetActive (false);
		labelBgTwinkle.SetActive (false);
		voiceNoticeBg.SetActive (false);
		sunAndMoon.SetActive (false);
		microPhone.SetActive (false);

		isItemShowDone=false;
		isPhotoShowDone = false;
		isArrowShowDone=false;
		isCreate_Update=false;
		isEnergized = false;
		isHasSwitch=false;

		voiceOperSwitchNum = 0;
		isLightActSwitchNum = 0;

		data = LevelManager.currentLevelData;

	    itemsList=CircuitItemManager._instance.itemList;  // for test

		prePos = Vector3.zero; 
		iconCount = 1;
		mask.alpha = 0;
		maskTime = itemsList.Count  * 1;//显示图标的总时间=图标个数*每个图标隔的时间
		foreach (var item in lines) 
		{
			maskTime += (float)((item.Count - 1) * 0.2);//显示一条线的总时间
		}
		StartCoroutine (PhotoShow ());//进入识别界面的第一步是显示拍摄的照片

		StartCoroutine (RemoveEmptyArrow ());
	}
		
	IEnumerator RemoveEmptyArrow()
	{
		while (true) {
			for (int i = 0; i < arrowList.Count; ) {
				if (arrowList[i]) {
					i++;
				} else {
					arrowList.RemoveAt (i);
				}
			}
			print (arrowList.Count);
			yield return new WaitForSeconds (0.5f);
		}



	}

	/// <summary>
	/// 显示拍摄的图片
	/// </summary>
	/// <returns>The show.</returns>
	IEnumerator PhotoShow()
	{
		//这里需要调用图像识别的一个函数，该函数返回的是一个UItexture
		// to do...
		image.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		isPhotoShowDone = true;
	}

	//private List<GameObject> allSwitches = new List<GameObject> ();
	//识别完以后，播放电流之前，首先得判断有没有开关，如果没有开关，就直接显示电流；

	/// <summary>
	/// 整个电路图中有没有开关的标记
	/// </summary>

	void Update () 
	{
		if (isPhotoShowDone) 
		{
			#region 蒙板出现，透明度渐变
			maskTimer += Time.deltaTime;
			if (maskTimer >= maskTime) 
			{
				maskTimer =maskTime;
			}
			mask.alpha = Mathf.Lerp (0, 1f, maskTimer/maskTime);
			#endregion

			//当一个函数要放在update里面时， 又要保证只执行一次，可以在这个函数之前加一个bool值来标志
			if(isCreate_Update==false)
			{
				//创建图标
				StartCoroutine (CreateAllItem ());
				isCreate_Update =true;
			}
			if (iconCount == 0) 
			{
				isItemShowDone = true;
			}
			if(voiceOperSwitchNum>0)//如果有声控开关，需要伴随出现话筒按钮
			{
				microPhone.SetActive(true);   

			}
			if(isLightActSwitchNum>0)//如果有光敏开关，需要伴随出现太阳月亮按钮
			{
				sunAndMoon.SetActive(true);
			}

			if (isItemShowDone && result == Result.Success) //如果图标都显示完了且匹配成功
			{
				LevelHandle._instance.CircuitHandleByLevelID (LevelManager.currentLevelData.LevelID);
				//CircuitHandleByLevelID (LevelManager.currentLevelData.LevelID);
			}
			#region 如果匹配结果是错误的，就跳转到失败界面
			else if (isItemShowDone && result == Result.Fail) 
			{
				Fail ();
			}
			#endregion
		}
	}

	   
	public Vector3 ChooseRandomPoint()
	{
		int index=Random.Range(0,linePointsList.Count);
		Vector3	pos=linePointsList[index];
		return pos;
	}

	public void ShowFingerOnLine(Vector3 pos)
	{
		StartCoroutine (ShowFingerOnLineT (pos));
	}

	IEnumerator ShowFingerOnLineT(Vector3 pos)//需要传一个线上的随机坐标
	{
		yield return new WaitForSeconds (3f);
		ShowFinger (pos);
	}
		
	private Vector3 offSet = new Vector3 (113, -108, 0);
	[HideInInspector]
	public GameObject finger;
	private Vector3 prePos= Vector3.zero; // 记录当前指向的开关，如果没发生变化，不做任何操作

	public void ShowFinger(Vector3 switchPos)
	{
		if (prePos == switchPos) {
			return;
		}
		prePos = switchPos;
		if (finger) {
			Destroy (finger);
			finger = null;
		}
		finger = Instantiate (fingerPrefab) as GameObject;
		finger.name="finger";
		finger.transform.parent = transform;
		finger.transform.localScale = Vector3.one;
		finger.GetComponent<FingerCtrl> ().FingerShow (switchPos + offSet);
	}

	/// <summary>
	/// 创建所有的图标
	/// </summary>
	IEnumerator CreateAllItem()
	{
		//itemsList=GetImage._instance.itemLists[0];//for test
		iconCount = itemsList.Count;
		for (int i = 0; i < itemsList.Count; i++) 
		{
			CreateSingleItem (itemsList [i]);
			yield return new WaitForSeconds (0.5f);//隔0.5秒创建一个图标
		}
	}
	/// <summary>
	/// 创建单个图标
	/// </summary>
	/// <param name="circuitItem">单个图标</param>
	public void CreateSingleItem(CircuitItem circuitItem)
	{

		GameObject item = null;

		switch (circuitItem.type) 
		{
		case ItemType.Battery://如果是电池，则克隆电池的图标
			item = GameObject.Instantiate (battery) as GameObject;
			goList.Add (item);//新创建一个对象的同时把这个对象加入到对象列表，方便关闭界面的时候销毁这些新创建的对象
			batteryList.Add(item);
			item.name = "battery"; 
			iconCount--;
			break;
	
		case ItemType.Bulb:
			item = GameObject.Instantiate (bulb) as GameObject;
			goList.Add (item);
			bulbList.Add (item);
			item.name = "bulb";
			iconCount--;
			break;

		case ItemType.Switch:
			isHasSwitch = true;
			//item = GameObject.Instantiate (switchOn, circuitItem.list[0], Quaternion.identity) as GameObject;
			item = GameObject.Instantiate (switchBtn) as GameObject;
			switchList.Add (item);
			goList.Add (item);
			item.name = "switch"; 
			iconCount--;
			break;

		case ItemType.DoubleDirSwitch:
			isHasSwitch = true;
			item = GameObject.Instantiate (doubleDirSwitch) as GameObject;
			switchList.Add (item);
			goList.Add (item);
			item.name = "doubleDirSwitch";
			iconCount--;
			break;

		case ItemType.VoiceTimedelaySwitch:
			isHasSwitch = true;
			item = GameObject.Instantiate (voiceTimedelaySwitch) as GameObject;
			goList.Add (item);
			item.name = "voiceTimedelaySwitch";
			voiceOperSwitchNum++;
			iconCount--;
			break;

		case ItemType.VoiceOperSwitch:
			isHasSwitch = true;
			item = GameObject.Instantiate (voiceOperSwitch) as GameObject;
			goList.Add (item);
			item.name = "voiceOperSwitch";
			voiceOperSwitchNum++;//声控开关出现的时候记录一下，话筒按钮需要伴随出现
			iconCount--;
			break;
		
		case ItemType.LightActSwitch:
			isHasSwitch = true;
			item = GameObject.Instantiate (lightActSwitch) as GameObject;
			goList.Add (item);
			item.name = "lightActSwitch";
			isLightActSwitchNum++;//光敏开关出现的时候记录一下，太阳月亮切换按钮需要伴随出现
			iconCount--;
			break;

		case ItemType.Loudspeaker:
			item = GameObject.Instantiate (loudspeaker) as GameObject;
			goList.Add (item);
			item.name = "loudspeaker"; 
			iconCount--;
			break;

		case  ItemType.InductionCooker:
			item = GameObject.Instantiate (inductionCooker) as GameObject;
			goList.Add (item);
			item.name = "inductionCooker";
			iconCount--;
			break;

		case ItemType.CircuitLine:
			//如果是线路，则加入线路列表中，方便计算所有图标创建完的总时间
			lines.Add (circuitItem.list);

			for (int i = 0; i < circuitItem.list.Count; i++) 
			{

				linePointsList.Add (circuitItem.list [i]);

			}
			//开始画线
			StartCoroutine(DrawCircuit(circuitItem.list));
			break;
		default:
			break;

		}
		if (item!=null) 
		{
			item.transform.parent = transform;
			item.transform.localPosition = circuitItem.list [0];// 如果测试用的坐标是根据localPosition设定的，就要用localPosition来接收
			item.transform.localScale = new Vector3 (1, 1, 1); 
			//根据图标数据的旋转角度进行旋转，旋转的是Z上的弧度
			item.transform.Rotate(new Vector3(0,0,(float)circuitItem.theta));
		}
	}

	/// <summary>
	/// 画线路
	/// </summary>
	/// <param name="pos">线上点的集合</param>
	IEnumerator DrawCircuit(List <Vector3> pos)
	{
		for (int i = 0; i < pos.Count - 1; i++)
		{

			DrawLine (pos [i], pos [i + 1]);
			yield return new WaitForSeconds (0.1f);//画一条线，隔0.1秒再画一条

		}
		iconCount--;

	}

	/// <summary>
	/// 两点之间画线
	/// </summary>
	/// <param name="pos">参数是点的集合</param>
	//两点之间画线，需要知道两点之间的距离，线段的中心点，以及角度------思想是把要显示的图片放在中心点的位置，然后把图片的宽度拉伸到和线段一样长，再依照角度旋转
	void DrawLine(Vector3 posFrom, Vector3 posTo)
	{
		distance = Vector3.Distance (posFrom, posTo);
		centerPos = Vector3.Lerp (posFrom, posTo, 0.5f);
		angle = TanAngle (posFrom, posTo);

		GameObject lineGo = NGUITools.AddChild(lineParent, linePrefab);//生成新的连线  
		goList.Add(lineGo);
		UISprite lineSp = lineGo.GetComponent<UISprite>();//获取连线的 UISprite 脚本  
		lineSp.width = (int)(distance+6);//将连线图片的宽度设置为上面计算的距离  
		lineGo.transform.localPosition = centerPos;//设置连线图片的坐标 ----@@@@@@@@@@@@@@@@@@@@#####这里用世界坐标还是本地坐标看测试代码中设定的坐标是根据什么来定的 

		lineGo.GetComponent<BoxCollider>().size = lineGo.GetComponent<UISprite>().localSize;

		lineGo.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);//旋转连线图片  
	}

	IEnumerator ArrowShow()
	{
		List<Vector3> line = new List<Vector3> ();//整个线路上的点
		for (int i = 0; i < lines.Count; i++)
		{
			List<Vector3> singleLine = lines [i];
			for (int j = 0; j <singleLine.Count; j++) 
			{
				line.Add (singleLine [j]);
			}
		}
		for (int j = 0; /*j <line.Count*/; j++) {

			GameObject arrow = Instantiate (arrowPrefab) as GameObject;//应该是在第一条线的第一个点创建一个箭头
			arrowList.Add (arrow);

			arrow.transform.parent = transform;
			arrow.name="arrow";
			arrow.transform.localPosition =line[0];
			arrow.transform.localScale = Vector3.one;
			arrow.GetComponent<MoveCtrl> ().Move (line);
			yield return new WaitForSeconds(0.4f);

			#region 重玩按钮和下一步按钮出现
			animationTimer++;
			if (animationTimer >= animationTime) 
			{
				animationTimer = 0;
				WellDone ();
			}
			#endregion
		}

	}
	#region  箭头的生成和销毁
	private bool isCreateArrow = true;

	public void StopCreateArrows()
	{
		isCreateArrow = false;
	}

	public void ContinueCreateArrows()
	{
		isCreateArrow = true;
	} 
	/// <summary>
	/// 电流停止移动
	/// </summary>
	public void StopCircuit()
	{
		StopCreateArrows ();
		foreach (var item in arrowList) 
		{
			if (item) {
				item.GetComponent<UISprite> ().alpha = 0;
				item.GetComponent<MoveCtrl> ().Stop ();
			}


		}
	}
	/// <summary>
	/// 电流继续移动
	/// </summary>
	public void ContinueCircuit()
	{
		foreach (var item in arrowList) {
			if (item) {
				item.GetComponent<UISprite> ().alpha = 1;
				item.GetComponent<MoveCtrl> ().ContinueStart ();
			}

		}
		ContinueCreateArrows ();
	}

	public void ArrowShowLineByLine(List<List<Vector3>> lines ,int i)
	{
		StartCoroutine (ArrowShowLineByLineT (lines, i));

	}


	IEnumerator ArrowShowLineByLineT(List<List<Vector3>> lines ,int i)//递归协同，三条线一起循环，但又是有顺序的
	{
		bool hasStartCorout = false;//有没有开启协同的标志
		GameObject temp = null;

		List<Vector3> singleLine = lines [i];

		for (int k = 0;  ; k++) 
		{
			if (isCreateArrow) 
			{
				GameObject arrow = Instantiate (arrowPrefab) as GameObject;
				arrowList.Add (arrow);
				arrow.transform.parent = transform;
				arrow.name="arrow";
				arrow.transform.localPosition =singleLine[0];
				arrow.transform.localScale = Vector3.one;

				if (k == 0) 
				{
					temp = arrow;//保存第一个箭头
				}
				if (!hasStartCorout && temp == null && i < lines.Count - 1) 
				{//当第一个箭头为空，也就是箭头被销毁时，而且后面的协同没有开启时，而且保证有几条线就只有几个协同
					StartCoroutine (ArrowShowLineByLineT(lines, i + 1));
					hasStartCorout = true;
				}
				arrow.GetComponent<MoveCtrl> ().Move (singleLine);

				yield return new WaitForSeconds(0.4f);

				#region 重玩按钮和下一步按钮出现
				animationTimer++;
				if (animationTimer >= animationTime) 
				{
					animationTimer = 0;
					WellDone ();
				}
				#endregion
			} else {
				yield return new WaitForFixedUpdate ();
			}
		}
	}
	#endregion

	public void Fail()
	{
		commonPanel02.SetActive (true);
		failurePanel.SetActive (true);
		PanelOff ();
	}


	public void WellDone()
	{
		levelNameLabel.text="Congratulations!";
		labelBgTwinkle.SetActive (true);

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
			LevelManager._instance.LoadLocalLevelProgressData ();
		}
		transform.parent.Find ("LevelSelectPanel").gameObject.SetActive (true);

		PanelOff();

	}
		
	private float TanAngle(Vector2 from, Vector2 to)
	{
		float xdis = to.x - from.x;//计算临边长度  
		float ydis = to.y - from.y;//计算对边长度  
		float tanValue = Mathf.Atan2(ydis, xdis);//反正切得到弧度  
		float angle = tanValue * Mathf.Rad2Deg;//弧度转换为角度  
		return angle;  
	}

	void OnHelpBtnClick(GameObject btn)
	{
		//to do...
	
	}

	public void PanelOff()
	{
		foreach (GameObject item in goList) 
		{
			Destroy (item);//销毁创建的对象，保证再次打开该界面时是最初的界面，如果不销毁的话重新打开时上一次创建的对象会出现在界面
		}
		foreach (GameObject item in arrowList) 
		{
			Destroy (item);//销毁创建的对象，保证再次打开该界面时是最初的界面，如果不销毁的话重新打开时上一次创建的对象会出现在界面
		}

		gameObject.SetActive (false);
	}

	void OnDisable()
	{
		if (finger) {
			Destroy (finger);
			finger = null;
		}
		switchList.Clear ();
	}

}



