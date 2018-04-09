using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	// PUBLIC

	public GameObject TITLE;
	public GameObject GAME;

	public float speed;
	public Text InstructionText;
	public GameObject Rock;
	public GameObject Paper;
	public GameObject Scissors;
	public Sprite RockSprite;
	public Sprite PaperSprite;
	public Sprite ScissorsSprite;	
	public Image AIChoice;
	public Sprite defaultSprite;
	public int choiceMoveSpeed;
	public Text Wins;
	public Text Losses;
	public Text GamesPlayed;
	public Image EndGame;
	public Text WinLoseText;
	public Text WinLose2;


	// PRIVATE
	private int wins = 0;
	private int losses = 0;
	private int gamesPlayed = 0;
	private String choice = "";
	private String moveChoice = "";
	private String state = "free";
	String AI = "";	
	private float random;
	private int frame = 0;

	// Use this for initialization
	void Start () {

		// show the title screen first
		TITLE.SetActive(true);
		GAME.SetActive(false);

		// initialize variables
		EndGame.color = new Color32(0, 160, 255, 0);
		InstructionText.text = "Rock, Paper or Scissors?";
		WinLoseText.text = "";
		WinLose2.text = "";
		//transform.position = new Vector3(-2, 0, 0);
		Rock.transform.position = new Vector3(-5f, 3f, 0f);
		Paper.transform.position = new Vector3(-4.8f, 0f, 0f);
		Scissors.transform.position = new Vector3(-4.8f, -3.2f, 0f);
		AIChoice.sprite = defaultSprite;
		AIChoice.rectTransform.sizeDelta = new Vector2(200,200);
		AIChoice.color = new Color32(255, 255, 255, 255);
		state = "free";
		moveChoice = "";
		Wins.text = "Wins: "+wins.ToString();
		Losses.text = "Losses: "+losses.ToString();
		GamesPlayed.text = "Games Played: "+gamesPlayed.ToString();
	}

	// functions used to reposition gameobjects after movement
	void Reset() {
		EndGame.color = new Color32(0, 160, 255, 0);
		InstructionText.text = "Rock, Paper or Scissors?";				
		WinLoseText.text = "";
		WinLose2.text = "";		

		//transform.position = new Vector3(-2, 0, 0);
		Rock.transform.position = new Vector3(-5f, 3f, 0f);
		Paper.transform.position = new Vector3(-4.8f, 0f, 0f);
		Scissors.transform.position = new Vector3(-4.8f, -3.2f, 0f);
		AIChoice.sprite = defaultSprite;
		AIChoice.rectTransform.sizeDelta = new Vector2(200,200);		
		AIChoice.color = new Color32(255, 255, 255, 255);		
		state = "free";
		moveChoice = "";
		Wins.text = "Wins: "+wins.ToString();
		Losses.text = "Losses: "+losses.ToString();
		GamesPlayed.text = "Games Played: "+gamesPlayed.ToString();
		if (gamesPlayed == 10)
			state = "end";
	}
	
	// Update is called once per frame
	void Update () { 
		/** 10 games have been played */
		if (state == "end"){
			EndGame.color = new Color32(0, 160, 255, 255);
			if (wins > losses){
				WinLoseText.text = "YOU WIN!";
			}
			else if (losses > wins){
				WinLoseText.text = "YOU LOSE";
			}
			else{
				WinLoseText.text = "TIE GAME";
			}
			WinLose2.text = "Press Space to Exit";			
			if (Input.GetKeyDown("space")){
				LoginControl.auth.users[LoginControl.userIndex].history.RPSGame.dates.Add(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
				LoginControl.auth.users[LoginControl.userIndex].history.RPSGame.scores.Add(WinLoseText.text);
				LoginControl.WriteData();
				SceneManager.LoadScene("_Main_Scene");
			}
		}
		/** FREE STATE: walk around and pick any option */
		if (state == "free"){
			Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
			transform.position += move * speed * Time.deltaTime;

			InstructionText.alignment = TextAnchor.MiddleCenter;
			if (choice != "" && Input.GetKeyDown("space")){
				if (choice == "rock"){
					moveChoice = "ROCK";
				}
				else if (choice == "paper"){
					moveChoice = "PAPER";
				}
				else if (choice == "scissors"){
					moveChoice = "SCISSORS";
				}
				state = "chosen";
			}
		}
		/** CHOSEN STATE: we're now moving the chosen game object to the center of the screen */
		else if (state == "chosen"){
			if (moveChoice == "ROCK"){
				moveToMiddle(Rock);
			}
			else if (moveChoice == "PAPER"){
				moveToMiddle(Paper);
			}
			else if (moveChoice == "SCISSORS"){
				moveToMiddle(Scissors);
			}
		}
		/** WAIT STATE: the computer is generating a random choice */
		else if  (state == "wait"){
			random = UnityEngine.Random.value;
			/** ROCK */
			if (random < 0.333f){
				AIChoice.sprite = RockSprite;
				AI = "ROCK";
			}
			/** PAPER */
			else if (random > 0.666f){
				AIChoice.sprite = PaperSprite;
				AI = "PAPER";
			}
			/** SCISSORS */
			else if (random >= 0.333f && random <= 0.666f){
				AIChoice.sprite = ScissorsSprite;				
				AI = "SCISSORS";
			}
			AIChoice.rectTransform.sizeDelta = new Vector2(130,130);
			AIChoice.color = new Color32(255, 55, 55, 255);
			state = "restart";
		}
		/** RESTART STATE: calculate winner, give a time buffer to register win or loss, reset positions */
		else if (state == "restart"){
			print(moveChoice);
			if (moveChoice == "ROCK"){
				switch (AI){
					case "ROCK":
						InstructionText.text = "TIE GAME!";
						break;
					case "PAPER":
						InstructionText.text = "PAPER beats ROCK, you lose! :(";
						losses++;
						break;
					case "SCISSORS":
						InstructionText.text = "ROCK smashes SCISSORS, you win! :)";				
						wins++;
						break;
				}
			}
			else if (moveChoice == "PAPER"){
				switch (AI){
					case "ROCK":
						InstructionText.text = "PAPER beats ROCK, you win! :)";
						wins++;
						break;
					case "PAPER":
						InstructionText.text = "TIE GAME!";
						break;
					case "SCISSORS":
						InstructionText.text = "SCISSORS cuts PAPER, you lose! :(";
						losses++;
						break;
				}
			}
			else if (moveChoice == "SCISSORS"){
				switch (AI){
					case "ROCK":
						InstructionText.text = "ROCK smashes SCISSORS, you lose! :(";
						losses++;
						break;
					case "PAPER":
						InstructionText.text = "SCISSORS cuts PAPER, you win! :)";
						wins++;
						break;
					case "SCISSORS":
						InstructionText.text = "TIE GAME!";
						break;
				}
			}
			gamesPlayed++;
			InstructionText.alignment = TextAnchor.MiddleCenter;
			Wins.text = "Wins: "+wins.ToString();
			Losses.text = "Losses: "+losses.ToString();
			GamesPlayed.text = "Games Played: "+gamesPlayed.ToString();
			Invoke("Reset", 2);			
			state = "wait2";
		}
		frame++;
	}

	// check for trigger collisions
	void OnTriggerEnter2D(Collider2D other){

		/** MENU SCENE CHOICES */

		if (other.gameObject.CompareTag("PlayButton")){
			TITLE.SetActive(false);
			GAME.SetActive(true);
		}
		if (other.gameObject.CompareTag("QuitButton")){
			SceneManager.LoadScene("_Main_Scene");
		}

		/** GAMEPLAY CHOICES */

		if (other.gameObject.CompareTag("Rock")){
			InstructionText.text = "Press SPACE to choose Rock";
			choice = "rock";
		}
		else if (other.gameObject.CompareTag("Paper")){
			InstructionText.text = "Press SPACE to choose Paper";
			choice = "paper";
		}
		else if (other.gameObject.CompareTag("Scissor")){
			InstructionText.text = "Press SPACE to choose Scissors";
			choice = "scissors";
		}

	}

	void OnTriggerExit2D(Collider2D other){
		InstructionText.text = "Rock, Paper or Scissors?";
		choice = "";
	}

	// moves a game object to the middle of the scene
	void moveToMiddle(GameObject obj){
		obj.transform.position = Vector3.MoveTowards(obj.transform.position, new Vector3(0,0,0), choiceMoveSpeed*Time.deltaTime);
		if (obj.transform.position == new Vector3(0,0,0)){
			state = "wait";
		}
	}

}
