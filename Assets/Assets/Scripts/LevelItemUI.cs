using UnityEngine;
using System.Collections;

public class LevelItemUI : MonoBehaviour {



	//界面上显示的每一个item的信息

	private UISprite levelItemBg;//背景图片
	//private UILabel levelNumLabel;//等级数字
	public LevelItemData data;//item数据


	//private UIButton btn;
	private GameObject levelSelectPanel;
	private GameObject levelDescriptionPanel;

	private UISprite Sprite {
		get {
			if (levelItemBg == null) {
				//levelItemBg = transform.Find("LevelBtn").GetComponent<UISprite>();
				levelItemBg = transform.GetComponent<UISprite>();
			}
			return levelItemBg;
		}
	}
//	private UILabel Label {
//		get {
//			if (levelNumLabel == null) {
//				levelNumLabel = transform.Find("Label").GetComponent<UILabel>();
//			}
//			return levelNumLabel;
//		}
//	}


	void Awake()
	{
		//btn = transform.GetComponent<UIButton> ();
		//UIEventListener.Get (button).onClick = OnBtnClick;
		levelSelectPanel=GameObject.Find("UI Root/LevelSelectPanel");
		//levelDescriptionPanel = GameObject.Find ("UI Root/DescriptionPanel");
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
		Debug.Log ("levelBtn clicked: "+ this.name);
		levelSelectPanel.SetActive (false);
		levelDescriptionPanel.SetActive (true);

		//得到关卡数字
		int levelID = GetLevel (this.name);
		data = LevelManager._instance.GetSingleLevelItem (levelID);

		//这里需要把等级数字传递过去
		if (data!=null) 
		{
			//object[] objectArray = new object[2];
			//objectArray[0] = data;//这里可以把整个item的数据都传过去，方便后面使用，不要只传等级数字
			//objectArray[1] = this;
			DescriptionPanel._instance.Show (data);
			Debug.Log(DescriptionPanel._instance);
		}

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
		Debug.Log ("ret=="+ret);
		return ret;

	}
}
