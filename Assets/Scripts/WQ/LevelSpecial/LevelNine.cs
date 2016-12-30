using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

// level 9-----并联电路，2个电池+3个开关+2个灯泡+1个音响+1个电磁炉（2个开关分别控制2个灯泡，音响，电磁炉；电池不可点击）
public class LevelNine : MonoBehaviour 
{
	public  static  LevelNine _instance;

	[HideInInspector]
	public bool isLevelNine = false;
	/// <summary>
	/// bool to judge is bulb circuit powered or not
	/// </summary>
	[HideInInspector]
	public bool curBulbPowerd=false;

	private bool isBulbCircuitAniPlayed=false;
	private GameObject clickBulb =null;
	private List<GameObject> bulbList = null;
	private bool isBulbAddComponent = false;
	/// <summary>
	/// 记录小手最多出现的次数，小于0时销毁小手
	/// </summary>
	private int singnal=2;
	/// <summary>
	/// 标记能被点击的灯泡是否是半透明，false是不透明，true为透明
	/// </summary>
	private bool preClickBulbSemiStatus=false;
	/// </summary>

	UISprite nonClickBatterySprite=null;



	void Awake()
	{
		_instance=this;
	}

	void OnEnable ()
	{
		curBulbPowerd=false;
		isLevelNine = false;
		isBulbCircuitAniPlayed=false;
		isBulbAddComponent = false;
		singnal=2;
		preClickBulbSemiStatus=false;
		clickBulb = null;
	}

	//电流播放一会后，在灯泡的位置出现小手提示灯泡是可以点击的，（两个灯泡要确保有一个是在工作的，只能有一个能被点击变成半透明），
	//点击灯泡后，小手消失，灯泡变成半透明，另外一个灯泡变得更亮；再点击透明灯泡，灯泡复原成不透明，另外一个灯泡变正常亮
	void Update () 
	{
		if (isLevelNine) 
		{
			bulbList = PhotoRecognizingPanel.Instance.bulbList;


			if (curBulbPowerd) //如果灯泡的线路是有电的
			{
				isBulbCircuitAniPlayed=true;
			}
				
			if (isBulbCircuitAniPlayed) //有灯泡的电路通了一次后灯泡才可以被点击
			{
				clickBulb = bulbList[1];//识别部分设定是ID为0的不能点击，为1的可以点击
				nonClickBatterySprite=bulbList[0].GetComponent<UISprite>();

				#region 给bulb添加脚本
				if (!isBulbAddComponent) //保证只给bulb加一次脚本
				{
					clickBulb.AddComponent<BoxCollider> ();
					clickBulb.GetComponent<BoxCollider>().center=new Vector3(0.4f,3.5f,0);
					clickBulb.GetComponent<BoxCollider>().size=new Vector3(72f,100f,0);
					
					clickBulb.AddComponent<UIButton> ();//给随机的灯泡添加button组件和BulbCtrl组件来实现点击事件
					clickBulb.AddComponent<BulbCtrl> ();
					isBulbAddComponent = true;
				}
				#endregion

				#region 点击灯泡	
				if (clickBulb.GetComponent<BulbCtrl> ().isSemiTrans) //1个灯泡变成半透明
				{ 
					if (curBulbPowerd) //如果灯泡线路有电
					{
						nonClickBatterySprite.spriteName="bulbSpark";
					} 
					else //如果灯泡线路没电
					{
						nonClickBatterySprite.spriteName="bulbOff";
					}
					clickBulb.GetComponent<UISprite>().spriteName="semiTransBulb";
					clickBulb.GetComponent<UISprite> ().depth = 1;//透明灯泡在电线下面显示，不遮挡电线和箭头
				} 
				else
				{
					if (curBulbPowerd) 
					{
						nonClickBatterySprite.spriteName="bulbOn";
						clickBulb.GetComponent<UISprite>().spriteName="bulbOn";
					}
					else
					{
						nonClickBatterySprite.spriteName="bulbOff";
						clickBulb.GetComponent<UISprite>().spriteName="bulbOff";
					}
					clickBulb.GetComponent<UISprite> ().depth = 4;
				}
				#endregion

				#region  小手逻辑
				//小手只出现两次的逻辑
				if (singnal <=0) 
				{
					if (PhotoRecognizingPanel.Instance.finger) 
					{
						Destroy (PhotoRecognizingPanel.Instance.finger);
					}
				} 
				else 
				{
					if (clickBulb) 
					{
						GetComponent<PhotoRecognizingPanel> ().ShowFinger (clickBulb.transform.localPosition);//show finger
						if (preClickBulbSemiStatus != clickBulb.GetComponent<BulbCtrl> ().isSemiTrans) 
						{
							singnal--;
							preClickBulbSemiStatus = clickBulb.GetComponent<BulbCtrl> ().isSemiTrans;
						}
					}
				}
				#endregion
			}
			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);

		}

	}


}
