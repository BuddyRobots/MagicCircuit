using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

//该类用来读取和保存玩家信息
public static class PlayerData 
{
	//文件路径
	private static string filePath=Application.persistentDataPath+"/playerDataFile";
	private static string data = "";
	//自动初始化文件路径
	static PlayerData()
	{
		//若没有文件，创建一个文件
		if (!File.Exists (filePath)) 
		{
			StreamWriter sw = File.CreateText (filePath);
			sw.Close ();
		}
		else 
		{
			Flush ();
		
		}

	}


	//刷新数据
	public static void Flush()
	{
		StreamReader sr = File.OpenText (filePath);
		data = sr.ReadToEnd ();
		sr.Close ();

	}

	//需要有一个指标来判断关卡是否通关，可以用星星数来判断，根据传入的关卡来返回该关卡的星星数，如果返回的是0则表示没有通关
	public static int GetStar(int level)
	{
		int ret = 0;



		// to do...

		return ret;

	}

	//读取当前最高关卡，返回0代表一关都没有通过
	public static int GetMaxPassedLevel()
	{
		int max = 1;
		//查找字符串的位置，如果是-1，则不存在，
		while (data.IndexOf (max.ToString()) != -1) {

			max++;
		}
		return max - 1;//如果是0，则表示一关都没有过

	}


	//增加通关关卡，修改玩家数据
	public  static void AddLevelToPlayerData(int level)
	{
		string temp = level.ToString();
		data += temp;
		File.WriteAllText (filePath, data, Encoding.UTF8);
	}

}
