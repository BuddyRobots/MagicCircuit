using UnityEngine;
using System.Collections;

public class moveTest: MonoBehaviour
{	

	void Start()
	{

		//键值对儿的形式保存iTween所用到的参数
		Hashtable args = new Hashtable();

		//这里是设置类型，iTween的类型又很多种，在源码中的枚举EaseType中
		//例如移动的特效，先震动在移动、先后退在移动、先加速在变速、等等
		args.Add("easeType", iTween.EaseType.easeInOutExpo);

		//移动的速度，
		args.Add("speed",5f);
		//移动的整体时间。如果与speed共存那么优先speed
		args.Add("time",1f);


		//三个循环类型 none loop pingPong (一般 循环 来回)	
		//args.Add("loopType", "none");
		//args.Add("loopType", "loop");	
		args.Add("loopType", "pingPong");

		//处理移动过程中的事件。


		// x y z 标示移动的位置。

		args.Add("x",1);
		args.Add("y",1);
		args.Add("z",0);

		//当然也可以写Vector3
		//args.Add("position",Vectoe3.zero);

		//最终让改对象开始移动
		iTween.MoveTo(gameObject,args);	
	}



}