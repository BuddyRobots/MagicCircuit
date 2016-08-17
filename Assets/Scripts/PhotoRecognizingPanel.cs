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

	//匹配结果
	[HideInInspector]
	public Result result;

	//  btns on the panel
	private GameObject helpBtn;
	private GameObject replayBtn;
	private GameObject nextBtn;

	private GameObject arrowPrefab;

	//  items need to show
	private GameObject bulb;
	private GameObject battery;
	private GameObject switchBtn;

	//需要跳转连接的界面
	private GameObject commonPanel02;
	private GameObject failurePanel;

	private GameObject labelBgTwinkle;//文字显示的发光背景

	private UILabel levelNameLabel;

	/// <summary>
	/// 判断图标是否显示完的标志
	/// </summary>
	private bool isItemShowDone=false;

	/// <summary>
	/// 判断拍摄的照片是否显示完成的标志
	/// </summary>
	private bool isPhotoShowDone = false;

	/// <summary>
	/// 判断箭头电流动画是否完成的标志
	/// </summary>
	private bool isArrowShowDone=false;

	/// <summary>
	/// 保证创建图标的协同只走一遍的标志
	/// </summary>
	private bool isCreate_Update=false;

	/// <summary>
	/// 记录图标数量的信号量，为0时表示所有图标都显示完，可以显示箭头了
	/// </summary>
	private int iconCount = 1;

	private UISprite image;//拍摄截取的图像
	private UISprite mask;//遮盖背景图片的蒙板，通过改变透明度来显示拍摄的照片

	private float  animationTimer=0;//电流动画播放计时器
	private float animationTime=5f;//重玩按钮和下一步按钮在电流动画播放5秒后出现

	private Transform arrowOccurPos;


	private LevelItemData data;

	private Vector3 fromPos;
	private Vector3 toPos;
	private Vector3 centerPos;

	private float distance = 0;
	private float angle = 0;

	private GameObject lineParent;
	private GameObject linePrefab;

	private static float maskTimer = 0f;//蒙板渐变计时器
	private float maskTime;//蒙板渐变的总时间=所有item开始显示到显示完成的总时间

	/// <summary>
	/// 识别面板新创建出来的对象的集合（界面上新创建的对象需要在关闭面板的时候进行销毁，以保证重新打开界面时上一次操作中的新建的对象不会残留）
	/// </summary>
	private List<GameObject> goList=new List<GameObject>();

	private List<CircuitItem> itemsList=new List<CircuitItem>();//图标的集合
	List< List<Vector3> > lines=new List<List<Vector3>>();//所有线条的集合
	public  List<GameObject> arrowList=new List<GameObject>();

	void Awake()
	{
		levelNameLabel = transform.Find ("LevelNameBg/Label").GetComponent<UILabel> ();

	}
		
	void OnEnable()
	{
		levelNameLabel.text = LevelManager.currentLevelData.LevelName;
		labelBgTwinkle = transform.Find ("LevelNameBgT").GetComponent<UISprite> ().gameObject;
		commonPanel02 = transform.parent.Find ("CommonPanel02").gameObject;
		failurePanel=transform.parent.Find ("CommonPanel02/FailurePanel").gameObject;

		helpBtn = transform.Find ("HelpBtn").GetComponent<UIButton> ().gameObject;
		replayBtn=transform.Find("ReplayBtn").GetComponent<UIButton> ().gameObject;
		nextBtn=transform.Find("NextBtn").GetComponent<UIButton> ().gameObject;

		bulb = Resources.Load ("Bulb",typeof(GameObject))  as GameObject;
		battery = Resources.Load ("Battery",typeof(GameObject))  as GameObject;
		switchBtn = Resources.Load ("Switch",typeof(GameObject))  as GameObject;
		arrowPrefab=Resources.Load("Arrow") as GameObject;
		linePrefab = Resources.Load ("lineNew") as GameObject;

		lineParent = this.gameObject;


		isItemShowDone=false;
		isPhotoShowDone = false;
		isArrowShowDone=false;
		isCreate_Update=false;
		iconCount = 1;
		result = Result.Success;//for test..
		data = LevelManager.currentLevelData;

		image = transform.Find ("Bg/Image").GetComponent<UISprite> ();//for test..调用图像识别部分的一个接口（该接口返回的是一个UITexture）
		mask=transform.Find("Bg/Mask").GetComponent<UISprite> ();
		image.gameObject.SetActive (false);
		mask.gameObject.SetActive (true);

		itemsList=CircuitItemManager._instance.itemList;
		arrowOccurPos = gameObject.transform;

		replayBtn.SetActive(false);
		nextBtn.SetActive (false);
		labelBgTwinkle.SetActive (false);

		mask.alpha = 0;
		maskTime = itemsList.Count  * 1;//显示图标的总时间=图标个数*每个图标隔的时间

		foreach (var item in lines) 
		{
			maskTime += (float)((item.Count - 1) * 0.2);//显示一条线的总时间

		}
		//Debug.Log("maskTime---------"+maskTime);

		UIEventListener.Get (helpBtn).onClick = OnHelpBtnClick;
		UIEventListener.Get (replayBtn).onClick = OnReplayBtnClick;
		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;
		//循环10次调用图像识别的接口，然后再显示背景图片  ---或者放到拍摄界面的确认按钮点击里面
		// to do ...
		StartCoroutine (PhotoShow ());//进入识别界面的第一步是显示拍摄的照片


	}
		
	/// <summary>
	/// 显示拍摄的图片
	/// </summary>
	/// <returns>The show.</returns>
	IEnumerator PhotoShow()
	{
		image.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		isPhotoShowDone = true;
	}
		
	void Update () 
	{
		if (isPhotoShowDone) 
		{
			#region 背景图片显示2秒后蒙板出现，透明度渐变，同时开始创建图标
			maskTimer += Time.deltaTime;
			if (maskTimer >= maskTime) 
			{
				maskTimer =maskTime;
			}
			mask.alpha = Mathf.Lerp (0, 1f, maskTimer/maskTime);

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
			#endregion

			#region  图标创建完成，如果结果是正确的，就播放电流
			//如果所有图标都显示完了，且匹配成功，就播放动画，跳转到welldone界面
			if (isItemShowDone && result == Result.Success) 
			{

				//如果图标显示完成且匹配成功，就在开关右下角出现小手，指向开关位置提示玩家点击开关，玩家点击开关闭合后，如果还有其他开关，则小手移动的下一个开关的右下角。。。
				//依次把所有的开关都点击闭合后，小手消失，播放电流
				// to do ...


				if (isArrowShowDone == false) //false表示电流动画没有播放过，true表示已经播放了，保证动画只播放一次
				{
					StartCoroutine (ArrowShow01 ());
					isArrowShowDone = true;
				}
			}

			#endregion

			#region 如果结果是错误的，就跳转到失败界面
			else if (isItemShowDone && result == Result.Fail) 
			{
				Fail ();
			}
			#endregion
		}

	}

	/// <summary>
	/// 创建所有的图标
	/// </summary>
	IEnumerator CreateAllItem()
	{
		iconCount = itemsList.Count;
		for (int i = 0; i < itemsList.Count; i++) 
		{
			//isCreate = false;//只要进来了就说明线没画完
			CreateSingleItem (itemsList [i]);
			yield return new WaitForSeconds (1);//隔一秒创建一个图标
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
			//item = GameObject.Instantiate (battery, circuitItem.list [0], Quaternion.identity) as GameObject;
			item = GameObject.Instantiate (battery) as GameObject;

			goList.Add (item);//新创建一个对象的同时把这个对象加入到对象列表，方便关闭界面的时候销毁这些新创建的对象
			arrowOccurPos = item.transform;//箭头从电池的位置出来
			item.name = "battery"; 

			iconCount--;
			break;

		case ItemType.Bulb:
			//item = GameObject.Instantiate (bulb, circuitItem.list [0], Quaternion.identity) as GameObject;
			item = GameObject.Instantiate (bulb) as GameObject;
			goList.Add (item);
			item.name = "bulb";
			iconCount--;
			break;

		case ItemType.Switch:
			//item = GameObject.Instantiate (switchOn, circuitItem.list[0], Quaternion.identity) as GameObject;
			item = GameObject.Instantiate (switchBtn) as GameObject;
			goList.Add (item);
			item.name = "switch"; 
			iconCount--;
			break;

		case ItemType.CircuitLine:
			//如果是线路，则加入线路列表中，方便计算所有图标创建完的总时间
			lines.Add (circuitItem.list);
			//开始画线
			StartCoroutine(DrawCircuit(circuitItem.list));
			break;
		default:
			break;

		}
		if (item!=null) 
		{
			item.transform.parent = transform;// 将自己的父物体设置成“PhotoRecognizingPanel”
			//item.transform.position = circuitItem.list [0]; //我这里测试用写的坐标是根据transform.positon标志的，所以用transform.position来接收
			item.transform.localPosition = circuitItem.list [0];// 如果测试用的坐标是根据localPosition设定的，就要用localPosition来接收
			item.transform.localScale = new Vector3 (1, 1, 1); 
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
		lineGo.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);//旋转连线图片  
	}
		



	IEnumerator ArrowShow01()
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
			arrow.transform.localPosition =arrowOccurPos.localPosition;
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
			LevelManager._instance.loadLocalLevelProgressData ();
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
}



