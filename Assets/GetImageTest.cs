using UnityEngine;
using System.Collections;

public class GetImageTest : MonoBehaviour {

	private WebCamTexture webCamTexture;
	private WebCamDevice webCamDevice;
	private bool initDone = false;
	private int webCam_width  = 640;
	private int webCam_height = 480;
	[HideInInspector]
	public  Texture2D texture;

	void Start () 
	{
		StartCoroutine(init());
	}


	private IEnumerator init()
	{
		if (webCamTexture != null)
		{
			webCamTexture.Stop();
			initDone = false;
		}
	
		#if UNITY_EDITOR  
		if (WebCamTexture.devices.Length>0) 
		{
			webCamDevice = WebCamTexture.devices[0];
		}
		else
		{
			Debug.Log("no camera");
		}
		#elif UNITY_IPHONE 
		if (WebCamTexture.devices.Length>1) 
		{
		webCamDevice = WebCamTexture.devices[1];
		}
		else
		{
		Debug.Log("no camera");
		}
		#endif 

		webCamTexture = new WebCamTexture (webCamDevice.name, webCam_width, webCam_height);

		webCamTexture.Play();

		while (true)
		{
			if (webCamTexture.didUpdateThisFrame)
			{
				webCam_width  = webCamTexture.width;
				webCam_height = webCamTexture.height;
			
//				texture = new Texture2D(webCam_width, webCam_height, TextureFormat.RGBA32, false);
				gameObject.GetComponent<Renderer>().material.mainTexture = webCamTexture;

				initDone = true;
				break;
			}
			else
			{
				yield return 0;
			}
		}
	
	}
}
