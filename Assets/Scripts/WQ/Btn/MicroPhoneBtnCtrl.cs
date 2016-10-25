using UnityEngine;
using System.Collections;

public class MicroPhoneBtnCtrl : MonoBehaviour 
{
	[HideInInspector]
	public bool isCollectVoice = false;

	void OnEnable () 
	{
		isCollectVoice = false;
		transform.GetComponent<BoxCollider> ().enabled = false;
	}
		
	void OnClick()
	{
		isCollectVoice = true;
	}
}
