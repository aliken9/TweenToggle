//// Copyright (c) Thalassian Studios
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Move tween toggle demultiplexer.
/// Packs multiple objects that needs to be move tweened into one call
/// </summary>
public class TweenToggleDemux : MonoBehaviour{
	[Tooltip("All individual TweenToggle scripts to be controlled by this demux")]
	public TweenToggle[] tweenToggleList;

	public Action onStart { get; set; }
	public Action onComplete { get; set; }


	[Tooltip("Should toggle be allowed while demux is already tweening?")]
	public bool allowReverseWhileTweening = true;	// Allow for reverse tween while tweening?
	public bool startsHidden = false;
	public GameObject lastFinishedShowObject;	// For lock
	public GameObject lastFinishedHideObject;	// For lock
	public bool hideImmediately = false;
	private bool isShown; // Active lock
	public bool IsShowing{ get { return isShown; } }

	private bool isMoving; // Move lock
	public bool IsMoving{ get { return isMoving; } }

	[Tooltip("[OPTIONAL] Assign a canvas group to block clicks for UI behind")]
	public CanvasGroup UIRayCastBlock;			// If this is assigned, it will attempt to block (ONLY) UI Raycasts when shown

	void Awake(){
		isMoving = false;
		isShown = !startsHidden;

		foreach(TweenToggle tween in tweenToggleList){
			tween.startsHidden = startsHidden;	// TweenToggle Start() will take care of setting position
		}

		if(UIRayCastBlock != null) {
			UIRayCastBlock.blocksRaycasts = !startsHidden;
		}
//		
//		if(lastFinishedShowObject != null){
//			lastFinishedShowObjectScript = lastFinishedShowObject.GetComponent<TweenToggle>();
//		}
//		
//		if(lastFinishedHideObject != null){
//			lastFinishedHideObjectScript = lastFinishedHideObject.GetComponent<TweenToggle>();
//		}

		//TODO Need to determine last script show the complete isShown call, and do callbacks
	}
	
//	public void LgReset(){
//		if(UIRayCastBlock != null) {
//			UIRayCastBlock.blocksRaycasts = startsHidden ? false : true;
//		}
//		foreach(GameObject go in GoList){
//			TweenToggle[] toggleList = go.GetComponents<TweenToggle>();
//			foreach(TweenToggle toggle in toggleList){
//				toggle.startsHidden = startsHidden;
//				toggle.Reset();
//			}
//		}
//	}
	
	public void Show(){
		if(!isShown && (allowReverseWhileTweening || !isMoving)){
			isShown = true;
			isMoving = true;

			if(UIRayCastBlock != null) {
				UIRayCastBlock.blocksRaycasts = true;
			}

			StartCoroutine(SetNextFrameShow());
		}
		else{
			//Debug.Log(isShown + " + " + isMoving);
			//Debug.Log("Demux in locked state already");
		}
	}
	
	public void Hide(){
		if(isShown && (allowReverseWhileTweening || !isMoving)){
			isShown = false;
			isMoving = true;

			if(UIRayCastBlock != null) {
				UIRayCastBlock.blocksRaycasts = false;
			}

			StartCoroutine(SetNextFrameHide());
		}
		else{
			//Debug.Log(isShown + " + " + isMoving);
			//Debug.Log("Demux in locked state already");
		}
	}
	
	private IEnumerator SetNextFrameShow(){
		yield return 0;
		foreach(TweenToggle tween in tweenToggleList){
			tween.Show();
		}
	}
	
	private IEnumerator SetNextFrameHide(){
		yield return 0;
		foreach(TweenToggle tween in tweenToggleList){
			if(hideImmediately){
				//Debug.Log(" -- - - HIDE BOOLEAN TRUE");
				// TODO Need to call last hide object last!!!!
				tween.hideDuration = 0f;
				tween.hideDelay = 0f;
			}
			tween.Hide();
		}
	}
}
