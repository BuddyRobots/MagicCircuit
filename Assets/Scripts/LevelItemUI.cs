﻿using UnityEngine;
using System.Collections;

/// <summary>
/// 该类负责控制关卡按钮
/// </summary>
public class LevelItemUI : MonoBehaviour {


	public LevelItemData data;

	private GameObject levelSelectPanel;
	private GameObject levelDescriptionPanel;


	void Awake()
	{
		levelSelectPanel=GameObject.Find("UI Root/LevelSelectPanel");
		levelDescriptionPanel =transform.parent.parent.parent.parent.Find("DescriptionPanel").gameObject;
	}
		

	//初始化item（背景图片+关卡等级数字）
//	public void InitLevelItemUI(LevelItemData data)
//	{
//		
//		this.data = data;
//		Sprite.spriteName = data.IconName;
//		//Label.text = data.LevelNumber.ToString ();
//
//
//	}
		

	//如果对象有collider，可以这样实现点击事件
	public void OnClick()
	{
		levelDescriptionPanel.SetActive (true);

		//得到关卡数字
		int levelID = GetLevel (this.name);
		data = LevelManager._instance.GetSingleLevelItem (levelID);//根据关卡数字拿到关卡数据


		if (data!=null) 
		{
			DescriptionPanel._instance.Show (data);//根据拿到的数据进行显示
		}

		LevelManager._instance.SetCurrentLevel(data);//保存当前关卡信息

		levelSelectPanel.SetActive (false);
	}

	/// <summary>
	/// 获取关卡
	/// </summary>
	/// <returns>返回关卡数字</returns>
	/// <param name="levelName">参数是“LevelD01”类型的字符串</param>
	/// 如果返回0则表示解析错误；
	public int GetLevel(string levelName)
	{
		int ret = 0;

		string temp =levelName.Substring (6);
		if (temp [0] == 0)
		{

			string temp2 = temp.Substring (1);
			int.TryParse (temp2, out ret);
		
		} 
		else
		{
			int.TryParse (temp, out ret);
		}
		return ret;

	}
}
