using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

public class CommonFuncManager : MonoBehaviour 
{

	public static CommonFuncManager _instance;
	private bool tempIsLaSwitch = false;
	const int SOUND_CRITERION = 1;//音量大小标准，可以调整以满足具体需求

	void Awake()
	{
		_instance = this;
	}

	/// <summary>
	/// 接通电路
	/// </summary>
	public void OpenCircuit()
	{
		PhotoRecognizingPanel._instance.transValue = 1;
		//PhotoRecognizingPanel._instance.arrowGenInterval/=2;
		//Debug.Log ("after-----arrowGenInterval==" + PhotoRecognizingPanel._instance.arrowGenInterval);
		for (int i = 0; i < PhotoRecognizingPanel._instance.arrowList.Count; i++) 
		{
			if (PhotoRecognizingPanel._instance.arrowList [i]) 
			{
				//PhotoRecognizingPanel._instance.arrowList [i].GetComponent<ArrowCtrl> ().speed *=2;
				PhotoRecognizingPanel._instance.arrowList [i].GetComponent<UISprite> ().alpha = 1;//显示电流
			}

		}
		transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";//灯亮 

		//GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);//走电流
		GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;//标记已经播放电流
	}
		
	/// <summary>
	/// 电流动画的开启和关闭
	/// </summary>
	/// <param name="isSwitch">If set to <c>true</c> is switch.</param>
	public  void CircuitOnOrOff(bool isSwitch)
	{
		if (tempIsLaSwitch != isSwitch) 
		{
			if (!isSwitch) 
			{
				GetComponent<PhotoRecognizingPanel> ().StopCircuit ();
			}
			else
			{
				GetComponent<PhotoRecognizingPanel> ().ContinueCircuit ();
			}
			tempIsLaSwitch = isSwitch;
		} 
	}




	public bool isSoundLoudEnough()
	{
		float volume = MicroPhoneInput.getInstance ().getSoundVolume();
		if(volume > SOUND_CRITERION)
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// compute  the angle according to two points
	/// </summary>
	/// <returns>The angle.</returns>
	/// <param name="from">vector2 first</param>
	/// <param name="to">vector2 second</param>
	public float TanAngle(Vector2 from, Vector2 to)
	{
		float xdis = to.x - from.x;//计算临边长度  
		float ydis = to.y - from.y;//计算对边长度  
		float tanValue = Mathf.Atan2(ydis, xdis);//反正切得到弧度  
		float angle = tanValue * Mathf.Rad2Deg;//弧度转换为角度  
		return angle;  
	}

	//遍历新的circuitItem，根据其powered属性值来改变UI
	public void CircuitReset(List<CircuitItem> circuitItems)
	{
		//Debug.Log ("CircuitReset");
		for (int i = 0; i < circuitItems.Count ; i++) 
		{
			string tag = circuitItems [i].ID.ToString ();
			GameObject temp = GameObject.FindGameObjectWithTag (tag);
			switch (circuitItems [i].type) //找到tag与这个item的ID相等的对象，修改sprite
			{
				case ItemType.Bulb:
					temp.GetComponent<UISprite>().spriteName=(circuitItems [i].powered ? "bulbOn":"bulbOff");
					break;

				case ItemType.InductionCooker:
					//电磁炉通电/冒蒸汽的切换  to do ...
					break;
				case ItemType.Loudspeaker:
					AudioSource tempAudio = temp.GetComponent<AudioSource> ();
					if (circuitItems [i].powered) // this item is power on
					{
						if (!tempAudio.isPlaying) 
						{
							tempAudio.Play ();
						}

					} 
					else // this item is power off
					{
						if (tempAudio.isPlaying) 
						{
							tempAudio.Stop ();
						}
					}
					//喇叭有无声音的切换   to do...
					break;
			case ItemType.CircuitLine://有电则显示tag和这条线ID相同的箭头，没电则隐藏tag和这条线ID相同的箭头
					GameObject[] temps = GameObject.FindGameObjectsWithTag(tag);
					foreach (var item in temps) 
					{ 
						if (item) 
						{
							item.GetComponent<UISprite>().alpha = (circuitItems [i].powered ? 1:0);
						}
					}
					break;
				default:
					break;

			}


		}

	}
}
