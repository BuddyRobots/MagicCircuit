using UnityEngine;
using OpenCVForUnity;
using System.Collections;

public class getImage : MonoBehaviour
{
    [HideInInspector]
    public Texture2D texture;
	//[HideInInspector]
	//public Quaternion baseRotation;

    private WebCamTexture webCamTexture;
    private WebCamDevice webCamDevice;
    private Mat frameImg;

    private bool initDone = false;
    private const int cam_width  = 1280;
    private const int cam_height = 720;
    private const int tex_width  = 1280;//1120;//640;
    private const int tex_height = 720;

    // Use this for initialization
    void Start() {

        // Initialize webcam
        StartCoroutine(init());
    }

    private IEnumerator init()
    {
		
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
            initDone = false;
            frameImg.Dispose();
        }

        webCamDevice = WebCamTexture.devices[0];
        webCamTexture = new WebCamTexture(webCamDevice.name, cam_width, cam_height);


        webCamTexture.Play();
		//baseRotation = transform.rotation;

        while (true)
        {
            if (webCamTexture.didUpdateThisFrame)
            {
                frameImg = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC3);

                texture = new Texture2D(tex_width, tex_height, TextureFormat.RGBA32, false);
                gameObject.GetComponent<Renderer>().material.mainTexture = texture;

                initDone = true;
                break;
            }
            else
            {
                yield return 0;
            }
        }
    }

    // Update is called once per frame
    void Update() {
		
		//transform.rotation = baseRotation * Quaternion.AngleAxis(webCamTexture.videoRotationAngle, Vector3.up);
	
        if (!initDone)
            return;

        if (webCamTexture.didUpdateThisFrame)
        {
			//Debug.Log ("Update==webCamTexture.height==" + webCamTexture.height + "webCamTexture.width==" + webCamTexture.width);
			//Debug.Log ("Update==frameImg.height==" + frameImg.size());
            Utils.webCamTextureToMat(webCamTexture, frameImg);
            Mat resultImg = frameImg.clone();

            // Image Processing Codes
			Debug.Log(resultImg.size());

            Utils.matToTexture2D(resultImg, texture);
        }
    }
}