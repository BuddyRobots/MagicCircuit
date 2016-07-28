using UnityEngine;
using System.Collections;



	public enum WindowID
	{
		

	//开始界面，选关界面，关卡描述界面，拍照界面，识别界面，电路动画播放界面（识别成功界面），识别失败界面，
	//操作演示界面，主页界面，声音设置界面，帮助界面，开发者信息界面

	WindowID_Start=0,
	LevelSelect,
	LevelDescription,
	PhotoTaking,
	PhotoTaken,
	PhotoRecognizing,// 静态图片     
	PhotoRecognized, // 卡通符号注意出现在相应位置，背景渐变至原照片呗完全覆盖
	AnimationPlay,
	WellDone,
	Failed,
	DemoShow,
	MusicSet,
	Help,
	DevelopInfo


//		WindowID_Invaild = 0,
//		WindowID_Rank,          // 排行榜界面
//		WindowID_Rank_Detail,   // 排行榜详情界面
//		WindowID_Rank_OwnDetail,
//		WindowID_Level,
//		WindowID_LevelDetail,
//		WindowID_Matching,
//		WindowID_MatchResult,
//		WindowID_Skill,
//		WindowID_MainMenu,
//		WindowID_TopBar,
//		WindowID_MessageBox,
	}

	public enum UIWindowType
	{
	Normal,
	Fixed,
	PopUp
//		Normal,    // 可推出界面(UIMainMenu,UIRank等)
//		Fixed,     // 固定窗口(UITopBar等) 
//		PopUp,     // 模式窗口
	}

	public enum UIWindowShowMode
	{
		DoNothing,        
		HideOther,     // 闭其他界面
		NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
		NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
	}

	public enum UIWindowColliderMode
	{
		None,      // 显示该界面不包含碰撞背景
		Normal,    // 碰撞透明背景
		WithBg,    // 碰撞非透明背景
	}
	
	public class UIResourceDefine
	{
	
//		public static Dictionary<WindowID, string> windowPrefabPath = new Dictionary<WindowID, string>() 
//		{ 
//			{ WindowID.WindowID_Rank, "UIRank" },
//			{ WindowID.WindowID_Level, "UILevelWindow" },
//			{ WindowID.WindowID_MainMenu, "UIMainMenu" },
//			{ WindowID.WindowID_TopBar, "UITopBar" },
//			{ WindowID.WindowID_MessageBox, "UIMessageBox" },
//			{ WindowID.WindowID_LevelDetail, "UILevelDetailWindow" },
//			{ WindowID.WindowID_Matching, "UIMatching" },
//			{ WindowID.WindowID_MatchResult, "UIMatchResult" },
//			{ WindowID.WindowID_Skill, "UISkill" }
//		};

		public static string UIPrefabPath = "UIPrefab/";
	}

