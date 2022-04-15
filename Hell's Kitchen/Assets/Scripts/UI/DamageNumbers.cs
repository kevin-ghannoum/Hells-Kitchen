using System;
using Common.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DamageNumbers : MonoBehaviour
    {
        [SerializeField]
        public float damage = 200;
        
        [SerializeField]
        private float yOffset = 0.04f;

        [SerializeField]
        private float animationTime = 0.5f;

        [SerializeField]
        private AnimationCurve animationCurve;

        [SerializeField]
        private Canvas canvas;

        private float _time;
        private TextMeshProUGUI _text;
        private Camera _camera;

        private void Start()
        {
            _text = canvas.GetComponentInChildren<TextMeshProUGUI>();
            _camera = GameObject.FindWithTag(Tags.UICamera).GetComponent<Camera>();
            canvas.worldCamera = _camera;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            var normalizedTime = _time / animationTime;
            
            _text.text = $"-{damage}";
            var color = _text.color;
            color.a = 1.0f - normalizedTime;
            _text.color = color;
            
            var pos = transform.position;
            transform.position = pos + yOffset * animationCurve.Evaluate(normalizedTime) * Vector3.up;
            transform.rotation = Camera.main.transform.rotation;
            if (_time > animationTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
