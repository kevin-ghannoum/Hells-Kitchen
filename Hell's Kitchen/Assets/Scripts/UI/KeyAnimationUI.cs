using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class KeyAnimationUI : MonoBehaviour
    {
        [Header("Sprites")]
        [SerializeField]
        private Sprite[] frames;

        [Header("Animation Parameters")]
        [SerializeField]
        private bool loop = true;
        
        [SerializeField]
        private float duration = 1.0f;
        
        [Header("References")]
        [SerializeField]
        private Image image;

        private float _time;

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            int frame = Mathf.FloorToInt(Mathf.Clamp(_time * frames.Length / duration, 0.0f, frames.Length - 0.01f));
            image.sprite = frames[frame];
            if (_time >= duration)
            {
                _time = 0;
            }
        }
    }
}
