using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class testFeil : MonoBehaviour {

	[DllImport("__Internal")]
	private static extern void c_ctest();



	void OnGUI()  
	{  
		//开始按钮  创建了一个按钮。  
		if(GUI.Button(new Rect(20,100,200,50),"qingyun "))  
		{  
			
			print("hello qingyun !");
			c_ctest();  

		}  

	} 

	void testResult (string msg)
	{
		Debug.Log("testResult:" +msg);
	}

	//这个是C#里的一个回调。用来接收数据是否传送成功。----这里的作用就是OC里的回调。
	void testBtnResult (string msg)
	{
		Debug.Log ("btnPressSuccessssssssssss:"+msg);
	}
}

