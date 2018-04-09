using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Remember, we need this line for uGUI to work.
public class HighScore : MonoBehaviour {
	static public int score = 1000; // a
	void Awake() { // a
		// If the PlayerPrefs HighScore already exists, read it
		if (PlayerPrefs.HasKey("HighScore")) { // b
			score = PlayerPrefs.GetInt("HighScore");
		}
		// Assign the high score to HighScore
		PlayerPrefs.SetInt("HighScore", score); // c
	}
	void Update () { // b
		Text gt = this.GetComponent<Text>();
		gt.text = "High Score: "+score;
		if (score > PlayerPrefs.GetInt("HighScore")) { // d
			PlayerPrefs.SetInt("HighScore", score);
		}
	}
}