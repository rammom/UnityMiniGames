using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHistoryItem {
	public string Date, Score, Level;
	public GameHistoryItem(string date, string score, string level){
		this.Date = date;
		this.Score = score;
		this.Level = level;
	}
}

public class GameListControl : MonoBehaviour {

	public static List<GameHistoryItem> items;
	public static List<GameObject> prefabs;

	public static void Reset(ref GameObject ContentPanel){
		bool first = true;
		foreach(Transform child in ContentPanel.transform){
			if (!first)
				Destroy(child.gameObject);
			first = false;
		}
		items = new List<GameHistoryItem>();
	}

	void Start(){
		items = new List<GameHistoryItem>();
		prefabs = new List<GameObject>();
	}

	public static void Display(User user, ref GameObject ListItemPrefab, ref GameObject ContentPanel, ref Text name){
		items = new List<GameHistoryItem>();
		name.text = GameMenuControl.GameSelection + " History";
		// Get Data
		GameHistory history;
		if (GameMenuControl.GameSelection == "Apple Picker")
			history = user.history.AppleGame;
		else if (GameMenuControl.GameSelection == "Memory Game")
			history = user.history.MemoryGame;
		else if (GameMenuControl.GameSelection == "Rock Paper Scissors")
			history = user.history.RPSGame;
		else if (GameMenuControl.GameSelection == "Shooter Game")
			history = user.history.ShooterGame;
		else
			return;
		for(int i = 0; i < history.dates.Count; i++){
			if (GameMenuControl.GameSelection == "ShooterGame")
				items.Add(new GameHistoryItem(history.dates[i], history.scores[i], history.levels[i]));				
			else
				items.Add(new GameHistoryItem(history.dates[i], history.scores[i], ""));
		}
		// create prefab
		foreach (GameHistoryItem hist in items){
			GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
			ListItemController controller = newItem.GetComponent<ListItemController>();
			controller.Date.text = hist.Date;
			controller.Score.text = hist.Score;
			controller.Level.text = hist.Level;
			newItem.transform.SetParent(ContentPanel.transform);
			newItem.transform.localScale = Vector3.one;
		}
	}

}
