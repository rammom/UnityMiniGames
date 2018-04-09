using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[System.Serializable]
public class GameHistory {
    public List<string> dates;
    public List<string> scores;
    public List<string> levels;
}

[System.Serializable]
public class History {
    public List<string> logins;
    public List<string> durations;
    public GameHistory MemoryGame;
    public GameHistory AppleGame;
    public GameHistory RPSGame;
    public GameHistory ShooterGame;
    public History(){
        this.logins =  new List<string>();
        this.durations = new List<string>();
    }
}

[System.Serializable]
public class Users{
    public List<User> users;
    public int userCount;
}

[System.Serializable]
public class User {
    public string username;
    public string password;
    public string status;
    public History history;

    public User(string name, string pass){
        this.username = name;
        this.password = pass;
        this.status = "NEW";
    }

}
