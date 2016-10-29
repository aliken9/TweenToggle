// TweenToggle version 0.1 - https://pixelmetry.com/projects/unity-tweentoggle-plugin/
// Copyright (C) 2016 Wenshiang Sean Chung - Pixelmetry
using UnityEngine;
using System.Collections;

/// <summary>
/// Scale tween toggle.
/// Child of TweenToggle parent, Takes care of scale toggles
/// </summary>
public class ScaleTweenToggle : TweenToggle {
	[Header("Tween Delta")]
	public float hideDeltaX;
	public float hideDeltaY;
	public float hideDeltaZ;

	protected Vector3 hiddenScale;
	protected Vector3 showingScale;
	
	protected override void RememberPositions(){
		if(isGUI){
			showingScale = GUIRectTransform.localScale;
			hiddenScale = GUIRectTransform.localScale + new Vector3(hideDeltaX, hideDeltaY, hideDeltaZ);
		}
		else{
			showingScale = gameObject.transform.localScale;
			hiddenScale = gameObject.transform.localScale + new Vector3(hideDeltaX, hideDeltaY, hideDeltaZ);
		}
	}
	
	public override void Reset(){
		if (startsHidden){
			if(isGUI){
				GUIRectTransform.localScale = hiddenScale;
			}
			else{
				gameObject.transform.localScale = hiddenScale;
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
				tweenID = LeanTween.scale(GUIRectTransform, showingScale, time)
					.setEase(showEase)
					.setDelay(showDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(ShowSendCallback)
					.id;
			}
			else{
				tweenID = LeanTween.scale(gameObject, showingScale, time)
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
				tweenID = LeanTween.scale(GUIRectTransform, hiddenScale, time)
					.setEase(hideEase)
					.setDelay(hideDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(HideSendCallback)
					.id;
			}
			else{
				tweenID = LeanTween.scale(gameObject, hiddenScale, time)
					.setEase(hideEase)
					.setDelay(hideDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(HideSendCallback)
					.id;
			}
		}
	}
}
