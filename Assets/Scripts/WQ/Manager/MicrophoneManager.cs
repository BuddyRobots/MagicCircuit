using UnityEngine;
using System.Collections;

public class MicrophoneManager : MonoBehaviour {

	public static MicrophoneManager _instance;

	private int samplingRate=10000;

	private AudioSource _audio;

	const int RECORD_TIME = 5;

	public float loudness=0;  
	public float sensitivity=100;  
	private static string[] micArray = null;
	public bool isVoiceCollected=false;


	public AudioSource audio
	{
		get
		{
			if (_audio == null)
			{
				_audio = gameObject.AddComponent<AudioSource>();
			}
			return _audio;
		}
	}



	void Awake()
	{
		_instance = this;
	}
	private void  OnEnable()
	{
		isVoiceCollected = false;
	}

	void Start () 
	{
	    micArray = Microphone.devices;
		int deviceCount = micArray.Length;
		if (deviceCount == 0) 
		{
			Debug.Log ("no microphone found");
			return;
		}
	}
	void Update ()  
	{  
//		loudness = GetAveragedVolume () * sensitivity;  
//		if (loudness > 1)   
//		{  
//			isVoiceCollected = true;
//			Debug.Log("loudness = "+loudness);  
//		}  
	} 
		
	public void StartRecord()
	{
		audio.Stop();
		audio.loop = false;
		audio.mute = true;
		audio.clip = Microphone.Start(null, false, RECORD_TIME, samplingRate);    
		while (!(Microphone.GetPosition(null) > 0))
		{
		}
		audio.Play();
		Debug.Log("StartRecord"); 
		StartCoroutine(TimeDown());
	}

	private IEnumerator TimeDown()  
	{  
		Debug.Log(" IEnumerator TimeDown()");  

		int time = 0;  
		while (time < RECORD_TIME)  
		{  
			if (!Microphone.IsRecording (null))   
			{ //如果没有录制   
				Debug.Log ("IsRecording false");  
				yield break;  
			}  
			Debug.Log("yield return new WaitForSeconds "+time);  
			yield return new WaitForSeconds(1);  
			time++;  
		}  
		if (time >= RECORD_TIME)  
		{  
			Debug.Log("RECORD_TIME is out! stop record!");  
			StopRecord();  
			isVoiceCollected = true;
		}  
		yield return 0;  
	}  


	public void StopRecord()
	{
		if (!Microphone.IsRecording(null))
		{
			return;
		}
		Microphone.End(null);
		audio.Stop();
		Debug.Log("StopRecord"); 

	}

	public  float GetAveragedVolume()  
	{  
		float[] data=new float[256];  
		float a=0;  
		audio.GetOutputData(data,0);  
		foreach(float s in data)  
		{  
			a+=Mathf.Abs(s);  
		}  
		return a/256;  
	} 

	public void PrintRecord()
	{
		if (Microphone.IsRecording(null))
		{
			return;
		}
		byte[] data = GetClipData();
		string slog = "total length:" + data.Length + " time:" + audio.time;
		Debug.Log("PrintRecord");
	}

	public void PlayRecord()
	{
		if (Microphone.IsRecording(null))
		{
			return;
		}
		if (audio.clip == null)
		{
			return;
		}
		audio.mute = false;
		audio.loop = false;
		audio.Play();
		Debug.Log("PlayRecord");
	}

	public byte[] GetClipData()
	{
		if (audio.clip == null)
		{
			Debug.Log("GetClipData audio.clip is null");
			return null;
		}

		float[] samples = new float[audio.clip.samples];

		audio.clip.GetData(samples, 0);


		byte[] outData = new byte[samples.Length * 2];

		int rescaleFactor = 32767;

		for (int i = 0; i < samples.Length; i++)
		{
			short temshort = (short)(samples[i] * rescaleFactor);

			byte[] temdata = System.BitConverter.GetBytes(temshort);

			outData[i * 2] = temdata[0];
			outData[i * 2 + 1] = temdata[1];


		}
		if (outData == null || outData.Length <= 0)
		{
			Debug.Log("GetClipData intData is null");
			return null;
		}
		return outData;
	}

}
