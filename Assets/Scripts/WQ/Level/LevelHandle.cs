using UnityEngine;
using System.Collections;

public class LevelHandle : MonoBehaviour 
{


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
