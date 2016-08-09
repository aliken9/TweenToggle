//// Copyright (c) Thalassian Studios
using UnityEngine;
using System.Collections;

/// <summary>
/// Rotation tween toggle.
/// Child of TweenToggle parent, Takes care of rotation toggles
/// </summary>
public class RotationTweenToggle : TweenToggle {

	protected override void RememberPositions(){
		if(isGUI){
			showingPos = GUIRectTransform.localEulerAngles;
			hiddenPos = GUIRectTransform.localEulerAngles + new Vector3(hideDeltaX, hideDeltaY, hideDeltaZ);
		}
		else{
			showingPos = gameObject.transform.localEulerAngles;
			hiddenPos = gameObject.transform.localEulerAngles + new Vector3(hideDeltaX, hideDeltaY, hideDeltaZ);
		}
	}
	
	public override void Reset(){
		if (startsHidden){
			if(isGUI){
				GUIRectTransform.localEulerAngles = hiddenPos;
			}
			else{
				gameObject.transform.localEulerAngles = hiddenPos;
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
				if(showingPos.x != 0f || showingPos.z != 0f){
					Debug.LogWarning("GUI rotation will only rotate z-axis");
				}
				tweenID = LeanTween.rotateLocal(gameObject, new Vector3(0, 0, showingPos.z), time)	// Special case for GUI rotation
					.setEase(easeShow)
					.setDelay(showDelay)
					.setUseEstimatedTime(isUseEstimatedTime)
					.setOnComplete(ShowSendCallback)
					.id;
			}
			else{
				tweenID = LeanTween.rotateLocal(gameObject, showingPos, time)
					.setEase(easeShow)
					.setDelay(showDelay)
					.setUseEstimatedTime(isUseEstimatedTime)
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
				if(showingPos.x != 0f || showingPos.z != 0f){
					Debug.LogWarning("GUI rotation will only rotate z-axis");
				}
				tweenID = LeanTween.rotateLocal(gameObject, new Vector3(0, 0, hiddenPos.z), time)
					.setEase(easeHide)
					.setDelay(hideDelay)
					.setUseEstimatedTime(isUseEstimatedTime)
					.setOnComplete(ShowSendCallback)
					.id;
			}
			else{
				tweenID = LeanTween.rotateLocal(gameObject, hiddenPos, time)
					.setEase(easeHide)
					.setDelay(hideDelay)
					.setUseEstimatedTime(isUseEstimatedTime)
					.setOnComplete(HideSendCallback)
					.id;
			}
		}
	}
}
