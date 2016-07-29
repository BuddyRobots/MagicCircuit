using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;  

public class cvDllTest : MonoBehaviour {



	//[DllImport(“dllTest”)] 
	//[DllImport("dllTest")]
	[DllImport("TestDll")]
	private static extern int add (int a, int b);



	void Start()
	{

		//int c = add(1, 2);
		//print ("c==" + c);
	}

	void Update()
	{
		

	}
}
