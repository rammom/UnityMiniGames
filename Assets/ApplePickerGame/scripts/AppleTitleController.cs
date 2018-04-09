using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppleTitleController : MonoBehaviour {

	public GameObject TitleScreen;
	public GameObject GameScreen;

	public void Play(){
		TitleScreen.SetActive(false);
		GameScreen.SetActive(true);
	}

	public void Quit(){
		SceneManager.LoadScene("_Main_Scene");
	}

}
