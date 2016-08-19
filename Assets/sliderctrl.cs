using UnityEngine;
using System.Collections;

public class sliderctrl : MonoBehaviour {
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


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
