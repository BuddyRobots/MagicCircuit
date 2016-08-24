using UnityEngine;
using System.Collections;

public class Sliderctrl : MonoBehaviour 
{
	public UISlider HminSlider;
	public UISlider HmaxSlider;
	public UISlider SminSlider;
	public UISlider SmaxSlider;
	public UISlider VminSlider;
	public UISlider VmaxSlider;
	public UISlider AreaSlider;

	public UILabel HminLabel;
	public UILabel HmaxLabel;
	public UILabel SminLabel;
	public UILabel SmaxLabel;
	public UILabel VminLabel;
	public UILabel VmaxLabel;
	public UILabel AreaLabel;

	private int h_min = 0, h_max = 180;
	private int s_min = 0, s_max = 255;
	private int v_min = 0, v_max = 255;
	private int area = 30000;


	private string componentName;







	void Start()
	{



	}

	public void SetThreshhold(string name)
	{
		componentName = name;
		if (PlayerPrefs.HasKey(name + "_h_min"))
			loadThres(name);
		else
			saveThres(name);

	}

	public void saveThres(string name)
	{
		PlayerPrefs.SetInt(name + "_h_min", h_min);
		PlayerPrefs.SetInt(name + "_h_max", h_max);
		PlayerPrefs.SetInt(name + "_s_min", s_min);
		PlayerPrefs.SetInt(name + "_s_max", s_max);
		PlayerPrefs.SetInt(name + "_v_min", v_min);
		PlayerPrefs.SetInt(name + "_v_max", v_max);
		PlayerPrefs.SetInt(name + "_area", area);
	}

	public void loadThres(string name)
	{
		HminSlider.value  = (float)PlayerPrefs.GetInt(name + "_h_min");
		HmaxSlider.value  = (float)PlayerPrefs.GetInt(name + "_h_max");
		SminSlider.value  = (float)PlayerPrefs.GetInt(name + "_s_min");
		SmaxSlider.value  = (float)PlayerPrefs.GetInt(name + "_s_max");
		VminSlider.value  = (float)PlayerPrefs.GetInt(name + "_v_min");
		VmaxSlider.value  = (float)PlayerPrefs.GetInt(name + "_v_max");
		AreaSlider.value  = (float)PlayerPrefs.GetInt(name + "_area");
	}    

	public void ChangeHmin()
	{
		HminLabel.text=((int)(HminSlider.value*180)).ToString();

	}
	public void ChangeHmax()
	{
		HmaxLabel.text=((int)(HmaxSlider.value*180)).ToString();

	}
	public void ChangeSmin()
	{
		SminLabel.text=((int)(SminSlider.value*255)).ToString();

	}
	public void ChangeSmax()
	{
		SmaxLabel.text=((int)(SmaxSlider.value*255)).ToString();

	}
	public void ChangeVmin()
	{
		VminLabel.text=((int)(VminSlider.value*255)).ToString();

	}
	public void ChangeVmax()
	{
		VmaxLabel.text=((int)(VmaxSlider.value*255)).ToString();

	}
	public void ChangeArea()
	{
		AreaLabel.text=((int)(AreaSlider.value*30000)).ToString();

	}





}
