// TweenToggle version 1.0 - http://www.pixelmetry.com/?portfolio=tween-toggle
// Copyright (C) 2017 Sean Chung - Pixelmetry
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Move tween toggle demultiplexer.
/// Packs multiple objects that needs to be move tweened into one call
/// </summary>
public class TweenToggleDemux : MonoBehaviour {
	[Header("General Settings")]
	[Tooltip("Should toggle be allowed while demux is already tweening?")]
	public bool allowToggleWhileTweening = true;
	[Tooltip("Initial state on start")]
	public bool startsHidden = false;
	[Tooltip("On hide call, hide all objects immediately instead of tweening")]
	public bool hideImmediately = false;
	[Tooltip("[OPTIONAL] Assign a canvas group to block clicks for UI behind")]
	public CanvasGroup UIRayCastBlock;              // If this is assigned, it will attempt to block (ONLY) UI Raycasts when shown

	[Header("Tween Complete Callback")]
	public UnityEvent onShowComplete;
	public UnityEvent onHideComplete;

	[Header("TweenToggle Components")]
	[Tooltip("All individual TweenToggle scripts to be controlled by this demux")]
	public TweenToggle[] tweenToggleList;

	private bool isShown; // Active lock
	public bool IsShowing { get { return isShown; } }

	private bool isMoving; // Move lock
	public bool IsMoving { get { return isMoving; } }

	void Awake() {
		isMoving = false;
		isShown = !startsHidden;

		// Track the last toggle to finish
		TweenToggle lastShowToggleSoFar = null;
		TweenToggle lastHideToggleSoFar = null;

		foreach(TweenToggle tween in tweenToggleList) {
			tween.startsHidden = startsHidden;          // TweenToggle Start() will take care of setting position

			// Find the TweenToggles that are the last to finish for show and hide
			if(lastShowToggleSoFar == null) {
				lastShowToggleSoFar = tween;
			}
			else {
				if(tween.showDelay + tween.showDuration > lastShowToggleSoFar.showDelay + lastShowToggleSoFar.showDuration) {
					lastShowToggleSoFar = tween;
				}
			}
			if(lastHideToggleSoFar == null) {
				lastHideToggleSoFar = tween;
			}
			else {
				if(tween.hideDelay + tween.hideDuration > lastHideToggleSoFar.hideDelay + lastHideToggleSoFar.hideDuration) {
					lastHideToggleSoFar = tween;
				}
			}
		}

		// Init the last TweenToggles to call this demux on show/hide complete
		lastShowToggleSoFar.SetLastDemuxObject(true, this);
		lastHideToggleSoFar.SetLastDemuxObject(false, this);

		if(UIRayCastBlock != null) {
			UIRayCastBlock.blocksRaycasts = !startsHidden;
		}
	}

	public void Show() {
		if(!isShown && (allowToggleWhileTweening || !isMoving)) {
			isShown = true;
			isMoving = true;

			if(UIRayCastBlock != null) {
				UIRayCastBlock.blocksRaycasts = true;
			}

			// Might be disabled from tween toggle, set self active
			if(!gameObject.activeSelf) {
				gameObject.SetActive(true);
			}

			StartCoroutine(SetNextFrameShow());
		}
		else {
			//Debug.Log("Demux in locked state already");
		}
	}

	public void Hide() {
		if(isShown && (allowToggleWhileTweening || !isMoving)) {
			isShown = false;
			isMoving = true;

			if(UIRayCastBlock != null) {
				UIRayCastBlock.blocksRaycasts = false;
			}

			StartCoroutine(SetNextFrameHide());
		}
		else {
			//Debug.Log("Demux in locked state already");
		}
	}

	private IEnumerator SetNextFrameShow() {
		yield return 0;
		foreach(TweenToggle tween in tweenToggleList) {
			tween.Show();
		}
	}

	private IEnumerator SetNextFrameHide() {
		yield return 0;
		foreach(TweenToggle tween in tweenToggleList) {
			tween.ToggleHideImmediately(hideImmediately);
			tween.Hide();
		}
	}

	public void ShowSendCallback() {
		isMoving = false;
		onShowComplete.Invoke();
	}

	public void HideSendCallback() {
		isMoving = false;
		onHideComplete.Invoke();
	}

	public void TestPrint1() {
		Debug.Log("11111");
	}

	public void TestPrint2() {
		Debug.Log("22");
	}
}
