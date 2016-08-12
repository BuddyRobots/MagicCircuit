using UnityEngine;
using System.Collections;

public class showTest : MonoBehaviour {



	private float timer01=0;
	private float time = 5;


	// Use this for initialization
	void Start () {
		timer01=0f;
		time = 5f;
		gameObject.GetComponent<UISprite> ().alpha = 0;
	}
	
	// Update is called once per frame
	void Update () {

		timer01 += Time.deltaTime;

		if (timer01 >= time) {
			timer01 = time;
		


		}
		gameObject.GetComponent<UISprite> ().alpha = Mathf.Lerp (0, 1f, (float)timer01 / time);
		//gameObject.GetComponent<UISprite> ().color = Color.Lerp (Color.red, Color.grey, timer01 / time);
		Debug.Log ("alpha===" + gameObject.GetComponent<UISprite> ().alpha);



	}
}
