using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Move : MonoBehaviour {

	public List<Transform> pos;//箭头行走的路径

	public int index;
	[Range(0,1)]
	public float duration;

	// Use this for initialization
	void Start () {
		index = 0;
		this.transform.position = pos [0].position;
		//duration = 0.5f;
		MoveAround ();
	}
	

	public void MoveAround()
	{

		if (index < pos.Count-1) {
		
		
			index++;
			Tweener tween=this.transform.DOMove(pos[index].position,duration);

			//箭头指向目标的核心

			Vector3 looknor=new Vector3((pos[index].transform.position.x-pos[index-1].transform.position.x),(pos[index].transform.position.y-pos[index-1].transform.position.y),0f).normalized;
			this.transform.up=looknor;



			tween.OnComplete(MoveAround);
		}
	}
}
