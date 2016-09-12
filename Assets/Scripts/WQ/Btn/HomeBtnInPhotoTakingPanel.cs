using UnityEngine;
using System.Collections;

public class HomeBtnInPhotoTakingPanel : MonoBehaviour {

	private GameObject startPanel;


	void Start () 
	{

		startPanel = transform.parent.parent.Find ("StartPanel").gameObject;


	}
	void OnClick()
	{
		startPanel.SetActive (true);
		transform.parent.GetComponent<PhotoTakingPanel> ().PanelOff();

	}

}
