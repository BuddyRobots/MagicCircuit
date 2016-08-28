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
	private GameObject loudspeaker;

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
	/// 电路是否通电的标志
	/// </summary>
	private bool isEnergized = false;


	/// <summary>
	/// 记录图标数量的信号量，为0时表示所有图标都显示完，可以显示箭头了
	/// </summary>
	private int iconCount = 1;

	private UISprite image;//拍摄截取的图像
	private UISprite mask;//遮盖背景图片的蒙板，通过改变透明度来显示拍摄的照片

	private float  animationTimer=0;//电流动画播放计时器
	private float animationTime=5f;//重玩按钮和下一步按钮在电流动画播放5秒后出现

	//private Transform arrowOccurPos;
	private LevelItemData data;

	private Vector3 fromPos;
	private Vector3 toPos;
	private Vector3 centerPos;

	private float distance = 0;
	private float angle = 0;

	private GameObject lineParent;
	private GameObject linePrefab;
	private GameObject fingerPrefab;


	private static float maskTimer = 0f;//蒙板渐变计时器
	private float maskTime;//蒙板渐变的总时间=所有item开始显示到显示完成的总时间

	/// <summary>
	/// 识别面板新创建出来的对象的集合（界面上新创建的对象需要在关闭面板的时候进行销毁，以保证重新打开界面时上一次操作中的新建的对象不会残留）
	/// </summary>
	private List<GameObject> goList=new List<GameObject>();

	List<GameObject> switchList = new List<GameObject> ();

	private List<CircuitItem> itemsList=new List<CircuitItem>();//图标的集合
	List< List<Vector3> > lines=new List<List<Vector3>>();//所有线条的集合
	public  List<GameObject> arrowList=new List<GameObject>();

	void Start()
	{
		helpBtn = transform.Find ("HelpBtn").GetComponent<UIButton> ().gameObject;
		commonPanel02 = transform.parent.Find ("CommonPanel02").gameObject;
		failurePanel=transform.parent.Find ("CommonPanel02/FailurePanel").gameObject;

		bulb = Resources.Load ("Bulb",typeof(GameObject))  as GameObject;
		battery = Resources.Load ("Battery",typeof(GameObject))  as GameObject;
		switchBtn = Resources.Load ("Switch",typeof(GameObject))  as GameObject;
		loudspeaker = Resources.Load ("Loudspeaker", typeof(GameObject)) as GameObject;
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
		mask=transform.Find("Bg/Mask").GetComponent<UISprite> ();
		replayBtn=transform.Find("ReplayBtn").GetComponent<UIButton> ().gameObject;
		nextBtn=transform.Find("NextBtn").GetComponent<UIButton> ().gameObject;
		labelBgTwinkle = transform.Find ("LevelNameBgT").GetComponent<UISprite> ().gameObject;

		lineParent = this.gameObject;

		image.gameObject.SetActive (false);
		mask.gameObject.SetActive (true);
		replayBtn.SetActive(false);
		nextBtn.SetActive (false);
		labelBgTwinkle.SetActive (false);

		isItemShowDone=false;
		isPhotoShowDone = false;
		isArrowShowDone=false;
		isCreate_Update=false;
		isEnergized = false;


		data = LevelManager.currentLevelData;

	    itemsList=CircuitItemManager._instance.itemList;  // for test

		//arrowOccurPos = gameObject.transform;

		prePos = Vector3.zero; 
		iconCount = 1;
		mask.alpha = 0;
		maskTime = itemsList.Count  * 1;//显示图标的总时间=图标个数*每个图标隔的时间
		foreach (var item in lines) 
		{
			maskTime += (float)((item.Count - 1) * 0.2);//显示一条线的总时间

		}

		StartCoroutine (PhotoShow ());//进入识别界面的第一步是显示拍摄的照片
	}
		
	void OnDisable()
	{
		if (finger) {
			Destroy (finger);
			finger = null;
		}
		switchList.Clear ();

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

				if(switchList.Count >0)//如果有开关，则需要显示小手
				{
					for (int i = 0; i < switchList.Count; i++) 
					{
						if (switchList[i].GetComponent<SwitchCtrl>().isSwitchOn) 
						{
							//isEnergized=false;
							int hasShownHand = PlayerPrefs.GetInt ("switchItem");
							//if(LevelManager._instance.GetSingleLevelItem().Progress == LevelProgress.Doing){
							//	ShowFinger(switchList[i].transform.localPosition);//显示小手，传入开关的位置
							//}
							if(hasShownHand == 0)
							{
								ShowFinger(switchList[i].transform.localPosition);//显示小手，传入开关的位置
								PlayerPrefs.SetInt ("switchItem",1);

							}
							break;
						}

						/*if (i == switchList.Count - 1) {
							Destroy(finger);
							isFingerShow=true;
							finger = null; 
						}*/

						if(switchList[i].GetComponent<SwitchCtrl>().isSwitchOn ==false)//如果点击了开关，开关闭合，就销毁小手
						{

							Destroy(finger);
							//isFingerShow=true;
							finger = null;
							if(i == switchList.Count - 1)//如果所有的开关都闭合了，就表示电路通电了，灯泡变亮，电流流动  to  do ...遍历灯泡，都变亮
							{
								isEnergized=true;
							}
						}
					}

				}
				if (isArrowShowDone == false && isEnergized) //false表示电流动画没有播放过，true表示已经播放了，保证动画只播放一次
				{
					//StartCoroutine (ArrowShow ());
					StartCoroutine (ArrowShowLineByLine1(lines , 0));
					isArrowShowDone = true;
				}


				//如果已经播放了电流，玩家又点击了开关，开关断开，应该隐藏所有的箭头，灯变暗；直到玩家再次点击开关，开关闭合，显示所有的箭头，灯变亮

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



	private Vector3 offSet = new Vector3 (113, -108, 0);
	private GameObject finger;
	private Vector3 prePos= Vector3.zero; // 记录当前指向的开关，如果没发生变化，不做任何操作

	void ShowFinger(Vector3 switchPos)
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
		



	//小手逻辑   假设可以点击的图标有  开关，双闸开关，   
	//规则是--(比如)第一关有开关（第一次出现），显示小手；第2关不显示小手；第三关有双闸开关（第一次出现），需要显示小手
	//如果所有图标都显示完了，且开关（在所有关卡中）是第一次出现---也可以用关卡数字来判断是不是第一次出现，就需要出现小手，








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
			//arrowOccurPos = item.transform;//箭头从电池的位置出来
			item.name = "battery"; 
			iconCount--;
			break;
	
		case ItemType.Bulb:
			item = GameObject.Instantiate (bulb) as GameObject;
			goList.Add (item);
			item.name = "bulb";
			iconCount--;
			break;

		case ItemType.Switch:
			//item = GameObject.Instantiate (switchOn, circuitItem.list[0], Quaternion.identity) as GameObject;
			item = GameObject.Instantiate (switchBtn) as GameObject;
			switchList.Add (item);
			goList.Add (item);
			item.name = "switch"; 
			iconCount--;
			break;

		case ItemType.Loudspeaker:
			//item = GameObject.Instantiate (switchOn, circuitItem.list[0], Quaternion.identity) as GameObject;
			item = GameObject.Instantiate (loudspeaker) as GameObject;
			goList.Add (item);
			item.name = "loudspeaker"; 
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

	IEnumerator ArrowShowLineByLine1(List<List<Vector3>> lines ,int i)//递归协同，三条线一起循环，但又是有顺序的
	{
		bool hasStartCorout = false;//有没有开启协同的标志
		GameObject temp = null;

		List<Vector3> singleLine = lines [i];

		for (int k = 0; ; k++) 
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
				StartCoroutine (ArrowShowLineByLine1 (lines, i + 1));
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
}



