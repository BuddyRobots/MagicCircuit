using UnityEngine;
using System.Collections;

public class LevelHandle : MonoBehaviour 
{

	public static LevelHandle _instance;

	void Awake()
	{
		_instance = this;

	}


	void OnEnable()
	{
		//给识别界面添加组件
		switch (LevelManager.currentLevelData.LevelID)
		{
		case 1:
			transform.gameObject.AddComponent<PlayCircuitAnimation> ();
			break;
		case 2:
			transform.gameObject.AddComponent<RemoveLine> ();
			break;
		case 3:
			transform.gameObject.AddComponent<NormalSwitchOccur> ();
			break;
		case 4:
			transform.gameObject.AddComponent<LoudSpeakerInLevelFour> ();
			break;
		case 5:
			transform.gameObject.AddComponent<TwoSwitchInSeriesCircuit> ();
			break;
		case 6:
			//for test         to do .....
			transform.gameObject.AddComponent<LightActiveSwitchOccur> ();

			break;
		case  7:
			//for test         to do .....
			transform.gameObject.AddComponent<VOswitchOccur> ();
			break;
		case 8:
			//for test         to do .....
			transform.gameObject.AddComponent<SPDTswitchOccur> ();
			break;
		case  9:
			//for test         to do .....
			transform.gameObject.AddComponent<VOswitchAndLAswitchTogether> ();
			break;
		case 10:
			//for test         to do .....
			transform.gameObject.AddComponent<TwoSwitchInSeriesCircuit> ();
			break;
		case 11:
			transform.gameObject.AddComponent<VOswitchOccur> ();
			break;
		case 12:
			transform.gameObject.AddComponent<LightActiveSwitchOccur> ();
			break;
		case 13:
			transform.gameObject.AddComponent<VOswitchAndLAswitchTogether> ();
			break;
		case 14:
			break;
		case 15:
			transform.gameObject.AddComponent<SPDTswitchOccur> ();
			break;
		default:
			break;

		}


	}

	public void CircuitHandleByLevelID(int levelID)
	{
		switch (levelID) 
		{

		case 1:
			gameObject.GetComponent<PlayCircuitAnimation> ().isPlayCircuitAnimation = true;
			break;
		case 2:
			gameObject.GetComponent<RemoveLine> ().isRemoveLine = true;
			break;
		case 3:
			GetComponent<NormalSwitchOccur> ().isNormalSwitchOccur = true;
			break;
		case 4:
			GetComponent<LoudSpeakerInLevelFour> ().isLoudSpeakerOccur = true;
			break;
		case 5:
			GetComponent<TwoSwitchInSeriesCircuit> ().isTwoSwitchInSeriesCircuit = true;
			break;
		case 6:
			//for test        to do.....
			GetComponent<LightActiveSwitchOccur> ().isLAswitchOccur = true;
			break;
		case 7:
			//for test        to do.....
			GetComponent<VOswitchOccur> ().isVOswitchOccur = true;
			break;
		case 8:
			//for test        to do.....
			GetComponent<SPDTswitchOccur> ().isSPDTswitchOccur = true;
			break;
		case 9:
			//for test        to do.....
			GetComponent<VOswitchAndLAswitchTogether> ().isVOswitchAndLAswitchTogether = true;
			break;
		case 10:
			//for test        to do.....
			GetComponent<TwoSwitchInSeriesCircuit> ().isTwoSwitchInSeriesCircuit = true;
			break;
		case 11:
			GetComponent<VOswitchOccur> ().isVOswitchOccur = true;
			break;
		case 12:
			GetComponent<LightActiveSwitchOccur> ().isLAswitchOccur = true;
			break;
		case 13:
			GetComponent<VOswitchAndLAswitchTogether> ().isVOswitchAndLAswitchTogether = true;
			break;
		case 14:
			break;
		case 15:
			GetComponent<SPDTswitchOccur> ().isSPDTswitchOccur = true;
			break;
		default:
			break;
			//如果是其他关卡（有开关的），如果是串联电路，应该把所有开关都闭合以后再显示电流；如果是并联电路，则...to do...
			//并联的有些复杂，暂时假设都是串联电路
			//需要一个所有开关的集合，遍历这个集合，如果所有开关都闭合了，就显示电流
		}
	}

	void OnDisable()
	{
		//删除组件
		switch (LevelManager.currentLevelData.LevelID)
		{
		case 1:
			Destroy (transform.GetComponent<PlayCircuitAnimation> ());
			break;
		case 2:
			Destroy (transform.GetComponent<RemoveLine> ());
			break;
		case 3:
			Destroy (transform.GetComponent<NormalSwitchOccur> ());
			break;
		case 4:
			Destroy (transform.GetComponent<LoudSpeakerInLevelFour> ());
			break;
		case 5:
			Destroy(transform.GetComponent<TwoSwitchInSeriesCircuit> ());
			break;
		case 6:// for test...
			Destroy(transform.GetComponent<LightActiveSwitchOccur> ());
			break;

		case 7:// for test...
			Destroy(transform.GetComponent<VOswitchOccur> ());
			break;
		case 8:// for test...
			Destroy(transform.GetComponent<SPDTswitchOccur> ());
			break;
		case  9:// for test...
			Destroy (transform.GetComponent<VOswitchAndLAswitchTogether> ());
			break;
		case 10:// for test...
			Destroy(transform.GetComponent<TwoSwitchInSeriesCircuit> ());
			break;
		case 11:
			Destroy(transform.GetComponent<VOswitchOccur> ());
			break;
		case 12:
			Destroy(transform.GetComponent<LightActiveSwitchOccur> ());
			break;
		case 13:
			Destroy (transform.GetComponent<VOswitchAndLAswitchTogether> ());
			break;
		case 14:
			break;
		case 15:
			Destroy (transform.GetComponent<SPDTswitchOccur> ());
			break;
		default:
			break;

		}

	}
}
