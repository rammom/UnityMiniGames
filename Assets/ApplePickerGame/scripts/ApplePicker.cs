using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ApplePicker : MonoBehaviour {
	[Header("Set in Inspector")] // a
	public GameObject basketPrefab;
	public int numBaskets = 3;
	public float basketBottomY = -14f;
	public float basketSpacingY = 2f;
	public List<GameObject> basketList;
	void Start () {
		for (int i=0; i<numBaskets; i++) {
			GameObject tBasketGO = Instantiate<GameObject>( basketPrefab );
			Vector3 pos = Vector3.zero;
			pos.y = basketBottomY + ( basketSpacingY * i );
			tBasketGO.transform.position = pos;
			basketList.Add (tBasketGO);
		}
	}
	public void AppleDestroyed() { // a
		// Destroy all of the falling apples
		GameObject[] tAppleArray=GameObject.FindGameObjectsWithTag("Apple"); // b
		foreach ( GameObject tGO in tAppleArray ) {
			Destroy( tGO );
		}
		int basketIndex = basketList.Count - 1;
		GameObject tBasketGO = basketList [basketIndex];
		basketList.RemoveAt( basketIndex );
		Destroy( tBasketGO );
		// If there are no Baskets left, restart the game
		if ( basketList.Count == 0 ) {
			// END GAME
			LoginControl.auth.users[LoginControl.userIndex].history.AppleGame.dates.Add(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
			LoginControl.auth.users[LoginControl.userIndex].history.AppleGame.scores.Add(HighScore.score.ToString());
			LoginControl.WriteData();
			SceneManager.LoadScene( "_Main_Scene" ); // a
		}
	}
}