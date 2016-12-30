using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

//level 7-----并联电路，2个电池+2个开关+1个灯泡+1个音响（两个开关分别控制灯泡和音响），电路通电流后有一个电池可以被点击（在透明和半透明之间切换）
public class LevelSeven : MonoBehaviour 
{

	public static LevelSeven _instance;
	[HideInInspector]
	public bool isLevelSeven=false;
	[HideInInspector]
	public bool isTwoBatteryWork=true;

	[HideInInspector]
	public bool curSwitchPowered=false;

	private bool isCircuitAnimationPlayed=false;
	private List<GameObject> batteryList = null;
	private GameObject clickBattery =null;
	private bool isBatteryAddComponent = false;
	/// <summary>
	/// 标记小手最多出现的次数，小于0时销毁
	/// </summary>
	private int singnal = 2;
	private bool preClickBatterySemiStatus = false;

	private bool preBatterySemi=false;
	private bool curBatterySemi=false;

	BatteryCtrl batteryCtrl=null;

	void Awake()
	{
		_instance=this;
	}
	void OnEnable () 
	{
		isTwoBatteryWork=true;
		curSwitchPowered=false;
		isLevelSeven=false;
		isCircuitAnimationPlayed=false;
		singnal = 2;
		isBatteryAddComponent = false;
		preClickBatterySemiStatus = false;
		clickBattery =null;
	}

	void Update () 
	{
		if (isLevelSeven) 
		{
			batteryList = PhotoRecognizingPanel.Instance.batteryList;
			clickBattery = batteryList[1];//识别部分设定是ID为0的不能点击，为1的可以点击
			if (curSwitchPowered) //如果任意一条有开关的线路有电了，表示电路通了可以给电池添加脚本并显示小手了
			{
				isCircuitAnimationPlayed=true;
			}
			if (isCircuitAnimationPlayed) //电池可以被点击
			{
				#region 保证只给battery加一次脚本
				if (!isBatteryAddComponent)//保证只给battery加一次脚本
				{
					clickBattery.AddComponent<BoxCollider> ();
					clickBattery.GetComponent<BoxCollider>().size= new Vector3(114f,74f,0); 
					clickBattery.GetComponent<BoxCollider>().center= new Vector3(0,-2.6f,0);

					clickBattery.AddComponent<UIButton> ();//给随机的电池添加button组件和BatteryCtrl组件来实现点击事件
					clickBattery.AddComponent<BatteryCtrl> ();
					batteryCtrl=clickBattery.GetComponent<BatteryCtrl> ();
					curBatterySemi=batteryCtrl.isSemiTrans;

					isBatteryAddComponent = true;
				}
				#endregion

				#region 点击电池
				if (batteryCtrl && batteryCtrl.isSemiTrans) //点击了电池，电池变成半透明，1个电池工作
				{ 
					isTwoBatteryWork=false;
					clickBattery.GetComponent<UISprite>().depth=1;
				} 
				else
				{
					isTwoBatteryWork=true;
					clickBattery.GetComponent<UISprite> ().depth = 4;
				}
				curBatterySemi=batteryCtrl.isSemiTrans;
				if (preBatterySemi!=curBatterySemi) 
				{
					ChangeUI(isTwoBatteryWork);
					preBatterySemi=curBatterySemi;
				}
				#endregion

				#region 小手出现两次
				if (singnal <=0) 
				{
					if (PhotoRecognizingPanel.Instance.finger) 
					{
						Destroy (PhotoRecognizingPanel.Instance.finger);
					}
				} 
				else 
				{
					if (clickBattery && batteryCtrl) 
					{
						PhotoRecognizingPanel.Instance.ShowFinger (clickBattery.transform.localPosition);//show finger
						if ( preClickBatterySemiStatus != batteryCtrl.isSemiTrans) 
						{
							singnal--;
							preClickBatterySemiStatus = batteryCtrl.isSemiTrans;
						}
					}
				}
				#endregion	
			}
			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
		}
	}
		
	void ChangeUI(bool twoBattery)
	{

		Debug.Log("changeui");
		string bulbName="";
		float volume=0;
		if (twoBattery) //如果两个电池工作
		{
			bulbName="bulbSpark";
			volume=1f;
		}
		else
		{
			bulbName="bulbOn";
			volume=0.5f;
		}

		foreach (var item in GetImage._instance.itemList) 
		{
			switch (item.type) 
			{
			case ItemType.Bulb:
				transform.Find("bulb").GetComponent<UISprite>().spriteName=item.powered ? bulbName:"bulbOff";
				break;
			case ItemType.Loudspeaker:
				AudioSource audio = transform.Find ("loudspeaker").GetComponent<AudioSource> ();
				if (item.powered) 
				{
					if (!audio .isPlaying)
					{
						audio.Play ();

					}
					audio.volume = volume;

				} 
				else 
				{
					if (audio .isPlaying)
					{
						audio.Stop ();
					}
				}
				break;
			default:
				break;
			}
		}
	}

}
