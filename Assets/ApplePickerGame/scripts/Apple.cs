using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour {
	public static float bottomY = -2f; // a
	void Update () {
		if ( transform.position.y < bottomY ) {
			Destroy( this.gameObject ); // b

			// Get a reference to the ApplePicker component of Main Camera
			ApplePicker apScript = Camera.main.GetComponent<ApplePicker>(); // b
			// Call the public AppleDestroyed() method of apScript
			apScript.AppleDestroyed();

		}
	}
}