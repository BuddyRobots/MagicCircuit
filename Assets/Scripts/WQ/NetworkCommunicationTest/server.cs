using UnityEngine;
using System.Collections;

public class server : MonoBehaviour {


	int port=10000;

	void OnGUI()
	{

		switch(Network.peerType)
		{

		case NetworkPeerType.Disconnected:
			StartServer();
			break;
		case NetworkPeerType.Server:
			OnServer();
			break;
		case NetworkPeerType.Client:
			break;
		case  NetworkPeerType.Connecting:
			break;

		}

	}


	void StartServer()
	{
		//当用户点击按钮的时候为true  
		if (GUILayout.Button("create server")) 
		{
			//初始化本机服务器端口，第一个参数就是本机接收多少连接  
			NetworkConnectionError error=Network.InitializeServer(12,port,false);
			Debug.Log("错误日志"+error);
		}

	}

	void OnServer()
	{
		GUILayout.Label("the server is running, waiting for client connection");
		//Network.connections是所有连接的玩家, 数组[]  
		//取客户端连接数. 
		int length=Network.connections.Length;
		//按数组下标输出每个客户端的IP,Port  
		for (int i = 0; i < length; i++) {
			GUILayout.Label("client "+i);
			GUILayout.Label("client IP: "+Network.connections[i].ipAddress);
			GUILayout.Label("client port: "+Network.connections[i].port);
		}
		if (GUILayout.Button("break the server")) {
			Network.Disconnect();
		}

	}



}
