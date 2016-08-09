using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class test : MonoBehaviour {



	public List<Vector3> vec = new List<Vector3> ();
	private Vector3 target;
	private Vector3 normal;

	public bool isRun = true;
	public LineManager manager;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		DrawLine ();
	}

	//设置一段线的数据

	public void Setting(Vector3 to,Vector3 from){
		this.transform.position=from;
		this.GetComponent<LineRenderer>().SetPosition(0,from);
		this.GetComponent<LineRenderer>().SetPosition(1,from);

		target=to;
		//向量的减法运算
		normal = (target - transform.position).normalized;
		manager=GameObject.Find("Manager").GetComponent<LineManager>();
	}
	//划线的核心

	public void DrawLine(){

		if(isRun){
			if (Vector3.Distance(transform.position, target) > 0.1)
			{
				//向量的加法运算
				transform.position = transform.position + normal * 0.1f;
				vec.Add(transform.position);
			}
			else
			{
				transform.position = target;

				vec.Add(transform.position);
				isRun=false;
				manager.isFinished=true;
			}
			setpos();
		}



	}

	//更变LineRenderer的终点实现动态划线
	public void setpos()
	{

		for(int i=0;i<vec.Count;i++){
			//				print (i+vec[i].ToString());
			this.GetComponent<LineRenderer>().SetPosition(1,vec[i]);
		}


	}




}
