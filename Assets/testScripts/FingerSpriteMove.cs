using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FingerSpriteMove : MonoBehaviour {
	public Vector3 offsetPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition += offsetPos;
	}
}
