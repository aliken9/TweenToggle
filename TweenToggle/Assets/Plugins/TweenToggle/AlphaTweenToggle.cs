//// Copyright (c) Thalassian Studios
using UnityEngine;
using UnityEngine.UI;

public class AlphaTweenToggle : TweenToggle {
	[Header("Tween Delta")]
	public float hideDeltaAlpha;

	private float showingAlpha;
	private float hiddenAlpha;

	protected override void RememberPositions(){
		if(isGUI){
			showingAlpha = GUIRectTransform.GetComponent<Image>().color.a;
			hiddenAlpha = showingAlpha + hideDeltaAlpha;
		}
		else{
			Debug.LogWarning("Tween alpha not implemented");
		}

		// Sanity check on hidden alpha values, sometimes we want to override simply
		if(hiddenAlpha < 0){
			hiddenAlpha = 0f;
		}
		else if(hiddenAlpha > 1){
			Debug.LogWarning("Hidden alpha is out of bounds: " + hiddenAlpha);
			hiddenAlpha = 1f;
		}
	}
	
	public override void Reset(){
		if(startsHidden){
			if(isGUI){
				Color colorAux = GUIRectTransform.GetComponent<Image>().color;
				GUIRectTransform.GetComponent<Image>().color = new Color(colorAux.r, colorAux.g, colorAux.b, hiddenAlpha);
			}
			else{
				Debug.LogWarning("Tween alpha not implemented");
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
				tweenID = LeanTween.alpha(GUIRectTransform, showingAlpha, time)
					.setEase(showEase)
					.setDelay(showDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(ShowSendCallback)
					.id;
			}
			else{
				Debug.LogWarning("Tween alpha not implemented");
			}
		}
	}
	
	public override void Hide(float time){
		if(isShown){
			isShown = false;
			isMoving = true;

			LeanTween.cancel(tweenID);
			
			if(isGUI){
				tweenID = LeanTween.alpha(GUIRectTransform, hiddenAlpha, time)
					.setEase(hideEase)
					.setDelay(hideDelay)
					.setUseEstimatedTime(useEstimatedTime)
					.setOnComplete(HideSendCallback)
					.id;
			}
			else{
				Debug.LogWarning("Tween alpha not implemented");
			}
		}
	}
}
