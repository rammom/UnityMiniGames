using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoginControl : MonoBehaviour {

	[Header("LoginScreen")]
	public InputField UsernameInput;
	public InputField PasswordInput;
	public Button LoginButton, QuitButton;
	public GameObject ErrorText, DeactivateText;
	public GameObject LoginScreen;

	[Header("ChangePasswordScreen")]
	public InputField OldPasswordInput;
	public InputField NewPasswordInput;
	public GameObject IncorrectPasswordErrorText;
	public GameObject ChangePasswordScreen;

	[Header("Other")]
	public GameObject FileScreen;
	public GameObject MenuScreen;

	public static User AuthenticatedUser;
	public static int userIndex = -1;	
	public static bool Authenticated = false;
	public static Users auth;
	public static System.DateTime start;
	
	private static bool accessGranted = false;
	private static Dictionary<string, int> loginAttempts = new Dictionary<string, int>();

	public static void Reset(){
		AuthenticatedUser = null;
		userIndex = -1;	
		Authenticated = false;
		auth = null;
		accessGranted = false;
		loginAttempts = new Dictionary<string, int>();
	}

	void Start(){
		GameMenuControl.CurrentScreen = LoginScreen;
	}

	// On Quit
	public void Quit(){
		Debug.Log("Quitting..");
		Application.Quit();
	}

	private Users GetData(){
		string path = "Assets/Menu/Data/users.json";
		string jsonString = File.ReadAllText(path);
		return JsonUtility.FromJson<Users>(jsonString);
	}

	public static void WriteData(){
		string path = "Assets/Menu/Data/users.json";		
		File.WriteAllText(path, JsonUtility.ToJson(auth));
	}

	// On Login
	public void Login(){

		if (!loginAttempts.ContainsKey(UsernameInput.text)){
			loginAttempts.Add(UsernameInput.text, 1);
		}
		else{
			loginAttempts[UsernameInput.text]++;
		}

		// Read data from file into JSON
		auth = GetData();

		// Check if able to login
		if (loginAttempts[UsernameInput.text] > 3 && UsernameInput.text != "admin"){
			Debug.Log("Too many attempts");
			ErrorText.SetActive(false);
			for (int i = 0; i < auth.userCount; i++){
				if (auth.users[i].username == UsernameInput.text){
					auth.users[i].status = "BLOCKED";
					WriteData();
					break;
				}
			}
			DeactivateText.SetActive(true);
			return;
		}
		for (int i = 0; i < auth.userCount; i++){
			if (auth.users[i].username == UsernameInput.text){
				if (auth.users[i].status == "BLOCKED"){
					ErrorText.SetActive(false);					
					DeactivateText.SetActive(true);			
					return;
				}
			}
		}



		// Find users
		for (int i = 0; i < auth.userCount; i++){
			User user = auth.users[i];			
			if (UsernameInput.text == user.username && PasswordInput.text == user.password){
				userIndex = i;
				accessGranted = true;
				break;
			}
		}


		if (accessGranted){
			Debug.Log("Access Granted");
			start = System.DateTime.Now;
			Authenticated = true;
			AuthenticatedUser = auth.users[userIndex];

			LoginScreen.SetActive(false);
			if (auth.users[userIndex].status == "NEW"){
				ChangePasswordScreen.SetActive(true);
				auth.users[userIndex].status = "NORMAL";
			}
			else{
				MenuScreen.SetActive(true);
				GameMenuControl.CurrentScreen = MenuScreen;
			}

			auth.users[userIndex].history.logins.Add(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

			// Rewrite user with changes made
			WriteData();
		}
		else{
			Debug.Log("Invalid Credentials");
			ErrorText.SetActive(true);
			loginAttempts[UsernameInput.text]++;
		}

	}

	public void ChangePassword(){

		if (OldPasswordInput.text == auth.users[userIndex].password){
			auth.users[userIndex].password = NewPasswordInput.text;
			WriteData();
			BackFromChangePassword();
		}
		else{
			IncorrectPasswordErrorText.SetActive(true);
		}

	}

	public void BackFromChangePassword(){
		if (!Authenticated){
			Application.Quit();
		}
		else {
			ChangePasswordScreen.SetActive(false);
			MenuScreen.SetActive(true);
			GameMenuControl.CurrentScreen = MenuScreen;
		}
	}


}
