using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketServer : MonoBehaviour {


	void Start () {
		
		OpenServer();
	}

	void OnGUI()
	{

		GUILayout.Label("服务器");

	}


	void OpenServer()
	{

		IPAddress ipAddr=IPAddress.Parse("192.168.1.110");
		IPEndPoint ipEp=new IPEndPoint(ipAddr,8899);
		Socket serverSocket=new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);//获得一个socket描述
		serverSocket.Bind(ipEp);//用bind（）将socket绑定到一个网络地址（一般都是本机的IP地址）
		serverSocket.Listen(20);//用listen（）开始在某个端口监听

		while (true) {

			Socket client=serverSocket.Accept();//accept（）等待客户连接，如果客户端用connect（）函数连接服务器时accept（）会获得该客户端（socket）
			byte[] request=new byte[512];
			int byteRead=client.Receive(request);//接受数据
			string input=Encoding.UTF8.GetString(request,0,byteRead);
			print("server request: "+input);
			string output="connect server successfully~~~~";
			byte[] concent=Encoding.UTF8.GetBytes(output);
			client.Send(concent);//发送数据
			client.Shutdown(SocketShutdown.Both);
			client.Close();
		
		}


	}
}
