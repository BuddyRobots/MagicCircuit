using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoDataManager: SceneSinglton<DemoDataManager>
{

	public Dictionary<int, List<string> > mDicDemoLevelPic = new Dictionary<int, List<string> >();


//	public List<UITexture> startDemoTex;
	public List<UITexture> level_1_DemoTex;



	void Awake()
	{

		List<string> startShow_pic_data = new List<string>();//startShow pic data
		List<string> level_1_pic_data = new List<string>();//1st  Level pic data
		List<string> level_2_pic_data = new List<string>();
		List<string> level_3_pic_data = new List<string>();
		List<string> level_4_pic_data = new List<string>();
		List<string> level_5_pic_data = new List<string>();
		List<string> level_6_pic_data = new List<string>();
		List<string> level_7_pic_data = new List<string>();
		List<string> level_8_pic_data = new List<string>();
		List<string> level_9_pic_data = new List<string>();
		List<string> level_10_pic_data = new List<string>();
		List<string> level_11_pic_data = new List<string>();
		List<string> level_12_pic_data = new List<string>();
		List<string> level_13_pic_data = new List<string>();
		List<string> level_14_pic_data = new List<string>();
		List<string> level_15_pic_data = new List<string>();

		List<int> demoPicNumList=new List<int>();

		int num_0=3;//开始demo张数
		int num_1=3;
		int num_2=3;
		int num_3=4;
		int num_4=4;
		int num_5=4;
		int num_6=4;
		int num_7=4;
		int num_8=4;
		int num_9=4;
		int num_10=3;
		int num_11=4;
		int num_12=4;
		int num_13=5;
		int num_14=5;
		int num_15=6;


		demoPicNumList.Add(num_0);
		demoPicNumList.Add(num_1);
		demoPicNumList.Add(num_2);
		demoPicNumList.Add(num_3);
		demoPicNumList.Add(num_4);
		demoPicNumList.Add(num_5);
		demoPicNumList.Add(num_6);
		demoPicNumList.Add(num_7);
		demoPicNumList.Add(num_8);
		demoPicNumList.Add(num_9);
		demoPicNumList.Add(num_10);
		demoPicNumList.Add(num_11);
		demoPicNumList.Add(num_12);
		demoPicNumList.Add(num_13);
		demoPicNumList.Add(num_14);
		demoPicNumList.Add(num_15);

		for (int i = 0; i < num_0; i++) 
		{
			string startDemoPicName="Level_0_"+i;//to do.....
			startShow_pic_data.Add(startDemoPicName);
		}
		for (int i = 0; i < num_1; i++) 
		{

			string level_1_demoPicName="Level_0_"+i;
			level_1_pic_data.Add(level_1_demoPicName);
		}
		for (int i = 0; i < num_2; i++) 
		{

			string level_2_demoPicName="Level_1_"+i;
			level_2_pic_data.Add(level_2_demoPicName);
		}
		for (int i = 0; i < num_3; i++) 
		{

			string level_3_demoPicName="Level_2_"+i;
			level_3_pic_data.Add(level_3_demoPicName);
		}
		for (int i = 0; i < num_4; i++) 
		{

			string level_4_demoPicName="Level_3_"+i;
			level_4_pic_data.Add(level_4_demoPicName);
		}
		for (int i = 0; i < num_5; i++) 
		{

			string level_5_demoPicName="Level_4_"+i;
			level_5_pic_data.Add(level_5_demoPicName);
		}
		for (int i = 0; i < num_6; i++) 
		{

			string level_6_demoPicName="Level_5_"+i;
			level_6_pic_data.Add(level_6_demoPicName);
		}
		for (int i = 0; i < num_7; i++) 
		{

			string level_7_demoPicName="Level_6_"+i;
			level_7_pic_data.Add(level_7_demoPicName);
		}
		for (int i = 0; i < num_8; i++) 
		{

			string level_8_demoPicName="Level_7_"+i;
			level_8_pic_data.Add(level_8_demoPicName);
		}
		for (int i = 0; i < num_9; i++) 
		{

			string level_9_demoPicName="Level_8_"+i;
			level_9_pic_data.Add(level_9_demoPicName);
		}
		for (int i = 0; i < num_10; i++) 
		{

			string level_10_demoPicName="Level_9_"+i;
			level_10_pic_data.Add(level_10_demoPicName);
		}
		for (int i = 0; i < num_11; i++) 
		{

			string level_11_demoPicName="Level_10_"+i;
			level_11_pic_data.Add(level_11_demoPicName);
		}
		for (int i = 0; i < num_12; i++) 
		{

			string level_12_demoPicName="Level_11_"+i;
			level_12_pic_data.Add(level_12_demoPicName);
		}
		for (int i = 0; i < num_13; i++) 
		{

			string level_13_demoPicName="Level_12_"+i;
			level_13_pic_data.Add(level_13_demoPicName);
		}

		for (int i = 0; i < num_14; i++) 
		{

			string level_14_demoPicName="Level_13_"+i;
			level_14_pic_data.Add(level_14_demoPicName);
		}

		for (int i = 0; i < num_15; i++) 
		{

			string level_15_demoPicName="Level_14_"+i;
			level_15_pic_data.Add(level_15_demoPicName);
		}
			
		mDicDemoLevelPic.Add(0,startShow_pic_data);
		mDicDemoLevelPic.Add(1,level_1_pic_data);
		mDicDemoLevelPic.Add(2,level_2_pic_data);
		mDicDemoLevelPic.Add(3,level_3_pic_data);
		mDicDemoLevelPic.Add(4,level_4_pic_data);
		mDicDemoLevelPic.Add(5,level_5_pic_data);
		mDicDemoLevelPic.Add(6,level_6_pic_data);
		mDicDemoLevelPic.Add(7,level_7_pic_data);
		mDicDemoLevelPic.Add(8,level_8_pic_data);
		mDicDemoLevelPic.Add(9,level_9_pic_data);
		mDicDemoLevelPic.Add(10,level_10_pic_data);
		mDicDemoLevelPic.Add(11,level_11_pic_data);
		mDicDemoLevelPic.Add(12,level_12_pic_data);
		mDicDemoLevelPic.Add(13,level_13_pic_data);
		mDicDemoLevelPic.Add(14,level_14_pic_data);
		mDicDemoLevelPic.Add(15,level_15_pic_data);
	}

}
