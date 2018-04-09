using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour {

	public void PlayGame() {
		SceneManager.LoadScene("Shooter_Main_Menu_Scene");
	}

	public void QuitGame() {
		SceneManager.LoadScene("_Main_Scene");
	}

}
