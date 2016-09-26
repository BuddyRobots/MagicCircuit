using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveCtrl : MonoBehaviour 
{

	private bool isAtDest=false;
	private int index;
	/// <summary>
	/// 一条线上点的集合的记录
	/// </summary>
	private List<Vector3> line;

	private ArrowCtrl arrowCtrl;

	//public bool isArrowDestroy;


	void OnEnable()
	{
		
		arrowCtrl = transform.GetComponent<ArrowCtrl> ();

		index = 0;
		//isArrowDestroy=false;
	}
	

	void Update () 
	{
		if (line.Count > 0) {
			
		
			isAtDest = arrowCtrl.IsAtDest ();
			if (isAtDest)
			{//如果箭头移动到了目的地，重新设置目的地
				index++;
				arrowCtrl.SetDestination (line [index % line.Count], 2);
			}

			if (Vector3.Distance (transform.localPosition, line [line.Count - 1]) < 2) 
			{// 如果箭头移动到了线路的终点，就销毁这个箭头
				
				Destroy (gameObject);
				//箭头销毁，线上的bool值改变
				//isArrowDestroy=true;

			}

		}
	}

	//private bool isTimeToDestroyArrow=false;


//	IEnumerator StopForAwhile()
//	{
//
//		yield return new WaitForSeconds (0.4f);
//		isTimeToDestroyArrow = true;
//	}

	/// <summary>
	/// 沿着线移动
	/// </summary>
	/// <param name="line">Line.</param>
	public void Move(List<Vector3> line)
	{

		this.line = line;
		arrowCtrl.SetDestination (line [++index]);

	}

	/// <summary>
	/// 线上的箭头都停止移动
	/// </summary>
	public void Stop()
	{
		arrowCtrl.Stop ();
	}

	/// <summary>
	/// 线上箭头停止移动
	/// </summary>
	public void ContinueStart()
	{
		arrowCtrl.ContinueStart ();
	}
}
