using UnityEngine;
using System.Collections;

public class LevelItemData 
{
	//这是保存关卡的信息类，包括每一关的ID,关卡名字，描述文字
	private int m_LevelID;
	private int m_LevelNumber;//关卡数字
	private string m_LevelName;//关卡名字
	private string m_IconName;//背景图片在图集中的名字
	private string m_LevelDescription;//关卡描述

	public int LevelID
	{

		get
		{ 
			return m_LevelID;
		}
		set
		{ 
			m_LevelID = value;
		}

	}
	public int LevelNumber
	{
		get
		{ 
			return m_LevelNumber;
		}
		set
		{ 
			m_LevelNumber = value;
		}
	}

	public string IconName
	{

		get
		{ 
			return m_IconName;
		}
		set
		{ 
			m_IconName = value;
		}
	}

	public string LevelName
	{
		get
		{ 
			return m_LevelName;
		}
		set
		{
			m_LevelName = value;
		}
	}
	public string LevelDescription
	{
		get
		{ 
			return m_LevelDescription;
		}
		set
		{ 
			m_LevelDescription = value;
		}
	}




}
