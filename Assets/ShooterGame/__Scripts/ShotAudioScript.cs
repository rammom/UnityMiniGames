using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotAudioScript : MonoBehaviour {

	private static ShotAudioScript instance = null;
	public static ShotAudioScript Instance {
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
