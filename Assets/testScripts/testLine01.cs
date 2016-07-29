using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class testLine01 : MonoBehaviour {

	private LineRenderer lr;
	private List<GameObject> pointPos;
	private GameObject pointer;
	private int lineSeg=5;
	void Start () 
	{
		gameObject.SetActive(false);
		pointer=Resources.Load("Prefab/ConfirmBtn") as GameObject;
		lr = gameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;            
		lr.SetWidth(0.1f,0.1f);
	}

	// Update is called once per frame
	void Update () {    
		if(Input.GetMouseButtonUp(0)){

			//Get click position
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			lr.SetVertexCount(lineSeg+1);
			RaycastHit rh;
			if(Physics.Raycast(ray,out rh)){                
				pointPos.Add(DrawLine(rh));
			}            
		}

		if(Input.GetMouseButtonUp(1)){
			//destroy point
			DestroyLine();
		}


	}


	GameObject DrawLine(RaycastHit pointPos){

		//Display point
		GameObject gb_pointer = GameObject.Instantiate(pointer) as GameObject;
		gb_pointer.transform.position =pointPos.point + (transform.position - pointPos.point) * 0.01f; 
		gb_pointer.transform.rotation = Quaternion.LookRotation (pointPos.normal, Camera.main.transform.up);
		Vector3 laserpos = new Vector3();
		laserpos.x= 90.0f;
		laserpos.y= gb_pointer.transform.position.y;
		laserpos.z= gb_pointer.transform.position.z;
		gb_pointer.transform.eulerAngles = laserpos;
		lr.SetPosition(lineSeg,pointPos.point);   //设置目标点的坐标，使用的是world坐标系
		lineSeg++;
		return gb_pointer;
	}

	void DestroyLine(){

		int arrayLength = pointPos.Count;
		if(arrayLength > 0){
			GameObject.Destroy(pointPos[arrayLength-1]);
			pointPos.RemoveAt(arrayLength-1);        
			lr.SetVertexCount(--lineSeg);
		}
	}
}
