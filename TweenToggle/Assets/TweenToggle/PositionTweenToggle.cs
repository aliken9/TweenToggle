// TweenToggle version 0.1 - https://pixelmetry.com/projects/unity-tweentoggle-plugin/
// Copyright (C) 2016 Wenshiang Sean Chung - Pixelmetry
using UnityEngine;
using System.Collections;

/// <summary>
/// Position tween toggle.
/// Child of TweenToggle parent, Takes care of translation toggles
/// </summary>
public class PositionTweenToggle : TweenToggle {
	[Header("Tween Delta")]
	public float hideDeltaX;
	public float hideDeltaY;
	public float hideDeltaZ;

	protected Vector3 hiddenPosition;
	protected Vector3 showingPosition;

	protected override void RememberPositions(){
		if(isGUI){
			showingPosition = GUIRectTransform.anchoredPosition3D;
			hiddenPosition = GUIRectTransform.anchoredPosition3D + new Vector3(hideDeltaX, hideDeltaY, hideDeltaZ);
		}
		else{
			showingPosition = gameObject.transform.localPosition;
			hiddenPosition = gameObject.transform.localPosition + new Vector3(hideDeltaX, hideDeltaY, hideDeltaZ);
		}
	}
	
	public override void Reset(){
		if(startsHidden){
			if(isGUI){
				GUIRectTransform.anchoredPosition3D = hiddenPosition;
			}
			else{
				gameObject.transform.localPosition = hiddenPosition;
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
				tweenID = LeanTween.move(GUIRectTransform, showingPosition, time)
					.setEase(showEase)
					.setDelay(showDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(ShowSendCallback)
					.id;

			}
			else{
				tweenID = LeanTween.moveLocal(gameObject, showingPosition, time)
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
				tweenID = LeanTween.move(GUIRectTransform, hiddenPosition, time)
					.setEase(hideEase)
					.setDelay(hideDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(HideSendCallback)
					.id;
			}
			else{
				tweenID = LeanTween.moveLocal(gameObject, hiddenPosition, time)
					.setEase(hideEase)
					.setDelay(hideDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(HideSendCallback)
					.id;
			}
		}
	}
}
