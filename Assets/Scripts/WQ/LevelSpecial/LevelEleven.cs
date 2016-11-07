using UnityEngine;
using System.Collections;
using MagicCircuit;

//level 11
public class LevelEleven : MonoBehaviour 
{
	[HideInInspector]
	public bool isVOswitchOccur=false;
	private bool isAnimationPlay=false;
	private bool isStartRecord = false;

	/// <summary>
	/// 保证声音收集一次的标志
	/// </summary>
	private bool stayForAwhile = false;

	//const int SOUND_CRITERION = 1;//音量大小标准，可以调整以满足具体需求

	void OnEnable () 
	{
		isVOswitchOccur=false;
		isAnimationPlay=false;
		isStartRecord = false;
		stayForAwhile = false;
	
	}


	void Update () 
	{
		if (isVOswitchOccur) 
		{
				Transform voiceSwitch=transform.Find("voiceOperSwitch");
				//在话筒按钮出现小手
				GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("MicroPhoneBtn").localPosition);
				//voiceSwitch.GetComponent<BoxCollider> ().enabled = true;
				//点击话筒按钮，
				if (transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice) 
				{
					Destroy (PhotoRecognizingPanel._instance.finger);
					
					if (!isStartRecord) 
					{  
						PhotoRecognizingPanel._instance.noticeToMakeVoice.SetActive(true);//弹出提示框
						PhotoRecognizingPanel._instance.voiceCollectionMark.SetActive(true);//弹出声音收集图片
						PhotoRecognizingPanel._instance.voiceCollectionMark.transform.Find ("Wave").GetComponent<MyAnimation> ().canPlay = true;//显示声音收集动画
						MicroPhoneInput.getInstance().StartRecord();//收集声音
						isStartRecord = true;
					}
					//收集到声音后，播放声音收集完成音效，提示框消失
					if (CommonFuncManager._instance.isSoundLoudEnough ()) 
					{
						isAnimationPlay = true;
						PhotoRecognizingPanel._instance.noticeToMakeVoice.SetActive (false);
						PhotoRecognizingPanel._instance.voiceCollectionMark.transform.Find ("Wave").GetComponent<MyAnimation> ().canPlay = false;
						PhotoRecognizingPanel._instance.voiceCollectionMark.SetActive (false);
						MicroPhoneInput.getInstance ().StopRecord ();
						GetImage._instance.cf.switchOnOff (int.Parse (voiceSwitch.gameObject.tag), true);
						voiceSwitch.GetComponent<UISprite>().spriteName="VOswitchOn";
					} 
				CommonFuncManager._instance.CircuitItemReset (GetImage._instance.itemList);	
				}	
		}
	}


	/// <summary>
	/// 声音收集动画播放一会
	/// </summary>
	/// <returns>The for seconds.</returns>
	IEnumerator StayForSeconds()
	{
		yield return new WaitForSeconds (1f);

	}

}
