using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Drunkard
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
		[SerializeField] private TextMeshProUGUI winner;
		
		[SerializeField] CanvasGroup cg;
		
		private void Awake()
		{
			Instance = this;
		}
		
		public void Win(int player)
		{
			winner.text = "Player " + player.ToString() + "win!";
			cg.alpha = 1f;
			cg.interactable = true;
		}
		
		public void PlayGame()
		{
			cg.alpha = 0f;
			cg.interactable = false;
			GameManager.Instance.InitGame();
		}
    }
}
