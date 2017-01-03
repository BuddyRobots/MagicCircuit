using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenCVForUnity;
using MagicCircuit;


[RequireComponent(typeof(WebCamTextureToMatHelper_Test))]
public class GetImage : MonoBehaviour
{
	#region 公共变量
	public static GetImage _instance;
	public bool isThreadEnd = false;
	public bool isCircuitCorrect = false;
	public bool isStartUpdate = true;
	// Flag for taking 10 photos
	public bool isTakingPhoto = false;

	// Parameters for generating itemList
	public List<CircuitItem> itemList = new List<CircuitItem>();
	public List<Mat> frameImgList = new List<Mat>();
	public CurrentFlow cf;
	public CurrentFlow_SPDTSwitch cf_SPDT;

	// Parameters for using WebCam
	[HideInInspector]
	public  Texture2D texture;
	#endregion

	#region 私有属性
	private Mat frameImg;
	private WebCamTextureToMatHelper_Test webCamTextureToMatHelper_test;
	private Color32[] colors;
	private float GCTimer=0f;
	private float GCTime=1f;

	private RecognizeAlgo recognizeAlge;
	public WebCamTexture webCamTexture;
	private WebCamDevice webCamDevice;
	private bool initDone = false;
	private int webCam_width  = 640;
	private int webCam_height = 480;
	// Parameter for loading xml file for test
	private List<CircuitItem> xmlItemList = new List<CircuitItem>();
	private List<List<CircuitItem>> listItemList = new List<List<CircuitItem>>();
	#endregion 


	void Awake()
	{
		_instance = this;

		recognizeAlge = new RecognizeAlgo();
		cf = new CurrentFlow();
		cf_SPDT = new CurrentFlow_SPDTSwitch();
	}
		

	void Start()
	{
		webCamTextureToMatHelper_test = gameObject.GetComponent<WebCamTextureToMatHelper_Test>();

		webCamTextureToMatHelper_test.Init();
	}

	void OnEnable()
	{
		// For test, load xml to xmlItemList
		#if UNITY_EDITOR  
		string xmlPath = "Xmls/CircuitItems_lv" + LevelManager.currentLevelData.LevelID + ".xml";
		xmlItemList = XmlCircuitItemCollection.Load(Path.Combine(Application.dataPath, xmlPath)).toCircuitItems();
		#elif UNITY_IPHONE 
		#endif
	}
		

	void Update()
	{
		if (webCamTextureToMatHelper_test.isPlaying() && webCamTextureToMatHelper_test.didUpdateThisFrame())
		{
		    frameImg = webCamTextureToMatHelper_test.GetMat();




			#if UNITY_IPHONE
			RotateCamera.rotate(ref frameImg);
			#endif

			if (isTakingPhoto)
			{	

				frameImgList.Add(frameImg.clone());
				if (frameImgList.Count >= Constant.TAKE_NUM_OF_PHOTOS)
					isTakingPhoto = false; 

			}

			texture.Resize(frameImg.cols(), frameImg.rows());
			Utils.matToTexture2D(frameImg, texture, colors);
		}


	
		GCTimer+=Time.deltaTime;
		if (GCTimer>=GCTime) 
		{
			GCTimer=0;
			GC.Collect();
		}

	}

	public void Thread_Process_Start()
	{
		isThreadEnd = false;
		Manager.Instance.isTreadEnd=isThreadEnd;
		
		listItemList.Clear();

		Thread threadProcess = new Thread(Thread_Process);
		threadProcess.IsBackground = true;
		threadProcess.Start();
	}

	// Thread for RecognizeAlgo.process 10 images
	private void Thread_Process()
	{
		Debug.Log("GetImage.cs Thread_Process : Start!");

		for(var i = 0; i < frameImgList.Count; i++)
		{

			int startTime_1 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;

			itemList.Clear();

			recognizeAlge.process(frameImgList[i], ref itemList);

			listItemList.Add(itemList);


			int time_1 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
			int elapse_1 = time_1 - startTime_1;

			//Debug.Log("GetImage.cs Thread_Process : image NO. " + i + " itemList.Count = " + itemList.Count + " time elapse" + elapse_1);
		}

		// TODO
		// Average listItemList to get the final itemList
		// @Input  : listItemList
		// @Output : itemLists
		// itemList = average(listItemList);

		#if UNITY_EDITOR
		itemList = xmlItemList;
		#elif UNITY_IPHONE 
		#endif


		frameImgList.Clear();



		///
		for (var i = 0; i < itemList.Count; i++)
		{
			Debug.Log("RecognizeAlgo.cs Threadd_Process() : itemList["+i+"].type = " + itemList[i].type);
		}
		///
		int startTime_2 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
		///
	


		// Compute CurrentFlow
		computeCurrentFlow();


		int time_2 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
		int elapse_2 = time_2 - startTime_2;
		//Debug.Log("GetImage.cs Thread_Process() : computeCurrentFlow Time elapse : " + elapse_2);
		//Debug.Log("Thread_Process_End");

		isThreadEnd = true;
		Manager.Instance.isTreadEnd=isThreadEnd;
	}

	private void computeCurrentFlow()
	{
		if (LevelManager.currentLevelData.LevelID == 15) 
		{
			isCircuitCorrect = cf_SPDT.compute(itemList);

		}
		else 
		{
			isCircuitCorrect = cf.compute(itemList, LevelManager.currentLevelData.LevelID);
			
        }
         Manager.Instance.isCircuitCorrect=isCircuitCorrect;
	}



	private void test_hsv_AdaptThreshold(ref Mat frameImg)
	{
		Mat grayImg = new Mat(frameImg.rows(), frameImg.cols(), CvType.CV_8UC1);
		Imgproc.cvtColor(frameImg, grayImg, Imgproc.COLOR_BGR2GRAY);
		Imgproc.adaptiveThreshold(grayImg, grayImg, 255, Imgproc.ADAPTIVE_THRESH_GAUSSIAN_C, Imgproc.THRESH_BINARY_INV, 21, 10);
		Imgproc.morphologyEx(grayImg, grayImg, Imgproc.MORPH_OPEN, Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(3, 3)));
		//Imgproc.morphologyEx(rstImg, rstImg, Imgproc.MORPH_CLOSE, Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(3, 3)));
		frameImg = grayImg.clone();
	}



	/// <summary>
	/// Raises the web cam texture to mat helper inited event.
	/// </summary>
	public void OnWebCamTextureToMatHelperInited()
	{

		Mat webCamTextureMat = webCamTextureToMatHelper_test.GetMat();

		colors = new Color32[webCamTextureMat.cols() * webCamTextureMat.rows()];
		texture = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);


//		gameObject.transform.localScale = new Vector3(webCamTextureMat.cols(), webCamTextureMat.rows(), 1);


		gameObject.transform.localScale = new Vector3(Constant.CAM_QUAD_WIDTH, Constant.CAM_QUAD_HEIGHT, 1);


//		Debug.Log("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);

		float width = 0;
		float height = 0;

		width = gameObject.transform.localScale.x;
		height = gameObject.transform.localScale.y;

		float widthScale =(float)Screen.width / width;
		float heightScale =(float)Screen.height / height;
		if (widthScale < heightScale) {
			Camera.main.orthographicSize =(width *(float)Screen.height /(float)Screen.width) / 2;
		} else {
			Camera.main.orthographicSize = height / 2;
		}

		gameObject.GetComponent<Renderer>().material.mainTexture = texture;

	}


	public void OnWebCamTextureToMatHelperDisposed()
	{
		Debug.Log("OnWebCamTextureToMatHelperDisposed");

	}

	void OnDisable()
	{
		webCamTextureToMatHelper_test.Dispose();
	}		
}	