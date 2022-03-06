using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drunkard
{
    public class PlayerControl : MonoBehaviour
    {
		public static PlayerControl Instance;
		
		
		
        public int playerId;
		
		private void Awake()
		{
			Instance = this;
		}
    }
}
