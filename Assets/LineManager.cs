using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineManager : MonoBehaviour {

	public List<Transform> pos=new List<Transform>();
	public int index;

	public GameObject linePrefabs;

	public bool isFinished=true;
	// Use this for initialization

	//只需要把所有的点加入到pos 即可完成划线
	void Start () {
		index=1;
	}

	// Update is called once per frame
	void Update () {
		if(isFinished&&index<pos.Count){
			SetLine();
			index+=1;
		}
	}

	public void SetLine(){
		GameObject  go=GameObject.Instantiate(linePrefabs) as GameObject;



		go.AddComponent<test>();
		go.GetComponent<test>().Setting(pos[index].position,pos[index-1].position);
		go.SetActive(true);
		isFinished=false;



	}

}
