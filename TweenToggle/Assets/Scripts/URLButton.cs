using UnityEngine;
using System.Collections;

public class URLButton : MonoBehaviour {
	
	public void OnButtonClicked(){
		Application.OpenURL("http://pixelmetry.com");
	}
}
