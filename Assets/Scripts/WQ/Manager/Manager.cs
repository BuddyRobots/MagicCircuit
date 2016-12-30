using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using MagicCircuit;

public class Manager : AllSceneSinglton<Manager>
{
	public static bool isMusicOn=true;
	[HideInInspector]
	public AudioSource bgAudio;
	[HideInInspector]
	public GameObject manager;


	public Texture texture_img;

	public List<CircuitItem> itemList = new List<CircuitItem>();

	public bool isTreadEnd=false;
	public bool isCircuitCorrect = false;

	void Start () 
	{
		manager=this.gameObject;
		Manager.isMusicOn=true;

		isTreadEnd=false;
		isCircuitCorrect = false;

		bgAudio=GameObject.Find("Manager").GetComponent<AudioSource>();
	}
		

	void Update () 
	{
		if (Manager.isMusicOn)
		{
			
			if (!bgAudio.isPlaying) 
			{
				bgAudio.Play ();
			}
		}
		if (!Manager.isMusicOn )
		{
			
			//关闭音乐 
			if (bgAudio.isPlaying) 
			{
				Debug.Log("pause");
				bgAudio.Pause ();
			}
		}

	}
		

}
