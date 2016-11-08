using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;


/// <summary>
///管理类----从文件中读取信息，初始化关卡列表信息
///该脚本挂在camera上面，游戏一开始就运行（初始化数据，加载本地关卡数据，等等）
/// </summary>
public class LevelManager : MonoBehaviour 
{	
	public static LevelManager _instance;

	/// <summary>
	/// 存储关卡数据的集合
	/// </summary>
	public List<LevelItemData> levelItemDataList = new List<LevelItemData>();

	public static LevelItemData currentLevelData;//这个数据可以被failurePanel，photoTakingPanel,photoRecognizingPanel 拿去获取当前关卡名字


	//json字符串，保存关卡的信息（这里的信息字段名和levelItemData里的属性保持一致）
	string leveljsonstr = @"
            {
 				""levelData"":[
				{
           			""levelID"": 1,
           			""levelName"": ""level_1"",
           			""levelDescription"":  ""level_1 is so easy"" ,
           			""progress"":1,
           			""preLevelID"": 0,
           			""nextLevelID"": 2

				},
				{
           			""levelID"": 2,
           			""levelName"": ""Level_2"",
           			""levelDescription"":  ""Level_2 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 1,
           			""nextLevelID"": 3
				},
				{
           			""levelID"": 3,
           			""levelName"": ""Level_3"",
           			""levelDescription"":  ""Level_3 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 2,
           			""nextLevelID"": 4
				},
				{
           			""levelID"": 4,
           			""levelName"": ""Level_4"",
           			""levelDescription"":  ""Level_4 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 3,
           			""nextLevelID"": 5
				},
				{
           			""levelID"": 5,
           			""levelName"": ""Level_5"",
           			""levelDescription"":  ""Level_5 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 4,
           			""nextLevelID"": 6
				},
				{
           			""levelID"": 6,
           			""levelName"": ""Level_6"",
           			""levelDescription"":  ""Level_6 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 5,
           			""nextLevelID"": 7
				},
				{
           			""levelID"": 7,
           			""levelName"": ""Level_7"",
           			""levelDescription"":  ""Level_7 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 6,
           			""nextLevelID"": 8
				},
				{
           			""levelID"": 8,
           			""levelName"": ""Level_8"",
           			""levelDescription"":  ""Level_8 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 7,
           			""nextLevelID"": 9
				},
				{
           			""levelID"": 9,
           			""levelName"": ""Level_9"",
           			""levelDescription"":  ""Level_9 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 8,
           			""nextLevelID"": 10
				},
				{
           			""levelID"": 10,
           			""levelName"": ""Level_10"",
           			""levelDescription"":  ""Level_10 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 9,
           			""nextLevelID"": 11
				},
				{
           			""levelID"": 11,
           			""levelName"": ""Level_11"",
           			""levelDescription"":  ""Level_11 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 10,
           			""nextLevelID"": 12
				},
				{
           			""levelID"": 12,
           			""levelName"": ""Level_12"",
           			""levelDescription"":  ""Level_12 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 11,
           			""nextLevelID"": 13
				},
				{
           			""levelID"": 13,
           			""levelName"": ""Level_13"",
           			""levelDescription"":  ""Level_13 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 12,
           			""nextLevelID"": 14
				},
				{
           			""levelID"": 14,
           			""levelName"": ""Level_14"",
           			""levelDescription"":  ""Level_14 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 13,
           			""nextLevelID"": 15
				},
				{
           			""levelID"": 15,
           			""levelName"": ""Level_15"",
           			""levelDescription"":  ""Level_15 is a little bit hard,just have a try!"" ,
           			""progress"":0,
           			""preLevelID"": 14,
           			""nextLevelID"": 16
				}
				

				]
            }";

	
	void Awake()
	{
		_instance = this;

		//code for test...
		PlayerPrefs.SetInt ("LevelID",1);
		PlayerPrefs.SetInt ("LevelProgress",2);
	}

	void Start() 
	{
		ParseLevelItemInfo();
		LoadLocalLevelProgressData ();
//		initNeedShowHandData();
	}

	/*
	void initNeedShowHandData()
	{
		PlayerPrefs.SetInt ("switchItem", 0);
		if (!PlayerPrefs.HasKey("switchItem")) 
		{
			PlayerPrefs.SetInt ("switchItem", 0);
		}
	}
	*/

	/// <summary>
	/// 加载本地已经完成的关卡
	/// </summary>
	public void LoadLocalLevelProgressData()
	{
		int levelID = 0;
		int levelPro = 0;
		if (PlayerPrefs.HasKey ("LevelID")) {
			//如果本地存储中有LevelID这个字段，表示玩家有闯关记录，则需要去拿到这个数据
			levelID = PlayerPrefs.GetInt ("LevelID");
			//Debug.Log ("levelID==" + levelID);
		} else {
			//如果没有，就创建这样一个ID
			PlayerPrefs.SetInt ("LevelID", 0);
			levelID = PlayerPrefs.GetInt ("LevelID");

		}
		if (PlayerPrefs.HasKey ("LevelProgress")) {

			levelPro = PlayerPrefs.GetInt ("LevelProgress");
			//Debug.Log ("levelPro==" + levelPro);
		} else {
		
			PlayerPrefs.SetInt ("LevelProgress", 0);
			levelPro = PlayerPrefs.GetInt ("LevelProgress");
//			Debug.Log ("levelPro2==" + levelPro);
		}
		//获取到已完成的关卡后需要更新list数据
		UpdateLevelItemDataList (levelID,levelPro);

	}

	/// <summary>
	/// 更新关卡数据信息
	/// </summary>
	/// <param name="levelID">关卡ID</param>
	/// <param name="levelPro">关卡进度</param>
	public void UpdateLevelItemDataList(int levelID,int levelPro)
	{
		//Debug.Log ("updateLevelItemDataList==");
		if (levelID == 0) //表示一关都没有玩过，是第一次玩
		{
			//Debug.Log ("updateLevelItemDataListAA");
			levelItemDataList [0].Progress = LevelProgress.Doing;
			for (int i = 1; i < levelItemDataList.Count; ++i) 
			{

				levelItemDataList [i].Progress = LevelProgress.Todo;

			}

		} 
		else 
		{	//有关卡记录，不是第一次玩
			//修改已完成的最高关卡之前的所有关卡进度---都是已完成
			for (int i = 0; i < levelID; ++i) 
			{

				levelItemDataList [i].Progress = LevelProgress.Done;

			}

			//设置当前要完成的关卡进度
			if (levelID < 15) 
			{
				/*if (levelID==11) 
				{
					Debug.Log ("level_12_Progress" + levelItemDataList [levelID].Progress);

				}*/
				levelItemDataList [levelID].Progress = LevelProgress.Doing;
			} 
			else 
			{
				levelItemDataList [levelID - 1].Progress = LevelProgress.Done;
			}

			//设置等待完成的关卡进度
			//由于初始化数据的时候默认所有关卡的进度都是Todo,所以这段代码可以省略
			/*
			for (int j = levelID + 2; j < levelItemDataList.Count; ++j) {
				levelItemDataList[j].Progress=LevelProgress.Todo;
			
			}
			*/
		}
	}

	/// <summary>
	/// 解析json字符串，并把信息存到levelitemdata里面
	/// </summary>
	public void ParseLevelItemInfo()
	{
		JsonData jd = JsonMapper.ToObject(leveljsonstr);   
		JsonData jdLevelItems = jd["levelData"]; 

		if (jdLevelItems.IsArray) 
		{
			for (int i = 0; i < jdLevelItems.Count; i++) 
			{
				LevelItemData levelItemData = new LevelItemData ();
				levelItemData.LevelID = (int)jdLevelItems [i] ["levelID"];
				levelItemData.LevelName = (string)jdLevelItems [i] ["levelName"];
				levelItemData.LevelDescription = (string)jdLevelItems [i] ["levelDescription"];
				levelItemData.Progress = (LevelProgress)((int)jdLevelItems [i] ["progress"]);
				levelItemData.PrelevelID = (int)jdLevelItems [i] ["preLevelID"];
				levelItemData.NextLevelID = (int)jdLevelItems [i] ["nextLevelID"];
				levelItemDataList.Add (levelItemData);

			}
		}
	}

	/// <summary>
	/// 根据关卡数字获取关卡数据,LevelManager提供这样一个获得关卡数据的方法，方便在关卡界面点击按钮时调用该方法来获取关卡数据
	/// </summary>
	/// <returns>返回一条关卡数据</returns>
	/// <param name="levelID">参数是关卡数字，也就是关卡ID</param>
	public LevelItemData GetSingleLevelItem(int levelID)
	{
		LevelItemData itemData = null;
		foreach (LevelItemData itemTemp in levelItemDataList)
		{
			if(itemTemp.LevelID==levelID)
			{
				itemData = itemTemp;
				break;

			}

		}
		return itemData;
	}
	/// <summary>
	/// 保存当前关卡数据，方便其他界面调用
	/// </summary>
	/// <param name="data">Data.</param>
	public void SetCurrentLevel(LevelItemData data)
	{
		currentLevelData = data;

	}
}
