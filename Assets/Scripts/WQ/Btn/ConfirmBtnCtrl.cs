using UnityEngine;
using System.Collections;

/// <summary>
/// control of the confirmBtn on both successPanel and failurePanel
/// </summary>
public class ConfirmBtnCtrl : MonoBehaviour 
{
	void OnClick()
	{
		transform.parent.gameObject.SetActive (false);
	}
}
