using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoDataManager: SceneSinglton<DemoDataManager>
{


	public Dictionary<int, List<string> > mDicDemoLevelPic = new Dictionary<int, List<string> >();


	void Awake(){
		//1st  Level pic data
		List<string> level_1_pic_data = new List<string>();

//		for(int i = 0; i < 4; i++)
//		{
//
//			level_1_pic_data.Add("level_1_pic_data_" + i + ".png");
//
//		}
//		mDicDemoLevelPic.Add(1,level_1_pic_data);



		string name1="music0";
		string name2="music10";
		string name3="music20";
		level_1_pic_data.Add(name1);
		level_1_pic_data.Add(name2);
		level_1_pic_data.Add(name3);


		mDicDemoLevelPic.Add(1,level_1_pic_data);


	}







}
