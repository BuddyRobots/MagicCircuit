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



	////json字符串
	string leveljsonstr = @"
            {
 				""levelData"":[
				{
           			""levelID"": 1,
           			""levelName"": ""level_1"",
           			""levelDescription"":  ""level_1 is so easy"" 
				},
				{
           			""levelID"": 2,
           			""levelName"": ""Level_2"",
           			""levelDescription"":  ""Level_2 is a little bit hard,just have a try!"" 
				},
				{
           			""levelID"": 3,
           			""levelName"": ""Level_3"",
           			""levelDescription"":  ""Level_3 is a little bit hard,just have a try!"" 
				},
				{
           			""levelID"": 4,
           			""levelName"": ""Level_4"",
           			""levelDescription"":  ""Level_4 is a little bit hard,just have a try!"" 
				},
				{
           			""levelID"": 5,
           			""levelName"": ""Level_5"",
           			""levelDescription"":  ""Level_5 is a little bit hard,just have a try!"" 
				},
				{
           			""levelID"": 6,
           			""levelName"": ""Level_6"",
           			""levelDescription"":  ""Level_6 is a little bit hard,just have a try!"" 
				},
				{
           			""levelID"": 7,
           			""levelName"": ""Level_7"",
           			""levelDescription"":  ""Level_7 is a little bit hard,just have a try!"" 
				},
				{
           			""levelID"": 8,
           			""levelName"": ""Level_8"",
           			""levelDescription"":  ""Level_8 is a little bit hard,just have a try!"" 
				},
				{
           			""levelID"": 9,
           			""levelName"": ""Level_9"",
           			""levelDescription"":  ""Level_9 is a little bit hard,just have a try!"" 
				}
				

				]
            }";

	
	void Awake() 
	{
		_instance = this;
		Debug.Log ("path is :"+Application.persistentDataPath);

	}

	void Start() 
	{
		parseLevelItemInfo();

	}

	//解析json字符串，并把信息存到levelitemdata里面，有几个就存几个
	public void parseLevelItemInfo()
	{
		JsonData jd = JsonMapper.ToObject(leveljsonstr);   
		JsonData jdLevelItems = jd["levelData"]; 

		if (jdLevelItems.IsArray) 
		for (int i = 0; i < jdLevelItems.Count; i++)
		{
			Debug.Log("LevelID = " + (int)jdLevelItems[i]["levelID"]);
			Debug.Log("LevelName = " + jdLevelItems[i]["levelName"]);
			Debug.Log("LevelDescription = " + jdLevelItems[i]["levelDescription"]);  

			LevelItemData levelItemData = new LevelItemData ();

			levelItemData.LevelNumber = (int)jdLevelItems [i] ["levelID"];
			levelItemData.LevelName = (string)jdLevelItems [i] ["levelName"];

			levelItemData.LevelDescription = (string)jdLevelItems [i] ["levelDescription"];
			levelItemData.IconName="icon";
			levelItemDataList.Add (levelItemData);

		}
			
		//OnLevelChange ();
		//LevelUI._instance.UpdateShow();

	}
}
