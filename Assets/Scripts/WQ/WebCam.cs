using UnityEngine;
using System.Collections;
using OpenCVForUnity;

public class WebCam : MonoBehaviour {

	public string deviceName;
	private WebCamTexture tex;
	WebCamTexture webcamTexture;

	//[HideInInspector]
	//public Quaternion baseRotation;


	void Awake()
	{
		//baseRotation = transform.rotation;
		//tex = new WebCamTexture();

	}


	IEnumerator Start()
	{
		
//		webcamTexture = new WebCamTexture ();
//		GetComponent<Renderer>().material.mainTexture = webcamTexture;
//		webcamTexture.requestedWidth = 1280;
//		webcamTexture.requestedHeight = 720;
//		webcamTexture.Play ();


		Mat frameImg;

		yield return Application.RequestUserAuthorization (UserAuthorization.WebCam);
		if(Application.HasUserAuthorization(UserAuthorization.WebCam))
		{
			WebCamDevice[] devices = WebCamTexture.devices;
			if(devices.Length > 0){
				deviceName = devices [1].name;//devices [1] --front camera
				tex = new WebCamTexture (deviceName,72, 72, 12);
				GetComponent<Renderer>().material.mainTexture = tex;


				//Utils.webCamTextureToMat(tex, frameImg);
//				RotateCamera rotateCamera = new RotateCamera ();
//				rotateCamera.rotate (ref frameImg);

				//Utils.matToTexture2D(frameImg, texture);




				tex.Play ();
			}

		}

	}

	void Update()
	{
		//transform.rotation = baseRotation * Quaternion.AngleAxis(tex.videoRotationAngle, Vector3.up);

	}
}


