using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drunkard
{
    public class AI : MonoBehaviour
    {
		public int playerId;
		private Deck deck;
		private Deck dead;
		
		private float timer = 0f;
		
		private IEnumerator Start()
		{
			yield return new WaitUntil(()=>{ return GameManager.Instance.playerDecks != null; });
			for (int i = 0; i < GameManager.Instance.playerDecks.Count; i++)
			{
				if (GameManager.Instance.playerDecks[i].player == playerId)
				{
					deck = GameManager.Instance.playerDecks[i];
				}
			}
			
			for (int i = 0; i < GameManager.Instance.deads.Count; i++)
			{
				if (GameManager.Instance.deads[i].player == playerId)
				{
					dead = GameManager.Instance.deads[i];
				}
			}
		}
		
        private void Update()
		{
			if (GameManager.Instance.activePlayer == playerId)
			{
				timer += Time.deltaTime;
				if (timer >= 0.7f)
				{					
					if (GameManager.Instance.state == GameManager.GameState.TakeCards)
					{
						GameManager.Instance.fields[0].TriggerDeck(playerId);
					}
					else
					{
						deck.TriggerDeck(playerId);
					}
					timer = 0f;
					
					if (deck.CardsCount == 0)
					{ 
						dead.TriggerDeck(playerId);
					}
				}
			}
		}
    }
}
