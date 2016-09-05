using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemoveLine : MonoBehaviour {
	[HideInInspector]
	public bool isRemoveLine=false;

	//private List<GameObject> arrows = null;
	private List<Vector3> pointsList = null;
	private List<List<Vector3>> lines=null;//all lines

	//Vector3	pos=Vector3.zero;
	//private bool isFingerShow = false;


	void OnEnable()
	{
		
		pointsList = PhotoRecognizingPanel._instance.linePointsList;


		int max=pointsList.Count-1;
		int index=Random.Range(0,max);
		//pos=pointsList[index];

		//isFingerShow = false;
	}

	void Update () 
	{
		if (isRemoveLine) 
		{
			//arrows = PhotoRecognizingPanel._instance.arrowList;
			lines = PhotoRecognizingPanel._instance.lines;
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
				transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulb-on";
				GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(lines,0);
				GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;
				//动画播放3秒后，在电线上的任意随机点位置出现小手
				Vector3	randomPos = GetComponent<PhotoRecognizingPanel> ().ChooseRandomPoint ();
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(randomPos);
			}

			if (Input.GetMouseButtonDown(0)) 
			{
				Ray ray = transform.parent.Find ("Camera").GetComponent<Camera> ().ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) 
				{
					GameObject go = hit.collider.gameObject;
					if (go.name.Contains ("line")) 
					{ //如果碰到的是线，线就消失，电流消失
						Destroy (go);
						Destroy (GetComponent<PhotoRecognizingPanel> ().finger);
						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbDark";
						GetComponent<PhotoRecognizingPanel> ().StopCreateArrows();
						foreach (GameObject item in GetComponent<PhotoRecognizingPanel> ().arrowList)
						{
							Destroy (item);
						}

					}

				}
			}

		}
	}
}
