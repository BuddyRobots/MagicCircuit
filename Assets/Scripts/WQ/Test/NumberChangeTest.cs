using UnityEngine;
using System.Collections;

public class NumberChangeTest : MonoBehaviour {

	UILabel countDown;
	private bool play=false;

	// Use this for initialization
	void Start () {
		countDown=gameObject.GetComponent<UILabel>();
//		countDown.gameObject.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!play) 
		{
			play=true;
			StartCoroutine(CountDown());

		}

	}

	IEnumerator CountDown()
	{
		countDown.gameObject.SetActive(true);
		//倒计时，每个数字停留一秒后变化
		countDown.text = "5";
		yield return new WaitForSeconds(1);
		countDown.text = "4";
		yield return new WaitForSeconds (1);
		countDown.text = "3";
		yield return new WaitForSeconds (1);
		countDown.text = "2";
		yield return new WaitForSeconds (1);
		countDown.text = "1";
		yield return new WaitForSeconds (1);
		countDown.text = " ";
		countDown.gameObject.SetActive(false);
//		play=false;
	}
}
