using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameMenuControl : MonoBehaviour {

	[Header("Screens")]
	public GameObject LoginScreen;
	public GameObject MenuScreen;
	public GameObject FileScreen;	
	public GameObject UserAccountsScreen;
	public GameObject HistoryScreen;
	public GameObject RenderedHistoryScreen;
	public GameObject GameHistoryScreen;
	public GameObject ChangePasswordScreen;
	public GameObject CreateUserScreen;	
	public GameObject DeleteUserScreen;
	public GameObject ReleaseUserScreen;
	public GameObject ConfigurationScreen;

	[Header("User Accounts")]
	public Button CreateUserButton;
	public Button DeleteUserButton;
	public Button ReleaseBlockButton;

	[Header("Create User")]
	public InputField NewUsernameInput;

	[Header("Delete User")]
	public Dropdown DeleteDropdown;

	[Header("Release User")]
	public Dropdown ReleaseDropdown;

	[Header("History")]
	public Dropdown HistoryDropdown;
	public static User HistoryUser;

	[Header("Configuration")]
	public AudioClip[] BackgroundMusics;
	public AudioSource BackgroundMusic;
	public Dropdown AudioDropdown;
	public Slider AudioSlider;
	public Sprite[] BackgroundImages;
	public Dropdown BackgroundDropdown;
	public Image BackgroundImage;
	public Image BackgroundPreview;

	[Header("ListControl")]
	public GameObject ListItemPrefab;
	public GameObject ContentPanel;
	public Text name;
	public Text StatusText;

	[Header("GameListControl")]
	public GameObject GameListItemPrefab;
	public GameObject GameContentPanel;
	public Text game;

	

	public static GameObject CurrentScreen;
	public static string GameSelection = "";

	void Start(){
		MenuScreen.SetActive(false);
		FileScreen.SetActive(false);
		UserAccountsScreen.SetActive(false);
		ConfigurationScreen.SetActive(false);
		HistoryScreen.SetActive(false);
		ChangePasswordScreen.SetActive(false);	
		GameObject.FindWithTag("BackgroundMusic").GetComponent<AudioSource>().Stop();
	}

	void Update(){
		if (BackgroundMusic.clip != BackgroundMusics[AudioDropdown.value]){
			BackgroundMusic.clip = BackgroundMusics[AudioDropdown.value];
			BackgroundMusic.Play();
		}
		BackgroundMusic.volume = AudioSlider.value;
		BackgroundPreview.sprite = BackgroundImages[BackgroundDropdown.value];
	}

	/*
		UPDATE BACKGROUND
	*/
	public void UpdateBackgroundPicture(){
		BackgroundImage.sprite = BackgroundImages[BackgroundDropdown.value];		
	}

	/*
		CREATE USER
	*/
	public void CreateNewUser(){
		User newUser = new User(NewUsernameInput.text, NewUsernameInput.text);
		LoginControl.auth.users.Add(newUser);
		LoginControl.auth.userCount++;
		LoginControl.WriteData();
		GoToFile();
	}

	/*
		DELETE USER
	*/
	public void DeleteUser(){
		for (int i = 0; i < LoginControl.auth.userCount; i++){
			if (LoginControl.auth.users[i].username == DeleteDropdown.options[DeleteDropdown.value].text){
				// delete the user
				if (LoginControl.userIndex != i){
					LoginControl.auth.users.RemoveAt(i);
					if (LoginControl.userIndex > i)
						LoginControl.userIndex--;
					LoginControl.auth.userCount--;
					LoginControl.WriteData();
					GoToFile();
				}
				return;
			}
		}
	}

	/*
		RELEASE USER
	*/
	public void ReleaseUser(){
		for (int i = 0; i < LoginControl.auth.userCount; i++){
			if (LoginControl.auth.users[i].username == ReleaseDropdown.options[ReleaseDropdown.value].text){
				LoginControl.auth.users[i].status = "NORMAL";
				LoginControl.WriteData();
				GoToFile();
				return;
			}
		}
	}

	/*
		LOGOUT
	*/
	public void Logout(){
		LoginControl.auth.users[LoginControl.userIndex].history.durations.Add((Math.Truncate((System.DateTime.Now.Subtract(LoginControl.start).TotalMinutes)*100.0) / 100.0).ToString());
		LoginControl.WriteData();
		LoginControl.Reset();
		CurrentScreen.SetActive(false);
		LoginScreen.SetActive(true);
		CurrentScreen = LoginScreen;
	}

	/*
		GO TO SCREENS
	*/
	public void GoToFile(){
		CurrentScreen.SetActive(false);
		FileScreen.SetActive(true);
		CurrentScreen = FileScreen;
	}
	public void GoToGameMenu(){
		CurrentScreen.SetActive(false);
		MenuScreen.SetActive(true);
		CurrentScreen = MenuScreen;
	}
	public void GoToUserAccounts(){
		CurrentScreen.SetActive(false);

		// restrict access
		if (LoginControl.AuthenticatedUser.username != "admin"){
			CreateUserButton.interactable = false;
			DeleteUserButton.interactable = false;
			ReleaseBlockButton.interactable = false;
		}

		UserAccountsScreen.SetActive(true);
		CurrentScreen = UserAccountsScreen;
	}
	public void GoToConfiguration(){
		CurrentScreen.SetActive(false);
		ConfigurationScreen.SetActive(true);
		CurrentScreen = ConfigurationScreen;
	}
	public void GoToChangePassword(){
		CurrentScreen.SetActive(false);
		ChangePasswordScreen.SetActive(true);
		CurrentScreen = ChangePasswordScreen;
	}
	public void GoToCreateUser(){
		CurrentScreen.SetActive(false);
		CreateUserScreen.SetActive(true);
		CurrentScreen = CreateUserScreen;
	}
	public void GoToDeleteUser(){
		CurrentScreen.SetActive(false);

		// populate dropdown
		DeleteDropdown.ClearOptions();
		for (int i = 1; i < LoginControl.auth.userCount; i++){
			Dropdown.OptionData newData = new Dropdown.OptionData();
			newData.text = LoginControl.auth.users[i].username;
			DeleteDropdown.options.Add(newData);
		}

		DeleteUserScreen.SetActive(true);
		CurrentScreen = DeleteUserScreen;
	}
	public void GoToReleaseUser(){
		CurrentScreen.SetActive(false);

		// populate dropdown
		ReleaseDropdown.ClearOptions();
		for (int i = 0; i < LoginControl.auth.userCount; i++){
			if (LoginControl.auth.users[i].status == "BLOCKED"){
				Dropdown.OptionData newData = new Dropdown.OptionData();
				newData.text = LoginControl.auth.users[i].username;
				ReleaseDropdown.options.Add(newData);
			}
		}

		ReleaseUserScreen.SetActive(true);
		CurrentScreen = ReleaseUserScreen;
	}
	public void GoToHistory(){
		CurrentScreen.SetActive(false);

		// populate dropdown
		HistoryDropdown.ClearOptions();

		for (int i = 0; i < LoginControl.auth.userCount; i++){
			if (LoginControl.AuthenticatedUser.username == "admin" || i == LoginControl.userIndex){
				Dropdown.OptionData newData = new Dropdown.OptionData();
				newData.text = LoginControl.auth.users[i].username;
				HistoryDropdown.options.Add(newData);
			}
		}

		HistoryScreen.SetActive(true);
		CurrentScreen = HistoryScreen;
	}
	public void GoToRenderedHistory(){
		ListControl.Reset(ref ContentPanel);
		foreach(User user in LoginControl.auth.users){
			if (user.username == HistoryDropdown.options[HistoryDropdown.value].text){
				ListControl.Display(user, ref ListItemPrefab, ref ContentPanel, ref name, ref StatusText);
				CurrentScreen.SetActive(false);
				RenderedHistoryScreen.SetActive(true);
				CurrentScreen = RenderedHistoryScreen;
				return;
			}
		}
	}

	/*
		GO TO APPLE PICKER GAME
	*/
	public void GameHistoryApple(){
		GameSelection = "Apple Picker";
		GameListControl.Display(LoginControl.auth.users[LoginControl.userIndex], ref GameListItemPrefab, ref GameContentPanel, ref game);
		CurrentScreen.SetActive(false);
		GameHistoryScreen.SetActive(true);
		CurrentScreen = GameHistoryScreen;
	}
	public void PlayApplePicker(){
		SceneManager.LoadScene("ApplePicker_Scene");
	}
	/*
		GO TO MEMORY GAME
	*/
	public void GameHistoryMemory(){
		GameSelection = "Memory Game";
		GameListControl.Display(LoginControl.auth.users[LoginControl.userIndex], ref GameListItemPrefab, ref GameContentPanel, ref game);
		CurrentScreen.SetActive(false);
		GameHistoryScreen.SetActive(true);
		CurrentScreen = GameHistoryScreen;
		
	}
	public void PlayMemoryGame(){
		SceneManager.LoadScene("MemoryGame_TitleScene");
	}
	/*
		GO TO RPS GAME
	*/
	public void GameHistoryRPS(){
		GameSelection = "Rock Paper Scissors";
		GameListControl.Display(LoginControl.auth.users[LoginControl.userIndex], ref GameListItemPrefab, ref GameContentPanel, ref game);
		CurrentScreen.SetActive(false);
		GameHistoryScreen.SetActive(true);
		CurrentScreen = GameHistoryScreen;
		
	}
	public void PlayRPSGame(){
		SceneManager.LoadScene("RPS_PlayScene");
	}
	/*
		GO TO SHOOTER GAME
	*/
	public void GameHistoryShooter(){
		GameSelection = "Shooter Game";
		GameListControl.Display(LoginControl.auth.users[LoginControl.userIndex], ref GameListItemPrefab, ref GameContentPanel, ref game);
		CurrentScreen.SetActive(false);
		GameHistoryScreen.SetActive(true);
		CurrentScreen = GameHistoryScreen;
	}
	
	public void PlayGame(){
		switch(GameSelection){
			case "Apple Picker":
				SceneManager.LoadScene("ApplePicker_Scene");
				break;
			case "Memory Game":	
				SceneManager.LoadScene("MemoryGame_TitleScene");
				break;
			case "Rock Paper Scissors":
				SceneManager.LoadScene("RPS_PlayScene");
				break;
			case "Shooter Game":
				SceneManager.LoadScene("Shooter_Title_Scene");
				break;
			default:
				break;
		}
	}

}
