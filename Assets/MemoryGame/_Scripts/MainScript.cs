using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour {

	public void PlayGame(){
		SceneManager.LoadScene("MemoryGame_GameScene", LoadSceneMode.Additive);
	}

	public void Quit(){
		SceneManager.LoadScene("_Main_Scene");
	}

}
