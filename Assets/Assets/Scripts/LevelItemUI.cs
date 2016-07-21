using UnityEngine;
using System.Collections;

public class LevelItemUI : MonoBehaviour {



	//界面上显示的每一个item的信息

	private UISprite levelItemBg;//背景图片
	private UILabel levelNumLabel;//等级数字
	public LevelItemData data;//item数据
	private UIButton btn;

	private UISprite Sprite {
		get {
			if (levelItemBg == null) {
				//levelItemBg = transform.Find("LevelBtn").GetComponent<UISprite>();
				levelItemBg = transform.GetComponent<UISprite>();
			}
			return levelItemBg;
		}
	}
	private UILabel Label {
		get {
			if (levelNumLabel == null) {
				levelNumLabel = transform.Find("Label").GetComponent<UILabel>();
			}
			return levelNumLabel;
		}
	}


	void Awake()
	{
		btn = transform.GetComponent<UIButton> ();
		//UIEventListener.Get (button).onClick = OnBtnClick;
	

	}
		

	//初始化item（背景图片+关卡等级数字）
	public void InitLevelItemUI(LevelItemData data)
	{
		//Debug.Log ("InitLevelItemUI");
		//Debug.Log ("data==" + data.LevelDescription + " " + data.LevelNumber);
		this.data = data;
		//Debug.Log(" data.LevelNumber.ToString==" +  data.LevelNumber.ToString ());
		Sprite.spriteName = data.IconName;
		Label.text = data.LevelNumber.ToString ();
		//Sprite.spriteName = data.IconName;


	}

	//如果item添加了EventListener组建，可以用这种方法来实现按钮的点击事件
//	public void OnBtnClick(GameObject go)
//	{
//		Debug.Log ("levelBtn clicked");
//		//这里需要把等级数字传递过去
//		if (data!=null) {
//			object[] objectArray = new object[3];
//			objectArray[0] = data;//这里可以把整个item的数据都传过去，方便后面使用，不要只传等级数字
//			Debug.Log ("OnClick__data=="+ data.LevelNumber+ " " + data.LevelDescription );
//			GameObject.Find("CommonPanel01").SendMessage("OnLevelItemClick", objectArray);
//		}
//
//
//		LevelUI._instance.PanelOff ();
//
//	}

	//如果对象有collider，可以这样实现点击事件
	public void OnClick()
	{
		Debug.Log ("levelBtn clicked");
		//这里需要把等级数字传递过去
		if (data!=null) {
			object[] objectArray = new object[3];
			objectArray[0] = data;//这里可以把整个item的数据都传过去，方便后面使用，不要只传等级数字
			objectArray[1] = this;
			//Debug.Log ("OnClick__data=="+ data.LevelNumber+ " " + data.LevelDescription );
			transform.parent.parent.parent.parent.SendMessage("OnLevelItemClick", objectArray);//把commonpanel01作为传输数据的中间模块
		}


		LevelUI._instance.PanelOff ();

	}


}
