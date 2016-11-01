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
			transform.gameObject.AddComponent<ParallelCircuitsWithTwoSwitch> ();
			break;
		case  7:      
			transform.gameObject.AddComponent<ParallelCircuitWithTwoBattery> ();
			break;
		case 8:
			transform.gameObject.AddComponent<LevelEight> ();
			break;
		case  9:      
			transform.gameObject.AddComponent<ParallelCircuitWithTwoBulb> ();
			break;
		case 10:      
			transform.gameObject.AddComponent<LevelTen> ();
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
			transform.gameObject.AddComponent<LevelFourteen> ();
			break;
		case 15:
			transform.gameObject.AddComponent<SPDTswitchOccur> ();
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// 根据关卡等级添加对应的关卡脚本
	/// </summary>
	/// <param name="levelID">Level I.</param>
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
			GetComponent<ParallelCircuitsWithTwoSwitch>().isParrallelCircuit=true;
			break;
		case 7:
			GetComponent<ParallelCircuitWithTwoBattery>().isParallelCircuitWithTwoBattery=true;
			break;
		case 8:
			GetComponent<LevelEight>().isLevelEight=true;
			break;
		case 9:
			GetComponent<ParallelCircuitWithTwoBulb>().isParallelCircuitWithTwoBulb=true;
			break;
		case 10:
			GetComponent<LevelTen>().isLevelTen=true;
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
			GetComponent<LevelFourteen> ().isLevelFourteen = true;
			break;
		case 15:
			GetComponent<SPDTswitchOccur> ().isSPDTswitchOccur = true;
			break;
		default:
			break;
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
		case 6:
			Destroy(transform.GetComponent<ParallelCircuitsWithTwoSwitch> ());
			break;
		case 7:
			Destroy(transform.GetComponent<ParallelCircuitWithTwoBattery> ());

			break;
		case 8:
			Destroy(transform.GetComponent<LevelEight> ());
			break;
		case  9:
			Destroy (transform.GetComponent<ParallelCircuitWithTwoBulb> ());
			break;
		case 10:
			Destroy(transform.GetComponent<LevelTen> ());
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
			Destroy (transform.GetComponent<LevelFourteen> ());
			break;
		case 15:
			Destroy (transform.GetComponent<SPDTswitchOccur> ());
			break;
		default:
			break;
		}
	}
}
