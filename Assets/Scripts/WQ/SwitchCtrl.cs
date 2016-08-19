using UnityEngine;
using System.Collections;

public class SwitchCtrl : MonoBehaviour
{

	public static bool isSwitchOn=true;
	private GameObject switchOnBtn;
	private GameObject switchOffBtn;

	void Start () 
	{
		switchOnBtn = transform.Find ("SwitchOn").gameObject;
		switchOffBtn = transform.Find ("SwitchOff").gameObject;

		UIEventListener.Get(switchOnBtn).onClick = OnSwitchOnBtnClick;
		UIEventListener.Get(switchOffBtn).onClick = OnSwitchOffBtnClick;

	}

	void Update () 
	{
		if (isSwitchOn && switchOffBtn.activeSelf)
		{
			//如果状态是开，而当前下面的“关” 按钮开着的话，关闭它，并且打开 “开” 的按钮
			switchOnBtn.SetActive(true);
			switchOffBtn.SetActive(false);

		}
		if (!isSwitchOn && switchOnBtn.activeSelf)
		{
			//如果状态是关，而当前下面的 “开” 按钮开着的话，关闭它，并且打开 “关” 的按钮
			switchOnBtn.SetActive(false);
			switchOffBtn.SetActive(true);

	
		}
	}

	/// <summary>
	/// click SwitchOn btn,close the Switch
	/// </summary>
	/// <param name="btn">Button.</param>
	void OnSwitchOnBtnClick(GameObject btn)
	{
		isSwitchOn = false;    
	}

	/// <summary>
	/// click SwitchOff btn,open the Switch
	/// </summary>
	/// <param name="btn">Button.</param>
	void  OnSwitchOffBtnClick(GameObject btn)  
	{
		isSwitchOn = true;

	}

}
