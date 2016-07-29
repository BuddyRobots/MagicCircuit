using UnityEngine;
using System.Collections;

public class WebCam : MonoBehaviour {



	public string deviceName;
	WebCamTexture tex;
	IEnumerator Start()
	{

		yield return Application.RequestUserAuthorization (UserAuthorization.WebCam);
		if(Application.HasUserAuthorization(UserAuthorization.WebCam))
		{
			WebCamDevice[] devices = WebCamTexture.devices;
			if(devices.Length > 0){
				deviceName = devices [0].name;
				tex = new WebCamTexture (deviceName, 400, 300, 12);
				GetComponent<Renderer>().material.mainTexture = tex;
				tex.Play ();
			}

		}

	}
}


