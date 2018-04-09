using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryItem {
	public string Date, Duration;
	public HistoryItem(string date, string duration){
		this.Date = date;
		this.Duration = duration;
	}
}

public class ListControl : MonoBehaviour {

	public static List<HistoryItem> items;
	public static List<GameObject> prefabs;

	public static void Reset(ref GameObject ContentPanel){
		bool first = true;
		foreach(Transform child in ContentPanel.transform){
			if (!first)
				Destroy(child.gameObject);
			first = false;
		}
		items = new List<HistoryItem>();
	}

	void Start(){
		items = new List<HistoryItem>();
		prefabs = new List<GameObject>();
	}

	public static void Display(User user, ref GameObject ListItemPrefab, ref GameObject ContentPanel, ref Text name, ref Text StatusText){
		items = new List<HistoryItem>();	
		name.text = user.username+"'s History";
		if (user.username == "admin")
			StatusText.text = user.status;
		// Get Data
		for(int i = 0; i < user.history.durations.Count; i++){
			items.Add(new HistoryItem(user.history.logins[i], user.history.durations[i]));
		}
		// create prefab
		foreach (HistoryItem hist in items){
			GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
			ListItemController controller = newItem.GetComponent<ListItemController>();
			controller.Date.text = hist.Date;
			controller.Duration.text = hist.Duration+" mins";
			newItem.transform.SetParent(ContentPanel.transform);			
			newItem.transform.localScale = Vector3.one;
		}
	}

}
