using UnityEngine;
using System.Collections;

public class client : MonoBehaviour {

	string ip="127.0.0.1";
	int port=10000;




	void OnGUI()
	{


		switch(Network.peerType)
		{
		//禁止客户端连接运行, 服务器未初始化  
		case NetworkPeerType.Disconnected:  
			StartConnect();  
			break;  
			//运行于服务器端  
		case NetworkPeerType.Server:  
			break;  
			//运行于客户端  
		case NetworkPeerType.Client:  
			break;  
			//正在尝试连接到服务器  
		case NetworkPeerType.Connecting:  
			break;  
		}


	}



	void StartConnect(){  
		if (GUILayout.Button("连接服务器")){  
			NetworkConnectionError error = Network.Connect(ip,port);  
			Debug.Log("连接状态"+error);  
		}  
	} 
}
