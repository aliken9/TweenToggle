// TweenToggle version 0.1 - https://pixelmetry.com/projects/unity-tweentoggle-plugin/
// Copyright (C) 2016 Wenshiang Sean Chung - Pixelmetry
using UnityEngine;
using System.Collections;

/// <summary>
/// Rotation tween toggle.
/// Child of TweenToggle parent, Takes care of rotation toggles
/// </summary>
public class RotationTweenToggle : TweenToggle {
	[Header("Tween Delta")]
	public float hideDeltaX;
	public float hideDeltaY;
	public float hideDeltaZ;

	protected Vector3 hiddenEulerAngle;
	protected Vector3 showingEulerAngle;

	protected override void RememberPositions(){
		if(isGUI){
			showingEulerAngle = GUIRectTransform.localEulerAngles;
			hiddenEulerAngle = GUIRectTransform.localEulerAngles + new Vector3(hideDeltaX, hideDeltaY, hideDeltaZ);
		}
		else{
			showingEulerAngle = gameObject.transform.localEulerAngles;
			hiddenEulerAngle = gameObject.transform.localEulerAngles + new Vector3(hideDeltaX, hideDeltaY, hideDeltaZ);
		}
	}
	
	public override void Reset(){
		if (startsHidden){
			if(isGUI){
				GUIRectTransform.localEulerAngles = hiddenEulerAngle;
			}
			else{
				gameObject.transform.localEulerAngles = hiddenEulerAngle;
			}

		 	// Need to call show first
			isShown = false;
			isMoving = false;
		}
		else{
			// Need to call hide first
			isShown = true;
			isMoving = false;
		}
	}
	
	public override void Show(float time){
		if(!isShown){
			isShown = true;
			isMoving = true;

			LeanTween.cancel(tweenID);

			if(isGUI){
				if(showingEulerAngle.x != 0f || showingEulerAngle.z != 0f){
					Debug.LogWarning("GUI rotation will only rotate z-axis");
				}
				tweenID = LeanTween.value(gameObject, SetRotation, hiddenEulerAngle, showingEulerAngle, time)
					.setEase(showEase)
					.setDelay(showDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(ShowSendCallback)
					.id;
			}
			else{
				tweenID = LeanTween.value(gameObject, SetRotation, hiddenEulerAngle, showingEulerAngle, time)
					.setEase(showEase)
					.setDelay(showDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(ShowSendCallback)
					.id;
			}
		}
	}

	public override void Hide(float time){
		if(isShown){
			isShown = false;
			isMoving = true;
            
			LeanTween.cancel(tweenID);

			if(isGUI){
				if(showingEulerAngle.x != 0f || showingEulerAngle.z != 0f){
					Debug.LogWarning("GUI rotation will only rotate z-axis");
				}
				tweenID = LeanTween.value(gameObject, SetRotation, showingEulerAngle, hiddenEulerAngle, time)
					.setEase(hideEase)
					.setDelay(hideDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(ShowSendCallback)
					.id;
			}
			else{
				tweenID = LeanTween.value(gameObject, SetRotation, showingEulerAngle, hiddenEulerAngle, time)
					.setEase(hideEase)
					.setDelay(hideDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(HideSendCallback)
					.id;
			}
		}
	}

	private void SetRotation(Vector3 value){
		gameObject.transform.localEulerAngles = value;
	}
}
