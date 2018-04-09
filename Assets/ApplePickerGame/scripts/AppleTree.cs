using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AppleTree : MonoBehaviour {
	[Header("Set in Inspector")]
	// Prefab for instantiating apples
	public GameObject applePrefab;
	// Speed at which the AppleTree moves
	public float speed = 20f;
	// Distance where AppleTree turns around
	public float leftAndRightEdge = 10f;
	// Chance that the AppleTree will change directions
	public float chanceToChangeDirections = 0.1f;
	// Rate at which Apples will be instantiated
	public float secondsBetweenAppleDrops = 1f;
	void Start () {
		// Dropping apples every second
		Invoke( "DropApple", 2f); // a
	}
	void DropApple() { // b
		GameObject apple = Instantiate<GameObject>( applePrefab ); // c
		apple.transform.position = transform.position; // d
		Invoke( "DropApple", secondsBetweenAppleDrops ); // e
	}
	void Update () {
		// Basic Movement
		Vector3 pos = transform.position; // b
		pos.x += speed * Time.deltaTime; // c
		transform.position = pos; // d
		 //Changing Direction
		if (pos.x < -leftAndRightEdge)
			speed = Mathf.Abs (speed);
		else if (pos.x > leftAndRightEdge)
			speed = -Mathf.Abs(speed);
	}
	void FixedUpdate() {
		// Changing Direction Randomly is now time-based because of FixedUpdate()
		if ( Random.value < chanceToChangeDirections ) { // b
			speed *= -1; // Change direction
		}
	}
}