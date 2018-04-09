using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Card {
	public GameObject obj;
	public Sprite sprite;
	public int type;
	public bool flipped;
	public bool clickable;

	public Card(int t){
		this.type = t;
		this.flipped = false;
		this.clickable = true;
	}

}

public class StackController : MonoBehaviour {

	public int cardNum = 6;
	public GameObject V_card;
	public Sprite[] sprites = new Sprite[10];
	public Sprite defaultSprite;
	public Text ScoreText;
	public Dropdown DropdownValue;
	public AudioSource POP;
	public AudioSource WIN;
	public AudioSource LOSE;
	public Text EndText;

	private ArrayList cards = new ArrayList();
	private ArrayList flippedCards = new ArrayList();
	private bool canClick = true;
	private int score = 1000;
	private bool canDec = true;
	private int completedCards = 0;

	public void Restart() {
		DeleteCards();
		cardNum = DropdownValue.value+6;
		Debug.Log(cardNum);
		cards = new ArrayList();
		flippedCards = new ArrayList();
		canClick = true;
		score = 1000;
		ScoreText.text = "Score: "+score;		
		canDec = true;
		completedCards = 0;
		Start();
	}

	void DeleteCards(){
		int count = 0;
		for(int i = 0; i < 4; i++){
			for (int j = 0; j < 5; j++){
				if (count >= cardNum*2){
					break;
				}
				Card tmpCard = (Card)cards[(i*5)+j];
				Destroy(tmpCard.obj);
				count++;
			}
		}
	}

	public void Quit(){
			SceneManager.LoadScene("_Main_Scene");
	}

	// Use this for initialization
	void Start () {
		//GameObject go = Instantiate<GameObject>(V_card);
		//go.transform.position = new Vector3(100, 400, 0);
		CreateCards();
		SuffleCards();
		DisplayCards();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (flippedCards.Count == 2){
			canClick = false;
			if (!checkCards()){
				if (canDec){
					score -= 40;
					ScoreText.text = "Score: "+score;
					canDec = false;
					LOSE.Play();
				}
				StartCoroutine(hideCards());
			}
		}
		else if(flippedCards.Count == 0){
			canDec = true;
		}

		if (completedCards == cardNum){
			// win
			LoginControl.auth.users[LoginControl.userIndex].history.MemoryGame.dates.Add(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
			LoginControl.auth.users[LoginControl.userIndex].history.MemoryGame.scores.Add(((float)score).ToString());
			LoginControl.WriteData();
			SceneManager.LoadScene("_Main_Scene");
		}

		if (score == 0){
			// lose
			LoginControl.auth.users[LoginControl.userIndex].history.MemoryGame.dates.Add(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
			LoginControl.auth.users[LoginControl.userIndex].history.MemoryGame.scores.Add(((float)score).ToString());
			LoginControl.WriteData();
			SceneManager.LoadScene("_Main_Scene");
		}

	}

	bool checkCards(){
		Card card1 = (Card)flippedCards[0];
		Card card2 = (Card)flippedCards[1];
		if (card1.type == card2.type){
			card1.clickable = false;
			card2.clickable = false;
			flippedCards[0] = card1;
			flippedCards[1] = card2;
			canClick = true;
			if (flippedCards.Count != 0){
				flippedCards = new ArrayList();
				WIN.Play();
				completedCards++;
			}
			return true;
		}
		return false;
	}

	IEnumerator hideCards(){
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i < flippedCards.Count; i++){
			flipCard((Card)flippedCards[i]);
		}
		canClick = true;
	}

	void flipCard(Card c){
		if (!canClick || !c.clickable){
			return;
		}
		POP.Play();
		if (!c.flipped){
			c.obj.GetComponent<Image>().sprite = sprites[c.type];
			c.flipped = true;
			flippedCards.Add(c);
		}
		else{
			c.obj.GetComponent<Image>().sprite = defaultSprite;		
			c.flipped = false;		
			flippedCards.Remove(c);
		}
	}

	bool contains(ArrayList cards, Card c) {
		for (int i = 0; i < cards.Count; i++){
			if (c == cards[i]){
				return true;
			}
		}
		return false;
	}

	void CreateCards() {
		int cnt = 0;
		for (int i = 0; i < cardNum*2; i+= 2){
			for (int k = 0; k < 2; k++){
				Card c = new Card(cnt);
				c.obj = Instantiate(V_card, new Vector3(0,0,0),  Quaternion.identity) as GameObject;
				c.obj.transform.SetParent(GameObject.FindGameObjectWithTag("canvas").transform, false);
				Vector3 tmp = c.obj.transform.position;
				tmp.x += 10f;
				tmp.y -= 10f;
				c.obj.transform.position = tmp;

				c.sprite = sprites[cnt];

				Button btn = c.obj.GetComponent<Button>();
				btn.onClick.AddListener(delegate{flipCard(c);});

				cards.Add(c);
			}
			cnt++;
		}
	}

	void SuffleCards(){
		// shuffle here
		int random;
		for (int i = 0; i < cards.Count; i++){
			random = Random.Range(0, cards.Count-1);
			Swap(cards, i, random);
		}
	}

	void Swap(ArrayList cs, int a, int b){
		Card tmp = (Card)cs[a];
		cs[a] = cs[b];
		cs[b] = tmp;
	}

	void DisplayCards() {
		int count = 0;
		for(int i = 0; i < 4; i++){
			for (int j = 0; j < 5; j++){
				if (count >= cardNum*2){
					break;
				}
				Card tmpCard = (Card)cards[(i*5)+j];
				Vector3 tmp = tmpCard.obj.transform.position;
				tmp.x += (float)150+100*j;
				tmp.y -= (float)90+80*i;
				tmpCard.obj.transform.position = tmp;
				count++;
			}
		}
	}

}
