using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketClient : MonoBehaviour {


	void Start () 
	{
		ConnectServer();
	
	}


	void OnGUI()
	{
		GUILayout.Label("client");

	}


	
	void ConnectServer()
	{
		IPAddress ipAddr=IPAddress.Parse("192.168.1.110");
		IPEndPoint ipEp=new IPEndPoint(ipAddr,8899);

		Socket clientSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

		clientSocket.Connect(ipEp);//连接到远程主机

		string output="client request to connect.....";
		byte[] concent=Encoding.UTF8.GetBytes(output);
		clientSocket.Send(concent);

		byte[] response=new byte[1024];
		int bytesRead=clientSocket.Receive(response);
		string input=Encoding.UTF8.GetString(response,0,bytesRead);
		print ("client request: "+ input);

		clientSocket.Shutdown(SocketShutdown.Both);
		clientSocket.Close();


	}
}
