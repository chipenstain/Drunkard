using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Drunkard
{
    public class Deck : MonoBehaviour
    {
		[SerializeField] private bool startDeck;
		[SerializeField] private bool deadDeck;
		
        public int player = -1;
		
		private List<Card> cards = new List<Card>();
		public int CardsCount { get => cards.Count; }
		public Card First { get => cards[0]; }
		public Card Last { get => cards.Last(); }
		
		private Card lastCard = null;
		
		private void Start()
		{
			if (!startDeck && !deadDeck && player != -1) GameManager.Instance.playerDecks.Add(this);
			else if (deadDeck) GameManager.Instance.deads.Add(this);
		}
		
		public Card GetCard(bool up = true)
		{
			Card c = new Card();
			if (up) { c = cards[0]; }
			else { c = cards.Last(); }
			cards.Remove(c);
			return c;
		}
		
		public void GiveCard(Card card, bool up = true)
		{
			card.transform.parent = transform;
			StartCoroutine(card.Move(lastCard, new Vector3(0f, 0.015f * cards.Count, -(0.0001f * cards.Count))));
			lastCard = card;
			if (up) { cards.Add(card); }
			else { cards.Insert(0, card); }
		}
		
		public void Shuffle()
		{
			cards = cards.OrderBy(a => Random.value).ToList();
		}
		
		public void TriggerDeck(int id)
		{

			if (id == player && id == GameManager.Instance.activePlayer && cards.Count > 0 && (GameManager.Instance.state == GameManager.GameState.MovePlayer || GameManager.Instance.state == GameManager.GameState.Arg))
			{
				if (!deadDeck)
				{
					Card card = GetCard(true);
					card.ChangeSide();
					GameManager.Instance.fields[id].GiveCard(card, false);
					GameManager.Instance.Check();
				}
				else
				{
					Deck dead = new Deck();
					for (int i = 0; i < GameManager.Instance.deads.Count; i++)
					{
						if (GameManager.Instance.deads[i].player == id)
						{
							dead = GameManager.Instance.deads[i];
						}
					}
					while (dead.CardsCount > 0)
					{
						for (int i = 0; i < GameManager.Instance.playerDecks.Count; i++)
						{
							if (GameManager.Instance.playerDecks[i].player == id)
							{
								GameManager.Instance.playerDecks[i].GiveCard(dead.GetCard());
							}
						}
					}
				}
			}
			else if (id == GameManager.Instance.activePlayer && cards.Count > 0 && player == -1 && GameManager.Instance.state == GameManager.GameState.TakeCards)
			{
				foreach (Deck d in GameManager.Instance.fields)
				{
					while (d.CardsCount > 0)
					{
						for (int i = 0; i < GameManager.Instance.deads.Count; i++)
						{
							if (GameManager.Instance.deads[i].player == id)
							{
								Card card = d.GetCard();
								card.ChangeSide();
								GameManager.Instance.deads[i].GiveCard(card);
							}
						}
					}
				}
				GameManager.Instance.Check();
			}
		}
		
		private void OnMouseDown()
		{
			TriggerDeck(PlayerControl.Instance.playerId);
		}
    }
}
