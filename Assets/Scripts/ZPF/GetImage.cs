using UnityEngine;
using OpenCVForUnity;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using MagicCircuit;
using System.Threading;
using System.Runtime.InteropServices;

public class GetImage : MonoBehaviour
{
	[DllImport("__Internal")]
	private static extern void _SavePhoto (string readAddr);
	[DllImport("__Internal")]  
	private static extern int testLuaWithArr(double[] arr,int len);

	public static GetImage _instance;

	/// <summary>
	/// 用作识别界面显示的照片
	/// </summary>
    [HideInInspector]
    public Texture2D texture;

    private WebCamTexture webCamTexture;
    private WebCamDevice webCamDevice;
    private Mat frameImg;
	private Mat img=new Mat();

	private const int cam_width  = 640;
	private const int cam_height = 480;
	private const int tex_width  = 640;
	private const int tex_height = 480;


	public RecognizeAlgo recognizeAlgo;
	private RotateCamera rotateCamera;

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
	/// <summary>
	/// 是否处理完了10张图片的标志
	/// </summary>
	[HideInInspector]
	public bool isHandleDone_ItemList = false;

	private List<Mat> tempImgs = new List<Mat>();
	/// <summary>
	/// itemsList的集合，多个图标集合的集合，之后会根据这个集合提炼出最终的一个List<CircuitItem>（），用来做识别界面取图标的依据
	/// </summary>
	public List<List<CircuitItem>> imageList=new List<List<CircuitItem>>(); 
	/// <summary>
	/// 所有items的集合,用作识别界面显示item的数据
	/// </summary>
	public List<CircuitItem> itemList=new List<CircuitItem>();
	public List<CircuitItem> itemList_temp=new List<CircuitItem>();//for test..

	private List<Mat> matList=new List<Mat>();

	private byte[] arr=new byte[28*28*3];
	private	double[] sample=new double[28*28*3];
	[HideInInspector]
	public CurrentFlow cf;
	[HideInInspector]
	public CurrentFlow_SPDTSwitch cf_SPDT;
	/// <summary>
	/// 判断电路是否正确的标记,默认为false，这个结果用于识别界面做判断是跳转到welldone界面还是failure界面
	/// </summary>
	public bool result=true;

	void Awake()
	{
		_instance = this;

	}

	void OnEnable()
	{

		result = true;
		isShotTook=false;
		isTakePicture = false;
		isFinishHandlePicture = false;
		initDone = false;
		tempImgs.Clear ();

		//是否处理完了十张图片
		isHandleDone_ItemList = false;

		StartCoroutine(init());
		//////////////////////////////////
		/// for test..
		#if UNITY_EDITOR  

		itemList_temp = XmlCircuitItemCollection.Load(Path.Combine(Application.dataPath, "Xmls/CircuitItems_lv3.xml")).toCircuitItems();

		#elif UNITY_IPHONE 

		string xmlPath4 = Application.dataPath.Substring( 0,  Application.dataPath.Length - 4);
		//			Debug.Log("xmlPath4==" + xmlPath4);
		string xmlPath5 = Path.Combine(xmlPath4, "Xmls/CircuitItems_lv2.xml");
		//			Debug.Log("xmlPath5==" + xmlPath5);

		if (File.Exists(xmlPath5))
		{
		Debug.Log("Great! I have found the file!");
		}
		else
		{
		Debug.Log("Sorry!");
		}
		itemList = XmlCircuitItemCollection.Load(xmlPath5).toCircuitItems();

		//			Debug.Log("circuitItems_size == " + itemList.Count);

		//			foreach(CircuitItem ci in itemList )
		//			{
		//				Debug.Log("CircuitItem_info == " + ci.name);
		//			}
		#endif

		//		Debug.Log ("=========");
		//
		//		for (int i = 0; i < itemList_temp.Count; i++) {
		//			Debug.Log ("item["+i+"]:"+itemList_temp [i].name+"  item["+i+"].connect_left:"+itemList_temp[i].connect_left+"  item["+i+"].connect_right:"+itemList_temp[i].connect_right);
		//		}
		//
		//		Debug.Log ("======end===");
		/////////////////////////////////////////

	}


    void Start() 
	{
		cf_SPDT = new CurrentFlow_SPDTSwitch ();
		cf = new CurrentFlow ();
		rotateCamera = new RotateCamera ();  
		// Intialize RecogniazeAlgo
		recognizeAlgo = new RecognizeAlgo();

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
		webCamTexture.Play ();//starts the camera

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
		TakePhoto();//拍照
		if (!isFinishHandlePicture && tempImgs.Count >= 10) //如果已经拍了10张图片且图片没有处理
		{
			TakePicture_Start ();//处理图片
			isFinishHandlePicture = true;
		}
	}

	public void TakePicture()
	{
		isTakePicture = true; 
	}

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

			if (isTakePicture && tempImgs.Count < 10)
			{
				tempImgs.Add (tmpImg);
			}
			matList=recognizeAlgo.createDataSet(tmpImg);//切割图片
			//Debug.Log ("matList.Count=="+matList.Count);

			#region call lua func with double/float paras
			if (matList.Count>0) 
			{
				for (int i = 0; i < matList.Count; i++) 
				{
					texture.Resize(28,28);
					//Imgproc.cvtColor (matList [i], matList [i], Imgproc.COLOR_BGR2RGB);
					Utils.matToTexture2D(matList[i],texture);
					Predict(matList[i]); 
				}

			}
			else 
			{
				texture.Resize(tmpImg.cols(),tmpImg.rows());
				Utils.matToTexture2D(tmpImg,texture);

			}
			#endregion

			#region save item pics into pad album
			//save item pics into pad album
			if (matList.Count > 0) 
			{

				for (int i = 0; i <1/* matList.Count*/; i++) { //把小图片存到相册中 
					#if UNITY_EDITOR 
					string path = Application.dataPath + "/Photos/" + System.DateTime.Now.Ticks + ".jpg";
					#elif UNITY_IPHONE 
					string path =Application.persistentDataPath+"/"+System.DateTime.Now.Ticks+".jpg";
					#endif 

					texture.Resize(28, 28);
					Utils.matToTexture2D (matList [i], texture);

					File.WriteAllBytes (path, texture.EncodeToJPG ());

					#if UNITY_EDITOR 
					#elif UNITY_IPHONE  
					_SavePhoto (path);
					#endif 
				}

			} 
			else 
			{
				texture.Resize(tmpImg.cols(),tmpImg.rows());
				Utils.matToTexture2D(tmpImg,texture);

			}
			#endregion
        
			if (!isShotTook) 
			{
				TakeSnapShot ();
				isShotTook = true;
			}
        }

    }

	public int Predict(Mat mat)
	{
		int ret = -1;
		mat.get(0, 0, arr);
	
		for (int i = 0; i < arr.Length; i++) 
		{
			sample [i] = (double)arr [i]/255;
		}
		//ret = testLuaWithArr(sample,sample.Length);
		//Debug.Log ("unity__testLuaWithArr_ret=="+ ret);
		return ret;
	}
	//开启线程的地方
	private void TakePicture_Start()
	{
		Debug.Log ("TakePicture_Start");
		Thread takePicture = new Thread (ThreadTakePicture);
		takePicture.IsBackground = true;
		takePicture.Start ();
	}
		
	public void SavePic()
	{
		Texture2D tex2D = new Texture2D (img.cols(), img.rows());
		Utils.matToTexture2D (img, tex2D);
		tex2D.Apply ();
		#if UNITY_EDITOR  
		string path = Application.dataPath +"/PaiZhao/" + "savepic.jpg";
		#elif UNITY_IPHONE 
		string path =Application.persistentDataPath+"/savepic.jpg";
		#endif 

//		File.WriteAllBytes(path, tex2D.EncodeToJPG ());
//		byte[] arr = File.ReadAllBytes (path);


		//for (int i = 0; i < tempImgs.Count; i++) 
//		{
//			matList=recognizeAlgo.createDataSet(tempImgs[0]);//把拍摄到的图片切成小图片
//			Debug.Log("matList.Count==="+matList.Count);
//			Debug.Log("------createDataSet()------");
//		}

		//把小图片存到相册中 to do ...
//		foreach (Mat item in matList) 
//		{
//			Texture2D texture = new Texture2D (item.cols(), item.rows());
//			Utils.matToTexture2D (item,texture);//mat转为texture存起来
//			texture.Apply();
//			textureList.Add (texture);
//		}
//		Debug.Log ("textureList.Count====" + textureList.Count);
//		for (int j = 0; j < textureList.Count; j++) 
//		{
//			#if UNITY_EDITOR  
//			string path01 = Application.dataPath +"/Photos/" + j+".jpg";
//			#elif UNITY_IPHONE 
//			string path01 =Application.persistentDataPath+ j+"/.jpg";
//			_SavePhoto (path01);
//			#endif 
//			File.WriteAllBytes(path01, textureList[j].EncodeToJPG ());
//			Debug.Log ("save to JPG---"+j);
//		}

	}



	//线程函数,此函数用于处理已经获得的照片
	private void ThreadTakePicture()
	{
		Debug.Log ("ThreadTakePicture()");
		imageList.Clear ();       
		for (int i = 0; i < tempImgs.Count; i++) 
		{
			itemList.Clear (); 
			Mat resultImg = recognizeAlgo.process(tempImgs[i], ref itemList);
			//////// for test ...should be deleted when distribute
			for (int j= 0; j < itemList_temp.Count; j++) 
			{
				itemList.Add(itemList_temp[j]);
			} 
			img = resultImg;
			imageList.Add (itemList);
//			matList=recognizeAlgo.createDataSet(tempImgs[i]);//把拍摄到的图片切成小图片
			//把小图片存到相册中 to do ...
//			foreach (Mat item in matList) 
//			{
//				Texture2D texture = new Texture2D (img.cols(), img.rows());
//				Utils.matToTexture2D (item,texture);//mat转为texture存起来
//				//texture.Apply();
//				textureList.Add (texture);
//			}
//			for (int j = 0; j < textureList.Count; j++) 
//			{
//				#if UNITY_EDITOR  
//				string path = Application.dataPath +"/Photos/" + j+".jpg";
//				#elif UNITY_IPHONE 
//				string path =Application.persistentDataPath+ j+"/.jpg";
//				_SavePhoto (path);
//				#endif 
//				File.WriteAllBytes(path, textureList[j].EncodeToJPG ());
//			}

		}
		bool compute_result;
		if (LevelManager.currentLevelData.LevelID == 15) 
		{
			compute_result = cf_SPDT.compute (ref itemList);
		} 
		else 
		{
			compute_result = cf.compute (ref itemList,LevelManager.currentLevelData.LevelID);
		}
		result = compute_result;//这个结果用于识别界面做判断是跳转到welldone界面还是failure界面
//		Debug.Log ("-----compute_result---:" + compute_result);
//		Debug.Log ("after compute ----$$$$$$ " + itemList.Count+" $$$$$$$$");
//		Debug.Log ("&&&&&&&&");
//		for (int k = 0; k < itemList.Count; k++) 
//		{
//			Debug.Log(k + " " + itemList[k].list[0] + " "  + itemList[k].powered);
//		}
//		Debug.Log ("&&&&&&&&&&&");
		isHandleDone_ItemList = true;



		//打印拍摄界面数据处理完后获取到的线上的点的个数和坐标
		for (int i = 3; i < itemList.Count; i++) {
			Debug.Log("itemList["+i+"] count==="+itemList[i].list.Count);
			Debug.Log("￥￥￥￥￥￥￥￥￥￥￥￥￥￥￥after comute pos ￥￥￥￥￥￥￥￥￥￥￥￥￥￥");
			for (int pi = 0; pi < itemList[i].list.Count; pi++) {
				Debug.Log(itemList[i].list[pi]);
			}
		}

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