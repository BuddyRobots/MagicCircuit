using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class panelTest : MonoBehaviour {
	private List<Vector3> pos=new List<Vector3>();

	private Vector3 fromPos;
	private Vector3 toPos;
	private Vector3 centerPos;

	private float distance = 0;
	private float angle = 0;

	private GameObject lineParent;
	private GameObject linePrefab;

	private float timer = 0;
	private float time = 1f;

	private int index = 0;

	void Awake()
	{

		pos.Add (new Vector3 (1f, 0, 0));
		pos.Add (new Vector3 (10, 1, 0));
		pos.Add (new Vector3 (20, 4, 0));
		pos.Add (new Vector3 (30, 10, 0));
		pos.Add (new Vector3 (40, 20, 0));
		pos.Add (new Vector3 (50, 30, 0));
		pos.Add (new Vector3 (60, 40, 0));
		pos.Add (new Vector3 (70, 50, 0));
		pos.Add (new Vector3 (80, 60, 0));
		pos.Add (new Vector3 (90, 70, 0));


		if (pos.Count == 1) 
		{
		
			fromPos = toPos = pos [0];
		} 
		else 
		{
			fromPos = pos [index];
			toPos = pos [index + 1];

		}
	}

	void Start () 
	{
		lineParent = this.gameObject;
		linePrefab = Resources.Load ("Line") as GameObject;
		
		distance = Vector3.Distance (fromPos, toPos);
		centerPos = Vector3.Lerp (fromPos, toPos, 0.5f);
		angle = TanAngle (fromPos, toPos);


		for (int i = 0; index < pos.Count - 1; i++) {
			DrawLine ();
		}
	}
	

	void Update () {
		//timer += Time.deltaTime;
		//if(timer>=time)
		{
//			timer = 0;
//			if (index < pos.Count - 1) {
//				DrawLine ();
//			}
		}
		
	
	}


	private float TanAngle(Vector2 from, Vector2 to)
	{
		float xdis = to.x - from.x;//计算临边长度  
		float ydis = to.y - from.y;//计算对边长度  
		float tanValue = Mathf.Atan2(ydis, xdis);//反正切得到弧度  
		float nnangle = tanValue * Mathf.Rad2Deg;//弧度转换为角度  

		return nnangle;  

	}

	private void DrawLine()
	{

		GameObject lineGo = NGUITools.AddChild(lineParent, linePrefab);//生成新的连线  
		UISprite lineSp = lineGo.GetComponent<UISprite>();//获取连线的 UISprite 脚本  
		lineSp.width = (int)distance;//将连线图片的宽度设置为上面计算的距离  
		lineGo.transform.localPosition = centerPos;//设置连线图片的坐标  
		lineGo.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);//旋转连线图片  

		//画完一条以后 fromPos后移，toPos后移
		index++;

		fromPos = pos [index];
		if (index < pos.Count - 1)
		{
			toPos = pos [index + 1];
		} 
		else 
		{
		
			toPos = fromPos=pos[pos.Count-1];
		}
		distance = Vector3.Distance (fromPos, toPos);
		centerPos = Vector3.Lerp (fromPos, toPos, 0.5f);
		angle = TanAngle (fromPos, toPos);

	}

	IEnumerator dw()
	{
		for (int i = 0; i < pos.Count - 1; i++) {
			DrawLine ();

		}
		yield return new WaitForSeconds (0.2f);
		for (int i = 0; i < pos.Count - 1; i++) {
			DrawLine ();
			yield return new WaitForSeconds (0.2f);
		}
		for (int i = 0; i < pos.Count - 1; i++) {
			DrawLine ();
			yield return new WaitForSeconds (0.2f);
		}

	}
}
