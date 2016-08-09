//// Copyright (c) Thalassian Studios
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Move tween toggle demultiplexer.
/// Packs multiple objects that needs to be move tweened into one call
/// </summary>
public class TweenToggleDemux : MonoBehaviour{

	public GameObject[] GoList;

	public Action onStart { get; set; }
	public Action onComplete { get; set; }



	public bool allowReverseWhileTweening = true;	// Allow for reverse tween while tweening?
	public bool startsHidden = false;
	public GameObject lastFinishedShowObject;	// For lock
	public GameObject lastFinishedHideObject;	// For lock
	public bool hideImmediately = false;
	private bool isShown; // Active lock
	public bool IsShowing{ get { return isShown; } }

	private bool isMoving; // Move lock
	public bool IsMoving{ get { return isMoving; } }

	public CanvasGroup UIRayCastBlock;			// If this is assigned, it will attempt to block (ONLY) UI Raycasts when shown

	void Awake(){
		isMoving = false;
		isShown = startsHidden? false : true;

		foreach(GameObject go in GoList){
			foreach(TweenToggle toggle in go.GetComponents<TweenToggle>()){
				if(toggle != null){
					if(startsHidden){
						toggle.startsHidden = true;	// TweenToggle Start() will take care of setting position
					}
					else{
						toggle.startsHidden = false;
					}
				}
			}
		}

		if(UIRayCastBlock != null) {
			UIRayCastBlock.blocksRaycasts = startsHidden ? false : true;
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
	
	IEnumerator SetNextFrameShow(){
		yield return 0;
		foreach(GameObject go in GoList){
			foreach(TweenToggle toggle in go.GetComponents<TweenToggle>()){
				if(toggle != null){
					toggle.Show();
				}
			}
		}
	}
	
	IEnumerator SetNextFrameHide(){
		yield return 0;
		foreach(GameObject go in GoList){
			foreach(TweenToggle toggle in go.GetComponents<TweenToggle>()){
				if(toggle != null){
					if(hideImmediately){
						//Debug.Log(" -- - - HIDE BOOLEAN TRUE");
						// TODO Need to call last hide object last!!!!
						toggle.hideDuration = 0f;
						toggle.hideDelay = 0f;
					}
					toggle.Hide();
				}
			}
		}
	}
}
