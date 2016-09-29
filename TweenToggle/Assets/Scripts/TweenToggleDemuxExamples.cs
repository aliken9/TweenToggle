using UnityEngine;
using System.Collections;

public class TweenToggleDemuxExamples : MonoBehaviour {
	public TweenToggleDemux demux1;
	public TweenToggleDemux demux2;
	public TweenToggleDemux demux3;
	public GameObject demux3CallbackText;
	public TweenToggleDemux demux4;

	public void OnDemux1Button(bool isOn){
		if(isOn){
			demux1.Show();
		}
		else{
			demux1.Hide();
		}
	}

	public void OnDemux2Button(bool isOn){
		if(isOn){
			demux2.Show();
		}
		else{
			demux2.Hide();
		}
	}

	public void OnDemux3Button(bool isOn){
		if(isOn){
			demux3.Show();
		}
		else{
			demux3.Hide();
		}
	}

	public void OnDemux3ShowComplete(){
		demux3CallbackText.SetActive(true);
	}

	public void OnDemux3HideComplete(){
		demux3CallbackText.SetActive(false);
	}

	public void OnDemux4Button(bool isOn){
		if(isOn){
			demux4.Show();
		}
		else{
			demux4.Hide();
		}
	}
}
