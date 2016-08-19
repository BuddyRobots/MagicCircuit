using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FingerSpriteMove : MonoBehaviour {
	public Vector3 offsetPos;

	//手指的原始位置----150，95，0        to   66,160,0

	private Vector3 toPos;
	private Vector3 fromPos;


	 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition += offsetPos;
	}
}
