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
		case  7:
			break;
		case  9:
			break;
		case 11:
			break;
		case 12:
			break;
		case 13:
			break;
		case 14:
			break;
		case 15:
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
			break;
		case  7:
			break;
		case  9:
			break;
		case 11:
			break;
		case 12:
			break;
		case 13:
			break;
		case 14:
			break;
		case 15:
			break;
		default:
			break;

		}

	}
}
