using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // This line enables use of uGUI features. // a

public class Basket : MonoBehaviour {
	[Header("Set Dynamically")]
	public Text scoreGT;
	void Start() {
		// Find a reference to the ScoreCounter GameObject
		GameObject scoreGO = GameObject.Find("ScoreCounter"); // b
		// Get the Text Component of that GameObject
		scoreGT = scoreGO.GetComponent<Text>(); // c
		// Set the starting number of points to 0
		scoreGT.text = "0";
	}
	void Update () {
		// Get the current screen position of the mouse from Input
		Vector3 mousePos2D = Input.mousePosition; // a
		// The Camera's z position sets how far to push the mouse into 3D
		mousePos2D.z = -Camera.main.transform.position.z; // b
		// Convert the point from 2D screen space into 3D game world space
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint( mousePos2D ); // c
		// Move the x position of this Basket to the x position of the Mouse
		Vector3 pos = this.transform.position;
		pos.x = mousePos3D.x;
		this.transform.position = pos;
	}
	void OnCollisionEnter( Collision coll ) { // a
		// Find out what hit this basket
		GameObject collidedWith = coll.gameObject; // b
		if ( collidedWith.tag == "Apple" ) { // c
			Destroy( collidedWith );

			int score = int.Parse (scoreGT.text);
			score += 100;
			scoreGT.text = score.ToString ();
			// Track the high score
			if (score > HighScore.score) {
				HighScore.score = score;
			}
		}

	}

}