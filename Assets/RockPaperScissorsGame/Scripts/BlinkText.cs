using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour {

	// PUBLIC
	public Text blinkText;
	public int blinkIntervalOff;
	public int blinkIntervalOn;

	// PRIVATE
	private int frames;
	private String blinkTextString;
	private bool state;

	// Use this for initialization
	void Start () {
		frames = 0;
		blinkTextString = blinkText.text;
		state = false;
	}
	
	// Update is called once per frame
	void Update () {
		frames++;
		// if every blinkIntervalOn frame, hide text
		if (frames % blinkIntervalOn == 0 && state == false){
			blinkText.text = "";
			state = true;
		}
		// if every blinkIntervalOff frame, show text		
		else if (frames % blinkIntervalOff == 0 && state == true){
			blinkText.text = blinkTextString;
			state = false;
		}
	}

}
