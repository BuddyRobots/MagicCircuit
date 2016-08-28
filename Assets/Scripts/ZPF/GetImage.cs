using UnityEngine;
using OpenCVForUnity;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using MagicCircuit;
using System.Threading;

public class GetImage : MonoBehaviour
{
	public static GetImage _instance;

    [HideInInspector]
    public Texture2D texture;

    private WebCamTexture webCamTexture;
    private WebCamDevice webCamDevice;
    private Mat frameImg;

	private const int cam_width = 640;
	private const int cam_height = 480;
	private const int tex_width  = 640;//1120;//640;
	private const int tex_height = 480;

	// textures
	public Texture2D light_tex;
	public Texture2D battery_tex;
	public Texture2D switch_tex;
	public Texture2D line_tex;

	public RecognizeAlgo recognizeAlgo;
	private RotateCamera rotateCamera;

	public List<CircuitItem> listItem;

	/// <summary>
	/// this is a mark to judge if the snapshot is done
	/// </summary>
	private bool isShotTook=false;
	private bool initDone = false;
	/// <summary>
	/// this is a mark to judge if 10 photos were save
	/// </summary>
	private bool isTakePicture;
	/// <summary>
	/// a mark to judge if the thread of 10-photos-handling was over 
	/// </summary>
	private bool isFinishHandlePicture;
	private List<Mat> tempImgs = new List<Mat>();
	public List<List<CircuitItem>> itemLists=new List<List<CircuitItem>>(); //多个图标集合的集合，之后会根据这个集合提炼出最终的一个List<CircuitItem>（），用来做识别界面取图标的依据


	void Awake()
	{
		_instance = this;

	}
		
    void Start() 
	{
		rotateCamera = new RotateCamera ();  
		// Intialize RecogniazeAlgo
		recognizeAlgo = new RecognizeAlgo(light_tex,
			battery_tex,
			switch_tex,
			line_tex);

		listItem = new List<CircuitItem>();
    }

	void OnEnable()
	{
		isShotTook=false;
		isTakePicture = false;
		isFinishHandlePicture = false;
		initDone = false;

		tempImgs.Clear ();

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
		WebCamDevice[] devices = WebCamTexture.devices;

		#if UNITY_EDITOR  
		webCamDevice = WebCamTexture.devices [0];
		#elif UNITY_IPHONE 
		webCamDevice = WebCamTexture.devices [1];
		#endif 

		webCamTexture = new WebCamTexture (webCamDevice.name, cam_width, cam_height);
		webCamTexture.Play ();

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



	void Update()
	{
		TakePhoto();
		if (!isFinishHandlePicture && tempImgs.Count >= 10) 
		{
			TakePicture_Start ();
			isFinishHandlePicture = true;
		}
	}

	public void TakePicture()
	{
		isTakePicture = true; 
	}


//	void Update() 
//	{
//		listItem = new List<CircuitItem>();
//		if (!initDone)
//			return;
//		if (webCamTexture.didUpdateThisFrame)
//		{
//			
////			Utils.webCamTextureToMat(webCamTexture, frameImg);
////			Mat tmpImg = frameImg.clone ();
////			rotateCamera.rotate (ref tmpImg);
////			// Image Processing Codes
////			Mat resultImg = recognizeAlgo.process(tmpImg, ref listItem);
////			//itemLists.Add (listItem);
////			texture.Resize(resultImg.cols(), resultImg.rows());
////			Utils.matToTexture2D(resultImg, texture);
//
//
//
//			Utils.webCamTextureToMat(webCamTexture, frameImg);
//			Mat tmpImg = frameImg.clone ();
//			rotateCamera.rotate (ref tmpImg);
//			texture.Resize(tmpImg.cols(), tmpImg.rows());
//			Utils.matToTexture2D(tmpImg, texture);
//
//			  
//
//
//			if (!isShotTook) 
//			{
//				TakeSnapShot ();
//				isShotTook = true;
//			}
//		}
//	}



	/// <summary>
	/// Take photos 
	/// </summary>
	public void TakePhoto() 
	{
		
        if (!initDone)
            return;
        if (webCamTexture.didUpdateThisFrame)
        {

			Utils.webCamTextureToMat(webCamTexture, frameImg);
			Mat tmpImg = frameImg.clone ();
			rotateCamera.rotate (ref tmpImg);

			if (isTakePicture && tempImgs.Count < 10) {
				tempImgs.Add (tmpImg);
			}
				
			texture.Resize(tmpImg.cols(), tmpImg.rows());
			Utils.matToTexture2D(tmpImg, texture);
			        
			if (!isShotTook) 
			{
				TakeSnapShot ();
				isShotTook = true;
			}
        }
    }
	//开启线程的地方
	private void TakePicture_Start()
	{
		Thread takePicture = new Thread (ThreadTakePicture);
		takePicture.IsBackground = true;
		takePicture.Start ();
		//Debug.Log ("itemList.count : " + itemLists.Count);
	}

	Mat img=new Mat();

	//线程函数,此函数用于处理已经获得的照片
	private void ThreadTakePicture()
	{
		itemLists.Clear ();                                                                                                                                                                                                                                                                                                                         

		for (int i = 0; i < tempImgs.Count; i++) {
			
			List<CircuitItem> temp = new List<CircuitItem>();

			Mat resultImg = recognizeAlgo.process(tempImgs[i], ref temp);
			img = resultImg;
			itemLists.Add (temp);

			//texture.Resize(resultImg.cols(), resultImg.rows());
			//Utils.matToTexture2D(resultImg, texture);
		}
		//Debug.Log ("itemList.count : " + itemLists[4].Count);
	}
		
	public void SavePic()
	{
		Texture2D tex2D = new Texture2D (img.cols(), img.rows());
		Utils.matToTexture2D (img, tex2D);
		tex2D.Apply ();
		#if UNITY_EDITOR  
		string path = Application.dataPath +"/Pic/" + "pic.jpg";
		#elif UNITY_IPHONE 
		string path =Application.persistentDataPath+"/b.jpg";
		#endif 
		File.WriteAllBytes(path, tex2D.EncodeToJPG ());

	}

	/// <summary>
	/// Take a snap shot.
	/// </summary>
	void TakeSnapShot()
	{
		Texture2D snap = new Texture2D(webCamTexture.width, webCamTexture.height);
		snap.SetPixels(webCamTexture.GetPixels());
		snap.Apply();

		#if UNITY_EDITOR  
		string filepath = Application.dataPath +"/PaiZhao/" + "a.jpg";
		#elif UNITY_IPHONE 
		string filepath =Application.persistentDataPath+"/a.jpg";
		#endif 

		File.WriteAllBytes(filepath, snap.EncodeToJPG ());


	}
}