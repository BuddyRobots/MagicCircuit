using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}




	string str="";
	Vector2 sc=new Vector2(0,0);
	void OnGUI()
	{
		

//		GUILayout.BeginScrollView(sc,GUILayout.Width(500),GUILayout.Height(400));
//
//
//
//		GUILayout.Box(str);
//		str=GUILayout.TextArea(str);
//
//
//		GUILayout.EndScrollView();

		GUILayout.BeginArea(new Rect(new Vector2(100,100),new Vector3(200,200)));
//		GUILayout.BeginScrollView(sc,GUILayout.Width(50),GUILayout.Height(50));

		GUILayout.Label(str);
		str=GUILayout.TextArea(str);

//		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
}
