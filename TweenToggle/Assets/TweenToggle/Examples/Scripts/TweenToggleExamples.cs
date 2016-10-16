using UnityEngine;
using System.Collections;

public class TweenToggleExamples : MonoBehaviour {

	public TweenToggle positionTweenToggle;
	public TweenToggle rotationTweenToggle;
	public TweenToggle scaleTweenToggle;
	public TweenToggle alphaTweenToggle;

	public void OnPositionToggleButton(bool isOn){
		if(isOn){
			positionTweenToggle.Show();
		}
		else{
			positionTweenToggle.Hide();
		}
	}

	public void OnRotationToggleButton(bool isOn){
		if(isOn){
			rotationTweenToggle.Show();
		}
		else{
			rotationTweenToggle.Hide();
		}
	}

	public void OnScaleToggleButton(bool isOn){
		if(isOn){
			scaleTweenToggle.Show();
		}
		else{
			scaleTweenToggle.Hide();
		}
	}

	public void OnAlphaToggleButton(bool isOn){
		if(isOn){
			alphaTweenToggle.Show();
		}
		else{
			alphaTweenToggle.Hide();
		}
	}

}
