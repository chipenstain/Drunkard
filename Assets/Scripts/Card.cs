using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Drunkard
{
    public class Card : MonoBehaviour
    {
		public enum SuitCard { Hearts, Spades, Diamonds, Clubs, Joker }
		
		private SpriteRenderer render;
		
		private Sprite forward;
		[SerializeField] private Sprite back;
		
		[SerializeField] private SuitCard suit;
        [SerializeField] private int strenght;
		
		private bool isMoved;
		private bool isBack;
		
		public SuitCard Suit { get => suit; }
		public int Strenght { get => strenght; }
		
		public bool IsMoved { get => isMoved; }
		public bool IsBack { get => isBack; }
	
		private void Start()
		{
			render = GetComponent<SpriteRenderer>();
			forward = render.sprite;
			ChangeSide();
		}
		
		public IEnumerator Move(Card card, Vector3 coord)
		{
			isMoved = true;
			if (card != null) { yield return new WaitUntil(()=>{ return !card.IsMoved; }); }
			transform.DOLocalMove(coord, 0.3f);
			
			yield return new WaitUntil(()=>{ return transform.localPosition == coord; });
			isMoved = false;
		}
		
		public void ChangeSide()
		{
			if (isBack)
			{
				render.sprite = forward;
			}
			else
			{
				render.sprite = back;
			}
			isBack = !isBack;
		}
	}
}
