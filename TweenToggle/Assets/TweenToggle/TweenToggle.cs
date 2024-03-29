﻿// TweenToggle version 1.0 - http://www.pixelmetry.com/?portfolio=tween-toggle
// Copyright (C) 2017 Wenshiang Sean Chung - Pixelmetry
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Used to toogle move objects with LeanTween
/// Parent class not to be used, implemented by PositionTweenToggle, ScaleTweenToggle, and RotationTweenToggle
/// </summary>
public abstract class TweenToggle : MonoBehaviour{
	protected bool isMoving;
	public bool IsMoving{
		get{ return isMoving; }
	}

	protected bool isShown;
	public bool IsShown{
		get{ return isShown; }
	}

	//////////////////////////////////////////////////////
	[Header("General Settings")]
	[Tooltip("Initial state on start, this is overwritten by demux if used by one")]
	public bool startsHidden = false;
	[Tooltip("Run independent of timescale")]
	public bool useEstimatedTime = false;
	[Tooltip("Inactivate object when hide")]
	public bool inactiveWhenHidden = true;
	[Tooltip("On hide call, hide object immediately instead of tweening")]
	public bool hideImmediately = false;

	[Header("Timing and Easing")]
	public float showDelay = 0.0f;
	public float showDuration = 0.5f;
	public LeanTweenType showEase = LeanTweenType.easeInOutQuad;
	[Space(10)]
	public float hideDelay = 0.0f;
	public float hideDuration = 0.5f;
	public LeanTweenType hideEase = LeanTweenType.easeInOutQuad;
	
	//////////////////////////////////////////////////////
	
	[Header("Tween Complete Callback")]
	public UnityEvent onShowComplete;
	public UnityEvent onHideComplete;

	protected bool isGUI;							// Check if Unity GUI, will be set on awake
	protected RectTransform GUIRectTransform;		// Local cache of rect transform if GUI

	protected int tweenID = -1;

	private bool isLastShowDemuxObject = false;		// If last object on a demux
	private bool isLastHideDemuxObject = false;		// If last object on a demux
	private TweenToggleDemux demuxScript;           // Aux reference to demux for finish callback

	private bool auxHideImmediatelyFlag = false;	// Flags that detects HideImmediate change
	private float auxHideDelay;						// Backup for HideImmediately toggle
	private float auxHideDuration;                  // Backup for HideImmediately toggle

	protected void Awake(){
		GUIRectTransform = gameObject.GetComponent<RectTransform>();
		isGUI = GUIRectTransform != null ? true : false;
		RememberPositions();
		Reset();
	}

	// Set as last demux object
	public void SetLastDemuxObject(bool isShow, TweenToggleDemux parentDemux){
		if(isShow){
			isLastShowDemuxObject = true;
		}
		else{
			isLastHideDemuxObject = true;
		}
		demuxScript = parentDemux;
	}

	// Implement in child
	protected abstract void RememberPositions();

	// Implement in child
	public abstract void Reset();

	protected void ResetFinish() {
		StartCoroutine(ResetFinishHelper());
	}

	// Since setting an object inactive too soon affects setup of other TweenToggle components
	// in the same object, we wait until the end of frame to deactivate it
	private IEnumerator ResetFinishHelper() {
		yield return new WaitForEndOfFrame();
		if(inactiveWhenHidden) {
			gameObject.SetActive(!startsHidden);
		}
	}

	public void ToggleHideImmediately(bool isOn) {
		hideImmediately = isOn;
		if(isOn) {
			if(auxHideImmediatelyFlag != isOn) {
				auxHideImmediatelyFlag = isOn;
				auxHideDelay = hideDelay;
				auxHideDuration = hideDuration;
				hideDelay = 0f;
				hideDuration = 0f;
			}
		}
		else {
			if(auxHideImmediatelyFlag != isOn) {
				auxHideImmediatelyFlag = isOn;
				hideDelay = auxHideDelay;
				hideDuration = auxHideDuration;
			}
		}
	}

	public void Show(){
		if(inactiveWhenHidden) {
			gameObject.SetActive(true);
		}
		Show(showDuration);
	}

	// Implement in child
	public abstract void Show(float time);
	
	// Since Hide() only saves its show/hide position in the beginning,
	// 	if something has a dynamically changing position (inventory),
	//	this will update the new positions before tweening
	public void HideWithUpdatedPosition(){
		if(isShown){
			RememberPositions();
			Hide();
		}
	}
	
	public void Hide(){
		ToggleHideImmediately(hideImmediately);
		Hide(hideDuration);
	}

	// Implement in child
	public abstract void Hide(float time);

	///////////////////////// CALLBACKS ///////////////////////////////
	protected void ShowSendCallback(){
		tweenID = -1;
		isMoving = false;

		if(isLastShowDemuxObject && demuxScript){
			demuxScript.ShowSendCallback();
		}
		onShowComplete.Invoke();
	}

	protected void HideSendCallback(){
		tweenID = -1;
		isMoving = false;

		if(isLastHideDemuxObject && demuxScript){
			demuxScript.HideSendCallback();
		}
		onHideComplete.Invoke();

		// Toggle object off
		if(inactiveWhenHidden) {
			gameObject.SetActive(false);
		}
	}
}
