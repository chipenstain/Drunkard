using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drunkard
{
    public class CameraController : MonoBehaviour
    {
		[SerializeField] private SpriteRenderer field;
		
        private void Awake()
        {
			Camera.main.orthographicSize = field.bounds.size.x * Screen.height / Screen.width * 0.5f;
        }
    }
}
