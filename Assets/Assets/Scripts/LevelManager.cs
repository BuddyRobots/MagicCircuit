using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;



public class LevelManager : MonoBehaviour 
{
	//管理类----从文件中读取信息，初始化关卡列表信息
	public static LevelManager _instance;

	public List<LevelItemData> levelItemDataList = new List<LevelItemData>();


	public delegate void OnLevelChangeEvent();
//	public event OnLevelChangeEvent OnLevelChange;
	public  Dictionary<int,int> pList=new Dictionary<int,int>();


	//private GameObject levelSelectPanel;

	////json字符串，保存关卡的信息
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
           			""progress"":2,
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
		//Debug.Log ("path is :"+Application.persistentDataPath);


	}

	void Start() 
	{
		parseLevelItemInfo();
		loadLocalLevelProgressData ();



	}

	//加载本地已经完成的关卡
	public void loadLocalLevelProgressData()
	{

		int levelID = 0;
		int levelPro = 0;
		if (PlayerPrefs.HasKey ("LevelID")) {
		
			levelID = PlayerPrefs.GetInt ("LevelID");
			//levelID = PlayerPrefs.SetInt ("LevelID1",2);
			//levelID = PlayerPrefs.SetInt ("LevelID",2);
		}
		if (PlayerPrefs.HasKey ("LevelProgress")) {

			levelPro = PlayerPrefs.GetInt ("LevelProgress");
		}

		if(pList.ContainsKey(levelID)==false)
		{
			pList.Add (levelID, levelPro);

		}
		//获取到已完成的关卡后需要更新list数据
		//updateLevelItemDataList (levelID,levelPro);

	}

	//更新关卡数据信息
	public void updateLevelItemDataList(int levelID,int levelPro)
	{
		/*int levelID = 0;
		int levelPro = 0;
		foreach (var dic in pList) 
		{
			levelID = dic.Key;//这是已经完成的最高关卡
			levelPro = dic.Value;//关卡的进度

		}*/

		//修改已完成的最高关卡之前的所有关卡进度---都是已完成
		for(int i=0;i<=levelID;++i)
		{

			levelItemDataList [i].Progress = LevelProgress.Done;

		}
		//设置当前要完成的关卡进度
		levelItemDataList[levelID+1].Progress=LevelProgress.Doing;

		//设置等待完成的关卡进度
		//由于初始化数据的时候默认所有关卡的进度都是Todo,所以这段代码可以省略
		/*
		for (int j = levelID + 2; j < levelItemDataList.Count; ++j) {
			levelItemDataList[j].Progress=LevelProgress.Todo;
		
		}
		*/


	}

	/// <summary>
	/// 解析json字符串，并把信息存到levelitemdata里面
	/// </summary>
	public void parseLevelItemInfo()
	{
		JsonData jd = JsonMapper.ToObject(leveljsonstr);   
		JsonData jdLevelItems = jd["levelData"]; 

		if (jdLevelItems.IsArray) 
		for (int i = 0; i < jdLevelItems.Count; i++)
		{
			//Debug.Log("LevelID = " + (int)jdLevelItems[i]["levelID"]);
			//Debug.Log("LevelName = " + jdLevelItems[i]["levelName"]);
			//Debug.Log("LevelDescription = " + jdLevelItems[i]["levelDescription"]);  

			LevelItemData levelItemData = new LevelItemData ();

			levelItemData.LevelID = (int)jdLevelItems [i] ["levelID"];
				//Debug.Log("levelID = " + levelItemData.LevelNumber);
			levelItemData.LevelName = (string)jdLevelItems [i] ["levelName"];

			levelItemData.LevelDescription = (string)jdLevelItems [i] ["levelDescription"];
			levelItemData.IconName="icon";
				levelItemData.Progress = (LevelProgress)((int)jdLevelItems [i] ["progress"]);
				//Debug.Log("Progress = " + levelItemData.Progress);
				levelItemData.PrelevelID = (int)jdLevelItems [i] ["preLevelID"];
				levelItemData.NextLevelID = (int)jdLevelItems [i] ["nextLevelID"];
			levelItemDataList.Add (levelItemData);
				//Debug.Log("0000000"+levelItemDataList.Count);

		}


		foreach(LevelItemData item in levelItemDataList){
			//Debug.Log("LevelID = " + item.LevelID);

		}
		//OnLevelChange ();
		//LevelUI._instance.UpdateShow();
		//LevelSelectPanel._instance.refreshLevelUI();
	}

	/// <summary>
	/// 根据关卡数字获取关卡数据
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


}
