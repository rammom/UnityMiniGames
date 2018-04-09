using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAudioScript : MonoBehaviour {

	private static DestroyAudioScript instance = null;
	public static DestroyAudioScript Instance {
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
