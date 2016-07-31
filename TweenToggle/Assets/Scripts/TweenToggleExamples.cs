using UnityEngine;
using System.Collections;

public class TweenToggleExamples : MonoBehaviour {

	public TweenToggle positionTweenToggle;
	public TweenToggle rotationTweenToggle;
	public TweenToggle alphaTweenToggle;
	public TweenToggle scaleTweenToggle;

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

}
