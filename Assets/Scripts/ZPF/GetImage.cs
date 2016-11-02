using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity;
using MagicCircuit;
using System.Runtime.InteropServices;

public class GetImage : MonoBehaviour
{
	[DllImport("__Internal")]
	private static extern void _SavePhoto (string readAddr);

	public static GetImage _instance;
	public bool isThreadEnd = false;
	public bool isCircuitCorrect = false;

	// Flag for taking 10 photos
	public bool isTakingPhoto = false;

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
//	private List<CircuitItem> xmlItemList = new List<CircuitItem>();
	public List<CircuitItem> xmlItemList = new List<CircuitItem>();// for test

	private RotateCamera rotateCamera;
	private RecognizeAlgo recognizeAlge;

	public List<Mat> frameImgList = new List<Mat>();
	private List<List<CircuitItem>> listItemList = new List<List<CircuitItem>>();

	void OnEnable()
	{
		_instance = this;

		StartCoroutine(init());

		rotateCamera = new RotateCamera();
		recognizeAlge = new RecognizeAlgo();
		cf = new CurrentFlow();
		cf_SPDT = new CurrentFlow_SPDTSwitch();

		// For test, load xml to xmlItemList
		#if UNITY_EDITOR  
		xmlItemList = XmlCircuitItemCollection.Load(Path.Combine(Application.dataPath, "Xmls/CircuitItems_lv2.xml")).toCircuitItems();

//		Debug.Log ("=====Start=====");
//		for (var i = 0; i < xmlItemList.Count; i++)
//		{
//			Debug.Log("xmlItemList["+i+"]: "               + xmlItemList[i].name         +
//				     " xmlItemList["+i+"].connect_left: "  + xmlItemList[i].connect_left +
//				     " xmlItemList["+i+"].connect_right: " + xmlItemList[i].connect_right);
//		}
//		Debug.Log ("======End======");
		#elif UNITY_IPHONE 
//		string xmlAppDataPath = Application.dataPath.Substring(0, Application.dataPath.Length - 4);
//		//Debug.Log("xmlAppDataPath = " + xmlAppDataPath);
//		string xmlPath = Path.Combine(xmlAppDataPath, "Xmls/CircuitItems_lv2.xml");
//		//Debug.Log("xmlPath = " + xmlPath);
//		if (File.Exists(xmlPath))
//		Debug.Log("Great! I have found the file!");
//		else
//		Debug.Log("Sorry! I have not found the file!");
//		xmlItemList = XmlCircuitItemCollection.Load(xmlPath).toCircuitItems();
		#endif
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

	void Update()
	{
		if (!initDone)
			return;
		Mat frameImg = new Mat(webCam_height, webCam_width, CvType.CV_8UC3);
		if (webCamTexture.didUpdateThisFrame)
		{
			Utils.webCamTextureToMat(webCamTexture, frameImg);
			rotateCamera.rotate(ref frameImg);

			if (isTakingPhoto)
			{	
				frameImgList.Add(frameImg.clone());
				if (frameImgList.Count >= Constant.TAKE_NUM_OF_PHOTOS)
					isTakingPhoto = false;
			}

			texture.Resize(frameImg.cols(), frameImg.rows());
			Utils.matToTexture2D(frameImg, texture);
			if (getImage == true)
			{
				if (frameImgList.Count >= Constant.THREAD_TAKE_NUM_OF_PHOTOS)
				{
					getImage = false;
				}
				else
				{
					frameImgList.Add(frameImg);
				}
			}
		}
		frameImg.Dispose();
	}

	public void Thread_Process_Start()
	{
		isThreadEnd = false;
		listItemList.Clear();

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
			int startTime_1 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;



			itemList.Clear();
			recognizeAlge.process(frameImgList[i], ref itemList);
			listItemList.Add(itemList);



			int time_1 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
			int elapse_1 = time_1 - startTime_1;
			Debug.Log("Thread_Process image NO." + i + " itemList.Count = " + itemList.Count + " time elapse" + elapse_1);
		}

		// TODO
		// Average listItemList to get the final itemList
		// @Input  : listItemList
		// @Output : itemLists
		// itemList = average(listItemList);
		itemList = xmlItemList;


		frameImgList.Clear();

//		for (int i = 0; i < itemList.Count; i++) {
//			Debug.Log("------------------");
//			for (int j = 0; j < itemList[i].list.Count; j++) {
//				Debug.Log("itemlist["+i+"]["+j+"]===="+itemList[i].list[j]);
//			}
//		}


		int startTime_2 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;




		// Compute CurrentFlow
		computeCurrentFlow();

		for (int i = 0; i < itemList.Count; i++) {
			for (int j = 0; j < itemList[i].list.Count; j++) {
				Debug.Log("GetImage Thread_Process itemlist["+i+"].list["+j+"]===="+itemList[i].list[j]);
			}
		}
			
	

		int time_2 = DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
		int elapse_2 = time_2 - startTime_2;
		Debug.Log("computeCurrentFlow Time elapse : " + elapse_2);
		Debug.Log("Thread_Process_End");

		isThreadEnd = true;
	}

	private void computeCurrentFlow()
	{
		if (LevelManager.currentLevelData.LevelID == 15) 
			isCircuitCorrect = cf_SPDT.compute(ref itemList);
		else 
			isCircuitCorrect = cf.compute(ref itemList, LevelManager.currentLevelData.LevelID);

		Debug.Log("CurrentFlow compute result = " + isCircuitCorrect);
//		Debug.Log("itemList.Count = " + itemList.Count);
//		for (var i = 0; i < itemList.Count; i++)
//			Debug.Log(i + " " + itemList[i].type + " " + itemList[i].list[0] + " " + itemList[i].powered);
	}
}