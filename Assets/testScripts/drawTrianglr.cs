using UnityEngine;
using System.Collections;

public class drawTrianglr : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//通过object对象名 face 得到网格渲染器对象
		MeshFilter meshFilter = (MeshFilter)GameObject.Find("face").GetComponent(typeof(MeshFilter));

		//通过渲染器对象得到网格对象
		Mesh mesh = meshFilter.mesh;

		//API中写的不是提清楚，我详细的在说一遍

		//设置顶点，这个属性非常重要
		//三个点确定一个面，所以Vector3数组的数量一定是3个倍数
		//遵循顺时针三点确定一面
		//这里的数量为6 也就是我创建了2个三角面
		//依次填写3D坐标点
		mesh.vertices = new Vector3[] {new Vector3(5, 0, 0), new Vector3(0, 5, 0), new Vector3(0, 0, 5),new Vector3(-5, 0, 0), new Vector3(0, -5, 0), new Vector3(0, 0, -5)};

		//设置贴图点，因为面确定出来以后就是就是2D 
		//所以贴纸贴图数量为Vector2 
		//第一个三角形设置5个贴图
		//第二个三角形设置一个贴图
		//数值数量依然要和顶点的数量一样
		mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 5), new Vector2(5, 5),new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};

		//设置三角形索引，这个索引是根据上面顶点坐标数组的索引
		//对应着定点数组Vector3中的每一项
		//最后将两个三角形绘制在平面中
		//数值数量依然要和顶点的数量一样
		mesh.triangles= new int []{0,1,2,3,4,5};
	
	}
}
