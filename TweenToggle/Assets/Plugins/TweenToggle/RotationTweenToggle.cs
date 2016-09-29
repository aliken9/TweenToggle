﻿//// Copyright (c) Thalassian Studios
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
			Debug.Log(hiddenEulerAngle);
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
				tweenID = LeanTween.rotateLocal(gameObject, new Vector3(0, 0, showingEulerAngle.z), time)	// Special case for GUI rotation
					.setEase(showEase)
					.setDelay(showDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(ShowSendCallback)
					.id;
			}
			else{
				tweenID = LeanTween.rotateLocal(gameObject, showingEulerAngle, time)
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
				tweenID = LeanTween.rotateLocal(gameObject, hiddenEulerAngle, time)
					.setEase(hideEase)
					.setDelay(hideDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(ShowSendCallback)
					.id;
			}
			else{
				tweenID = LeanTween.rotateLocal(gameObject, hiddenEulerAngle, time)
					.setEase(hideEase)
					.setDelay(hideDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(HideSendCallback)
					.id;
			}
		}
	}
}
