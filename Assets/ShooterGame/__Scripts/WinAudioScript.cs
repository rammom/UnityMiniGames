using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAudioScript : MonoBehaviour {

	private static WinAudioScript instance = null;
	public static WinAudioScript Instance {
		get { return instance; }
	}


	void Awake(){
		if (instance != null && instance != this){
			Destroy(this.gameObject);
			return;
		}
		else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
