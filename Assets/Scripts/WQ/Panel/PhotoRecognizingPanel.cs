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


	private const float lineItemInterval = 0.1f;
	private const float itemInterval = 0.5f;
	[HideInInspector]
	public  float arrowGenInterval = 0.8f;

	//匹配结果
	[HideInInspector]

	//public Result result;
	public bool result;

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
	[HideInInspector]
	public GameObject hourGlass;

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
	[HideInInspector]
	public GameObject microphoneAniBg;//声音收集喇叭图片

	private UILabel levelNameLabel;
	[HideInInspector]
	public UILabel countDownLabel;

	private UITexture photoImage;//拍摄截取的图像
	private UITexture dayMask;//遮盖背景图片的蒙板，通过改变透明度来显示拍摄的照片
	private UITexture successShow;//welldone 界面
	private UITexture failureShow;//failure界面


	[HideInInspector]
	public UITexture nightMask;

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
	/// mark to judge whether arrows are created or not
	/// </summary>
	private bool isArrowCreated=false;
	private bool isCreateArrowSingleLine=false;
	/// <summary>
	/// make to judge if it's necessary to create arrow
	/// </summary>
	[HideInInspector]
	public bool isCreateArrow = true;


	/// <summary>
	/// 结果显示界面只打开一次的标志
	/// </summary>
	private bool isShowResult = false;

	/// <summary>
	/// 数据只取一次的标记
	/// </summary>
	private bool GetItemList = false;


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

	private int isVoiceDelaySwitchNum=0;

	private float distance = 0;
	private float angle = 0;

	private float maskTimer = 0f;//蒙板渐变计时器
	private float maskTime=0f;//蒙板渐变的总时间=所有item开始显示到显示完成的总时间

	private LevelItemData data;

	private Vector3 fromPos;
	private Vector3 toPos;
	private Vector3 centerPos;



	/// <summary>
	/// 识别面板新创建出来的对象的集合（界面上新创建的对象需要在关闭面板的时候进行销毁，以保证重新打开界面时上一次操作中的新建的对象不会残留）
	/// </summary>
	private List<GameObject> goList=new List<GameObject>();
	[HideInInspector]
	public List<GameObject> switchList = new List<GameObject> ();//开关的集合
	[HideInInspector]
	public List<CircuitItem> itemList=new List<CircuitItem>();//图标的集合
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
	public List<List<Vector3>> circuitLines;
	[HideInInspector]
	public List<int> tags=new List<int>();

	[HideInInspector]
	public int transValue = 0;

	void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		helpBtn = transform.Find ("HelpBtn").GetComponent<UIButton> ().gameObject;
		commonPanel02 = transform.parent.Find ("CommonPanel02").gameObject;
		failurePanel=transform.parent.Find ("CommonPanel02/FailurePanel").gameObject;

		bulb = Resources.Load ("Prefabs/Items/Bulb",typeof(GameObject))  as GameObject;
		battery = Resources.Load ("Prefabs/Items/Battery",typeof(GameObject))  as GameObject;
		switchBtn = Resources.Load ("Prefabs/Items/Switch",typeof(GameObject))  as GameObject;
		loudspeaker = Resources.Load ("Prefabs/Items/Loudspeaker", typeof(GameObject)) as GameObject;
		voiceOperSwitch = Resources.Load ("Prefabs/Items/VoiceOperSwitch", typeof(GameObject)) as GameObject;
		lightActSwitch = Resources.Load ("Prefabs/Items/LightActSwitch", typeof(GameObject)) as GameObject;
		voiceTimedelaySwitch = Resources.Load ("Prefabs/Items/VoiceTimedelaySwitch", typeof(GameObject)) as GameObject;
		doubleDirSwitch = Resources.Load ("Prefabs/Items/DoubleDirSwitch", typeof(GameObject)) as GameObject;
		inductionCooker = Resources.Load ("Prefabs/Items/InductionCooker", typeof(GameObject)) as GameObject;

		arrowPrefab=Resources.Load("Prefabs/Items/Arrow") as GameObject;
		linePrefab = Resources.Load ("Prefabs/Items/lineNew") as GameObject;
		fingerPrefab= Resources.Load ("Prefabs/Items/Finger",typeof(GameObject)) as GameObject;

		UIEventListener.Get (helpBtn).onClick = OnHelpBtnClick;
		UIEventListener.Get (replayBtn).onClick = OnReplayBtnClick;
		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;


	}

		
	void OnEnable()
	{
		isShowResult = false;
		isLevelHandleAdd=false;

		HomeBtn.Instance.panelOff = PanelOff;
		maskTimer = 0;
		maskTime = 0;
		arrowGenInterval = 0.8f;
		transValue = 0;

		levelNameLabel = transform.Find ("LevelNameBg/Label").GetComponent<UILabel> ();
		levelNameLabel.text = LevelManager.currentLevelData.LevelName;
		countDownLabel = transform.Find ("CountDownLabel").GetComponent<UILabel> ();


		photoImage =transform.Find ("Bg/PhotoImage").GetComponent<UITexture> ();//real code 

		replayBtn=transform.Find("ReplayBtn").gameObject;
		nextBtn=transform.Find("NextBtn").gameObject;
		labelBgTwinkle = transform.Find ("LevelNameBgT").gameObject;
		voiceNoticeBg = transform.Find ("VoiceNotice").gameObject;
		microphoneAniBg = transform.Find ("MicrophoneAniBg").gameObject;

		sunAndMoon = transform.Find ("SunAndMoonWidget").gameObject;
		microPhone = transform.Find ("MicroPhoneBtn").gameObject;
		//hourGlass = transform.Find ("HourGlass").gameObject;

		dayMask = transform.Find ("Bg/DayBgT").GetComponent<UITexture> ();
		dayMask.gameObject.SetActive (true);
		dayMask.alpha = 0;
		successShow = transform.Find ("WellDoneBg").GetComponent<UITexture> ();
		failureShow=transform.Find ("FailureBg").GetComponent<UITexture> ();
		successShow.gameObject.SetActive (false);
		failureShow.gameObject.SetActive (false);

		nightMask = transform.Find ("Bg/NightBgT").GetComponent<UITexture> ();
		nightMask.gameObject.SetActive (true);
		nightMask.alpha = 0;

		lineParent = this.gameObject;

		photoImage.gameObject.SetActive (false);//real code 

		replayBtn.SetActive(false);
		nextBtn.SetActive (false);
		labelBgTwinkle.SetActive (false);
		voiceNoticeBg.SetActive (false);
		microphoneAniBg.SetActive (false);
		sunAndMoon.SetActive (false);
		microPhone.SetActive (false);
		countDownLabel.gameObject.SetActive(false);

		isItemShowDone=false;
		isPhotoShowDone = false;
		isArrowShowDone=false;
		isCreate_Update=false;
		isArrowCreated=false;
		isCreateArrowSingleLine=false;
		GetItemList = false;

		voiceOperSwitchNum = 0;
		isLightActSwitchNum = 0;
		isVoiceDelaySwitchNum = 0;

		data = LevelManager.currentLevelData;
		prePos = Vector3.zero; 
		iconCount = 1;
		circuitLines=new List<List<Vector3>>();

		jianBianShiJian = false;
//		itemList=GetImage._instance.itemList;
//		result = GetImage._instance.result;//real code
//		GetCircuitLines ();
//		maskTime = (itemList.Count-1)  * itemInterval;//显示图标的总时间=(图标个数-1)*图标间隔时间
//		foreach (var item in circuitLines) 
//		{
//			maskTime += (float)((item.Count - 1) * lineItemInterval);//显示一条线的总时间
//		}



		StartCoroutine (PhotoShow ());//进入识别界面的第一步是显示拍摄的照片
		StartCoroutine (RemoveEmptyArrow ());
	}

	IEnumerator RemoveEmptyArrow()
	{
		while (true) 
		{
			for (int i = 0; i < arrowList.Count; ) 
			{
				if (arrowList[i]) 
				{
					i++;
				} 
				else 
				{
					arrowList.RemoveAt (i);
				}
			}
			yield return new WaitForSeconds (itemInterval);
		}
	}




	/// <summary>
	/// 显示拍摄的图片
	/// </summary>
	/// <returns>The show.</returns>
	IEnumerator PhotoShow()
	{
		photoImage.gameObject.SetActive (true);
		photoImage.mainTexture = GetImage._instance.texture;

		yield return new WaitForSeconds (1f);
		isPhotoShowDone = true;
	}

	//渐变时间开关
	private bool jianBianShiJian = false;
	void Update () 
	{
		
		print ("GetImage._instance.isHandleDone_ItemList====" + GetImage._instance.isHandleDone_ItemList);
		if ( GetImage._instance.isHandleDone_ItemList) //如果数据处理完了，还没有取数据，就取数据
		{
			itemList=GetImage._instance.itemList;
			//Debug.Log ("****************final itemList******");
			for (int k = 0; k < itemList.Count; k++) 
			{
				
				//Debug.Log(k + " " + itemList[k].list[0] + " "  + itemList[k].powered);
			}
			//Debug.Log ("********************");

			result = GetImage._instance.result;//real code
			//Debug.Log("final result====="+result);
			if (!jianBianShiJian) 
			{
				GetCircuitLines ();
				//计算蒙板渐变需要的总时间
				maskTime = (itemList.Count-1)  * itemInterval;//显示图标的总时间=(图标个数-1)*图标间隔时间
				foreach (var item in circuitLines) 
				{
					maskTime += (float)((item.Count - 1) * lineItemInterval);//显示一条线的总时间
				}

				jianBianShiJian = true;
			}
			//----------------------------
			Debug.Log("maskTime==="+maskTime);


		} 
		else 
		{
			return;
		}
			
		//如果图片显示完了，且取到了数据，就进行后面的显示
		if (isPhotoShowDone )//&& GetItemList) 
		{
			
			#region 蒙板出现，透明度渐变
			maskTimer += Time.deltaTime;
			if (maskTimer >= maskTime) 
			{
				maskTimer =maskTime;
			}
			dayMask.alpha=Mathf.Lerp (0, 1f, maskTimer/maskTime);
			#endregion

			if(isCreate_Update==false)//当一个函数要放在update里面时， 又要保证只执行一次，可以在这个函数之前加一个bool值来标志
			{
				StartCoroutine (CreateAllItem ());//创建图标
				isCreate_Update =true;
			}
			if (!isArrowCreated) 
			{
				CreateArrow ();
				isArrowCreated = true;
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
			if (isVoiceDelaySwitchNum>0) 
			{
				//hourGlass.SetActive (true);
			}

			// 结果展示逻辑
			if (isItemShowDone && !isShowResult)
			{
				if (result) {
					WellDone ();
				} else {
					Fail ();
				}
				isShowResult = true;
			}
			//如果图标都显示完了且匹配成功，就根据关卡等级添加界面操作的脚本
			if (isItemShowDone && result)
			{
				LevelHandle._instance.CircuitHandleByLevelID (LevelManager.currentLevelData.LevelID);
			}


			if ( !isLevelHandleAdd && isItemShowDone ) //如果图标都显示完了，就给面板添加相应的关卡脚本
			{

				transform.gameObject.AddComponent<LevelHandle>();
				isLevelHandleAdd = true;
			}


		}
	}

	private bool isLevelHandleAdd=false;

	/// <summary>
	/// 从所有的线中选一个随机点
	/// </summary>
	/// <returns>The random point.</returns>
	public Vector3 ChooseRandomPoint()
	{
		int maxNum = 0;//点的最大数量
		int longestLineIndex = 0;//最长线段的下标
		for (int i = 0; i < lines.Count; i++) 
		{
			if (lines [i].Count > maxNum) 
			{
				maxNum=lines [i].Count;
				longestLineIndex = i;
			}	
		}
		//最长的线段是lines[longestLineIndex],该线段上的点的个数是maxNum；
		//需要取这条线段的中点
		int index = 0;
		if (maxNum % 2 == 0) 
		{
			index = maxNum / 2;	
		} 
		else 
		{
			index = (maxNum + 1) / 2;
		}
		Vector3 pos = (lines [longestLineIndex]) [index];
		return pos;
	}

	/// <summary>
	/// 在线条上显示手指
	/// </summary>
	/// <param name="pos">Position.</param>
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
	[HideInInspector]
	public Vector3 prePos= Vector3.zero; // 记录当前指向的开关，如果没发生变化，不做任何操作

	/// <summary>
	/// 在item上显示手指
	/// </summary>
	/// <param name="switchPos">Switch position.</param>
	public void ShowFinger(Vector3 switchPos)
	{
		if (prePos == switchPos) 
		{
			return;
		}
		prePos = switchPos;
		if (finger) 
		{
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
		//itemsList=CurrentFlow._instance.circuitItems;
		iconCount = itemList.Count;
		print(itemList.Count + "*************");
		for (int i = 0; i < itemList.Count; i++) 
		{
			
			CreateSingleItem (itemList [i]);
			yield return new WaitForSeconds (itemInterval);//隔0.5秒创建一个图标
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
			item.tag = circuitItem.ID.ToString ();
			iconCount--;
			break;
	
		case ItemType.Bulb:
			item = GameObject.Instantiate (bulb) as GameObject;
			goList.Add (item);
			bulbList.Add (item);
			item.name = "bulb";
			item.tag = circuitItem.ID.ToString ();
			iconCount--;
			break;

		case ItemType.Switch:
			//item = GameObject.Instantiate (switchOn, circuitItem.list[0], Quaternion.identity) as GameObject;
			item = GameObject.Instantiate (switchBtn) as GameObject;
			switchList.Add (item);
			goList.Add (item);
			item.name = "switch"; 
			item.tag = circuitItem.ID.ToString ();
			iconCount--;
			break;

		case ItemType.SPDTSwitch:
			item = GameObject.Instantiate (doubleDirSwitch) as GameObject;
			switchList.Add (item);
			goList.Add (item);
			item.name = "doubleDirSwitch";
			item.tag = circuitItem.ID.ToString ();
			iconCount--;
			break;

		case ItemType.VoiceTimedelaySwitch:
			item = GameObject.Instantiate (voiceTimedelaySwitch) as GameObject;
			switchList.Add (item);
			goList.Add (item);
			item.name = "voiceTimedelaySwitch";
			item.tag = circuitItem.ID.ToString ();
			voiceOperSwitchNum++;
			isVoiceDelaySwitchNum++;
			iconCount--;
			break;

		case ItemType.VoiceOperSwitch:
			item = GameObject.Instantiate (voiceOperSwitch) as GameObject;
			switchList.Add (item);
			goList.Add (item);
			item.name = "voiceOperSwitch";
			item.tag = circuitItem.ID.ToString ();
			voiceOperSwitchNum++;//声控开关出现的时候记录一下，话筒按钮需要伴随出现
			iconCount--;
			break;
		
		case ItemType.LightActSwitch:
			item = GameObject.Instantiate (lightActSwitch) as GameObject;
			switchList.Add (item);
			goList.Add (item);
			item.name = "lightActSwitch";
			item.tag = circuitItem.ID.ToString ();
			isLightActSwitchNum++;//光敏开关出现的时候记录一下，太阳月亮切换按钮需要伴随出现
			iconCount--;
			break;

		case ItemType.Loudspeaker:
			item = GameObject.Instantiate (loudspeaker) as GameObject;
			goList.Add (item);
			item.name = "loudspeaker"; 
			item.tag = circuitItem.ID.ToString ();
			iconCount--;
			break;

		case  ItemType.InductionCooker:
			item = GameObject.Instantiate (inductionCooker) as GameObject;
			goList.Add (item);
			item.name = "inductionCooker";
			item.tag = circuitItem.ID.ToString ();
			iconCount--;
			break;

		case ItemType.CircuitLine:
			//如果是线路，则加入线路列表中，方便计算所有图标创建完的总时间
			lines.Add (circuitItem.list);
			print(circuitItem.list.Count + "..........");
			for (int i = 0; i < circuitItem.list.Count; i++) 
			{
				linePointsList.Add (circuitItem.list [i]);
			}
			//开始画线
			StartCoroutine (DrawCircuit (circuitItem.list));
			break;
		default:
			break;
		}
		if (item!=null) 
		{
			item.transform.parent = transform;
			//音响的图片是音响和音符连在一起的大图，图片中的音响重心靠下，需要往上提重心才能使得界面上的图标看起来合理
			if (item.name == "loudspeaker") 
			{
				Vector3 temp = new Vector3 (circuitItem.list [0].x, circuitItem.list [0].y + 75, 0);
				item.transform.localPosition = temp;

			} 
			else
			{
				item.transform.localPosition = circuitItem.list [0];// 如果测试用的坐标是根据localPosition设定的，就要用localPosition来接收
			}

			item.transform.localScale = new Vector3 (1, 1, 1); 
			//根据图标数据的旋转角度进行旋转，旋转的是Z上的弧度
			item.transform.Rotate(new Vector3(0,0,(float)circuitItem.theta));
		}
	}
		
	/// <summary>
	/// 画线路itemList
	/// </summary>
	/// <param name="pos">线上点的集合</param>
	IEnumerator DrawCircuit(List <Vector3> pos)
	{
		for (int i = 0; i < pos.Count - 1; i++)
		{

			DrawLine (pos [i], pos [i + 1]);
			yield return new WaitForSeconds (lineItemInterval);//画一条线，隔0.1秒再画一条

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
		//angle = TanAngle (posFrom, posTo);
		angle = CommonFuncManager._instance.TanAngle (posFrom, posTo);
		GameObject lineGo = NGUITools.AddChild(lineParent, linePrefab);//生成新的连线  
		goList.Add(lineGo);
		UISprite lineSp = lineGo.GetComponent<UISprite>();//获取连线的 UISprite 脚本  
		lineSp.width = (int)(distance+6);//将连线图片的宽度设置为上面计算的距离  
		lineGo.transform.localPosition = centerPos;//设置连线图片的坐标 ----@@@@@@@@@@@@@@@@@@@@#####这里用世界坐标还是本地坐标看测试代码中设定的坐标是根据什么来定的 
		lineGo.GetComponent<BoxCollider>().size = lineGo.GetComponent<UISprite>().localSize;
		lineGo.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);//旋转连线图片  
	}

	#region  箭头的生成和销毁
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
	public void StopCircuit()//先停止创建箭头，再隐藏箭头
	{
		StopCreateArrows ();
		foreach (var item in arrowList) 
		{
			if (item) 
			{
				item.GetComponent<UISprite> ().alpha = 0;
				item.GetComponent<MoveCtrl> ().Stop ();
			}
		}
	}
	/// <summary>
	/// 电流继续移动
	/// </summary>
	public void ContinueCircuit()//隐藏的箭头先出现，再继续创建箭头
	{
		foreach (var item in arrowList) 
		{
			if (item) 
			{
				item.GetComponent<UISprite> ().alpha = 1;
				item.GetComponent<MoveCtrl> ().ContinueStart ();
			}

		}
		ContinueCreateArrows ();
	}
		
	#endregion

	//获得线路上的所有线-----用来创建电流
	void GetCircuitLines()
	{
		for (int i = 0; i < itemList.Count; i++) 
		{
			if (itemList[i].type==ItemType.CircuitLine) 
			{
				circuitLines.Add (itemList[i].list);
				//itemsList[i].ID//equals to tag of arrow
				tags.Add (itemList[i].ID);
			}
		}
	}


	/// <summary>
	/// create arrow around the circuit
	/// </summary>
	public void CreateArrow()
	{
		for (int i = 0; i < circuitLines.Count; i++) 
		{
			isCreateArrowSingleLine = true;
			if (isCreateArrowSingleLine) 
			{
//				Debug.Log ("-----before CreateArrowOnSingleLine---");
//				for (int k = 0; k <circuitLines[i].Count; k++) {
//					Debug.Log ("circuitLines[" + i + "][" + k + "]:" + circuitLines [i][k]);
//					
//				}
				//create arrows on every single line
				List<Vector3> templine = new List<Vector3>();
				for (int n = 0; n < circuitLines[i].Count; n++) {
					templine.Add (circuitLines[i] [n]);
				}

				StartCoroutine (CreateArrowOnSingleLine(templine,tags[i]));
			}
		}
	}


	IEnumerator CreateArrowOnSingleLine(List<Vector3> line,int tag)
	{
		isCreateArrowSingleLine = false;
		for (int k = 0;; k++) 
		{
			if (isCreateArrow) 
			{
				//从线的第一个点出箭头，一直往前移动，移动到最后一个销毁
				GameObject arrow = Instantiate (arrowPrefab) as GameObject;
				arrow.tag = tag.ToString ();
				arrow.GetComponent<UISprite> ().alpha =transValue;
				arrowList.Add (arrow);
				arrow.transform.parent = transform;
				arrow.name = "arrow";
				arrow.transform.localPosition = line [0];
				arrow.transform.localScale = Vector3.one;
				arrow.GetComponent<MoveCtrl> ().Move (line);

				yield return new WaitForSeconds (arrowGenInterval);
			} 
			else 
			{
				yield return new WaitForFixedUpdate ();
			}
		}

	}

	public void Fail()
	{
		failureShow.gameObject.SetActive (true);//显示失败界面
		replayBtn.SetActive (true);//显示重玩按钮
	}
		
	public void WellDone()
	{
		levelNameLabel.text="Congratulations!";
		labelBgTwinkle.SetActive (true);
		replayBtn.SetActive (true);
		nextBtn.SetActive (true);
		StartCoroutine (SuccessShow ());
	}

	IEnumerator SuccessShow()
	{
		yield return new WaitForSeconds (2f);
		successShow.gameObject.SetActive (true);
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
		if(data.Progress != LevelProgress.Done)
		{
			PlayerPrefs.SetInt ("LevelID",data.LevelID);
			PlayerPrefs.SetInt ("LevelProgress",2);
			LevelManager._instance.LoadLocalLevelProgressData ();
		}
		transform.parent.Find ("LevelSelectPanel").gameObject.SetActive (true);

		PanelOff();

	}

	void OnHelpBtnClick(GameObject btn)
	{
		//to do...
	
	}

	public void PanelOff()
	{
		for (int i = 0; i < goList.Count; i++) //销毁创建的对象，保证再次打开该界面时是最初的界面，如果不销毁的话重新打开时上一次创建的对象会出现在界面
		{
			Destroy (goList [i]);
		}
		for (int i = 0; i < arrowList.Count; i++) 
		{
			Destroy (arrowList [i]);
		}
		lines.Clear ();
		circuitLines.Clear ();
		gameObject.SetActive (false);
		successShow.gameObject.SetActive (false);
		failureShow.gameObject.SetActive (false);

		itemList.Clear();
	}

	void OnDisable()
	{
		if (finger) 
		{
			Destroy (finger);
			finger = null;
		}
		switchList.Clear ();
		bulbList.Clear ();
		batteryList.Clear ();


		Destroy (transform.GetComponent<LevelHandle> ());

	}

}



