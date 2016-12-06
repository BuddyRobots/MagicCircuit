using UnityEngine;
using System.Collections;
//level 1
public class LevelOne : MonoBehaviour 
{
	/// <summary>
	///执行该脚本中update函数中的方法的标志
	/// </summary>
	[HideInInspector]
	public bool isPlayCircuitAnimation=false;

	void Update () 
	{
		if (isPlayCircuitAnimation)
		{
			CommonFuncManager._instance.OpenCircuit ();
		}
	}

}
