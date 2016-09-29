﻿//// Copyright (c) Thalassian Studios
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Tween toggle.
/// Used to toogle move objects with LeanTween
/// Parent class not to be used, implemented by PositionTweenToggle, ScaleTweenToggle, and RotationTweenToggle
/// </summary>
public class TweenToggle : MonoBehaviour{

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
	[Tooltip("Doesn't do anything, check manually for organization")]
	public bool isUsingDemultiplexer = false;
	[Tooltip("Initial state on start, this is overwritten by demux if used by one")]
	public bool startsHidden = false;
	[Tooltip("Run independent of timescale")]
	public bool useEstimatedTime = false;

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

	protected bool isGUI;						// Check if Unity GUI, will be set on awake
	protected RectTransform GUIRectTransform;	// Local cache of rect transform if GUI

	protected int tweenID = -1;

	private bool isLastShowDemuxObject = false;		// If last object on a demux
	private bool isLastHideDemuxObject = false;		// If last object on a demux
	private TweenToggleDemux demuxScript;		// Aux reference to demux for finish callback

	protected void Awake(){
		GUIRectTransform = gameObject.GetComponent<RectTransform>();
		isGUI = GUIRectTransform != null ? true : false;
		RememberPositions();
	}
	
	protected void Start(){
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
	
	protected virtual void RememberPositions(){
		// Implement in child
	}

	public virtual void Reset(){
		// Implement in child
	}

	public void Show(){
		Show(showDuration);
	}
	
	public virtual void Show(float time){
		// Implement in child
	}
	
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
		Hide(hideDuration);
	}

	public virtual void Hide(float time){
		// Implement in child
	}

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
		onShowComplete.Invoke();
	}
}
