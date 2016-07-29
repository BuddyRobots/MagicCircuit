
using UnityEngine;
using System.Collections;

public class DrawLineTest : MonoBehaviour 
{
	void Update() 
	{
		Debug.DrawLine(Vector3.zero, new Vector3(1, 0, 0), Color.red);
	}
}