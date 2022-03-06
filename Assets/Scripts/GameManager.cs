using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drunkard
{
    public class GameManager : MonoBehaviour
    {		
		public enum GameState { MovePlayer, Arg, TakeCards }
	
		public static GameManager Instance;
		[SerializeField] private float offsetField;
	
		[SerializeField] private GameObject[] cards;
	
		[SerializeField] private Deck startDeck;
		
		public List<Deck> playerDecks = new List<Deck>();
		public List<Deck> fields = new List<Deck>();
		public List<Deck> deads = new List<Deck>();
		
		private int numbersOfPlayers = 2;		
		private int deckSize = 0;
		
		public int activePlayer = 0;
		private int hasMoved = 0;
		public GameState state = GameState.MovePlayer;
		
		private float timer = 10f;
		private bool inited2 = false;
		
		private void Awake()
		{
			Instance = this;
		}
		
		private IEnumerator Start()
		{
			yield return new WaitUntil(()=>{ return Instance != null; });
			
			for (int i = 0; i < numbersOfPlayers; i++)
			{
				GameObject go = new GameObject("field deck");
				fields.Add(go.AddComponent<Deck>());
				go.AddComponent<BoxCollider2D>().size = new Vector2(1.9f, 2.7f);
				
				go.transform.position = new Vector3((i + 1 % 2 == 1) ? offsetField * (i + 1) / 2 : -(offsetField * (i) / 2), 0f, 0f);
			}
		}
		
		private void Update()
		{
			timer += Time.deltaTime;
			if (timer >= 0.5f && inited2)
			{
				startDeck.Shuffle();
				int cur = 0;
				while (startDeck.CardsCount > 0)
				{
					playerDecks[cur].GiveCard(startDeck.GetCard());
					cur++;
					if (cur >= playerDecks.Count) { cur = 0; }
				}
				inited2 = false;
			}
		}
		
		public void InitGame()
		{
			inited2 = true;
			timer = 0f;
			
			foreach(GameObject cardObj in cards)
			{
				Card card = cardObj.GetComponent<Card>();
				
				if (deckSize == 0) //32
				{
					if (card.Strenght > 5)
					{
						Card cardInst = Instantiate(cardObj, startDeck.transform.position, startDeck.transform.rotation, startDeck.transform).GetComponent<Card>();
						startDeck.GiveCard(cardInst);
					}
				}
				else if (deckSize == 1) //52
				{
					if (card.Suit != Card.SuitCard.Joker)
					{
						Card cardInst = Instantiate(cardObj, startDeck.transform.position, startDeck.transform.rotation, startDeck.transform).GetComponent<Card>();
						startDeck.GiveCard(cardInst);
					}
				}
				else if (deckSize == 2) //54
				{
					Card cardInst = Instantiate(cardObj, startDeck.transform.position, startDeck.transform.rotation, startDeck.transform).GetComponent<Card>();
					startDeck.GiveCard(cardInst);
				}			
			}
		}
    
		public void Check()
		{
			if (hasMoved == numbersOfPlayers - 1)
			{
				hasMoved = 0;
				
				int id = -1;
				int max = -1;
				
				for (int i = 0; i < fields.Count; i++)
				{
					if (fields[i].First.Strenght == max)
					{
						state = GameState.Arg;
						activePlayer++;
						if (activePlayer >= numbersOfPlayers)
						{
							activePlayer = 0;
						}
						CheckWin();
						return;
					}
					else if (fields[i].First.Strenght == 6 && max == 14 && deckSize == 0)
					{
						max = fields[i].First.Strenght;
						id = i;
					}
					else if (fields[i].First.Strenght > max)
					{
						max = fields[i].First.Strenght;
						id = i;
					}
				}
				
				state = GameState.TakeCards;
				activePlayer = id;
				CheckWin();				
			}
			else
			{
				if (state == GameState.TakeCards)
				{
					state = GameState.MovePlayer;
				}
				else
				{
					hasMoved++;
					activePlayer++;
					if (activePlayer >= numbersOfPlayers)
					{
						activePlayer = 0;
					}
				}
			}
		}
	
		private void CheckWin()
		{
			for (int n = 0; n < numbersOfPlayers; n++)
			{
				Deck deck = new Deck();
				for (int i = 0; i < GameManager.Instance.playerDecks.Count; i++)
				{
					if (GameManager.Instance.playerDecks[i].player == n)
					{
						deck = GameManager.Instance.playerDecks[i];
						break;
					}
				}
				
				Deck dead = new Deck();
				for (int i = 0; i < GameManager.Instance.deads.Count; i++)
				{
					if (GameManager.Instance.deads[i].player == n)
					{
						dead = GameManager.Instance.deads[i];
						break;
					}
				}
				
				if (deck.CardsCount == 0 && dead.CardsCount == 0)
				{
					UIManager.Instance.Win( n + 1 );
					
					foreach (Deck d in playerDecks)
					{
						while (d.CardsCount > 0)
						{
							Destroy(d.GetCard().gameObject);	
						}
					}
					foreach (Deck d in fields)
					{
						while (d.CardsCount > 0)
						{
							Destroy(d.GetCard().gameObject);
						}
					}
					foreach (Deck d in deads)
					{
						while (d.CardsCount > 0)
						{
							Destroy(d.GetCard().gameObject);
						}
					}
					
					activePlayer = 0;
					hasMoved = 0;
					state = GameState.MovePlayer;
				}
			}
		}
	}
}
