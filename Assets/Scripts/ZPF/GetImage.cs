using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity;
using MagicCircuit;

public class GetImage : MonoBehaviour
{
	public static GetImage _instance;
	public bool isThreadEnd;
	public bool isCircuitCorrect;

	// Parameters for generating itemList
	public List<CircuitItem> itemList = new List<CircuitItem>();
	public CurrentFlow cf;
	public CurrentFlow_SPDTSwitch cf_SPDT;

	// Parameters for using WebCam
	[HideInInspector]
	public  Texture2D texture;
	private WebCamTexture webCamTexture;
	private WebCamDevice webCamDevice;
	private bool initDone = false;
	private int webCam_width  = 640;
	private int webCam_height = 480;

	// Parameter for loading xml file for test
	private List<CircuitItem> xmlItemList = new List<CircuitItem>();

	// Parameter for processing 10 photos
	private const int numOfPhotos = 10;
	private RotateCamera rotateCamera;
	private RecognizeAlgo recognizeAlge;
	private List<Mat> frameImgList = new List<Mat>();
	private List<List<CircuitItem>> listItemList = new List<List<CircuitItem>>();

	void Awake()
	{
		_instance = this;
	}

	void OnEnable()
	{
		isThreadEnd = false;
		isCircuitCorrect = false;

		StartCoroutine(init());

		rotateCamera = new RotateCamera();
		recognizeAlge = new RecognizeAlgo();
		cf = new CurrentFlow();
		cf_SPDT = new CurrentFlow_SPDTSwitch();

		// For test, load xml to xmlItemList
		#if UNITY_EDITOR  
		xmlItemList = XmlCircuitItemCollection.Load(Path.Combine(Application.dataPath, "Xmls/CircuitItems_lv1.xml")).toCircuitItems();
		#elif UNITY_IPHONE 
		string xmlAppDataPath = Application.dataPath.Substring(0, Application.dataPath.Length - 4);
		//Debug.Log("xmlAppDataPath = " + xmlAppDataPath);
		string xmlPath = Path.Combine(xmlAppDataPath, "Xmls/CircuitItems_lv2.xml");
		//Debug.Log("xmlPath = " + xmlPath);
		if (File.Exists(xmlPath))
			Debug.Log("Great! I have found the file!");
		else
			Debug.Log("Sorry! I have not found the file!");
		xmlItemList = XmlCircuitItemCollection.Load(xmlPath).toCircuitItems();
		#endif

		Debug.Log ("=====Start=====");
		for (var i = 0; i < xmlItemList.Count; i++)
		{
			Debug.Log("xmlItemList["+i+"]: "               + xmlItemList[i].name         +
				     " xmlItemList["+i+"].connect_left: "  + xmlItemList[i].connect_left +
				     " xmlItemList["+i+"].connect_right: " + xmlItemList[i].connect_right);
		}
		Debug.Log ("======End======");
	}

	private IEnumerator init()
	{
		if (webCamTexture != null)
		{
			webCamTexture.Stop();
			initDone = false;
		}
		WebCamDevice[] devices = WebCamTexture.devices;

		#if UNITY_EDITOR  
		webCamDevice = WebCamTexture.devices[0];
		#elif UNITY_IPHONE 
		webCamDevice = WebCamTexture.devices[1];
		#endif 

		webCamTexture = new WebCamTexture (webCamDevice.name, webCam_width, webCam_height);
		webCamTexture.Play();

		while (true)
		{
			if (webCamTexture.didUpdateThisFrame)
			{
				Mat frameImg = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC3);
				webCam_width  = webCamTexture.width;
				webCam_height = webCamTexture.height;

				texture = new Texture2D(frameImg.cols(), frameImg.rows(), TextureFormat.RGBA32, false);
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

	void Start()
	{
		
	}

	// Display current WebCamTexture to CamQuad
	void Update()
	{
		if (!initDone)
			return;
		Mat frameImg = new Mat(webCam_height, webCam_width, CvType.CV_8UC3);
		if (webCamTexture.didUpdateThisFrame)
		{
			Utils.webCamTextureToMat(webCamTexture, frameImg);
			rotateCamera.rotate(ref frameImg);
			texture.Resize(frameImg.cols(), frameImg.rows());
			Utils.matToTexture2D(frameImg, texture);
		}
		frameImg.Dispose();
	}

	public void Thread_Process_Start()
	{
		isThreadEnd = false;
		take10Pictures();

		Debug.Log("Thread_Process_Start");
		Thread threadProcess = new Thread(Thread_Process);
		threadProcess.IsBackground = true;
		threadProcess.Start();
	}

	// Thread for RecognizeAlgo.process 10 images
	private void Thread_Process()
	{
		for (var i = 0; i < frameImgList.Count; i++)
		{
			recognizeAlge.process(frameImgList[i], ref itemList);
			listItemList.Add(itemList);
		}

		// TODO
		// Average listItemList to get the final itemList
		// @Input  : listItemList
		// @Output : itemList
		// itemList = average(listItemList);
		itemList = xmlItemList;

		// Compute CurrentFlow
		computeCurrentFlow();

		isThreadEnd = true;
	}

	private void computeCurrentFlow()
	{
		if (LevelManager.currentLevelData.LevelID == 15) 
			isCircuitCorrect = cf_SPDT.compute(ref itemList);
		else 
			isCircuitCorrect = cf.compute(ref itemList, LevelManager.currentLevelData.LevelID);

		Debug.Log("CurrentFlow compute result = " + isCircuitCorrect);
		Debug.Log("itemList.Count = " + itemList.Count);
		for (var i = 0; i < itemList.Count; i++)
			Debug.Log(i + " " + itemList[i].list[0] + " " + itemList[i].powered);
	}

	private bool takePicture(ref Mat frameImg)
	{
		frameImg = new Mat(webCam_height, webCam_width, CvType.CV_8UC3);

		if (webCamTexture.didUpdateThisFrame) 
		{
			Utils.webCamTextureToMat(webCamTexture, frameImg);
			rotateCamera.rotate(ref frameImg);
			return true;
		}
		return false;
	}

	private void take10Pictures()
	{
		Mat frameImg = new Mat();
		for (var i = 0; i < numOfPhotos;)
			if (takePicture(ref frameImg))
			{
				i++;
				frameImgList.Add(frameImg);
			}
	}
}