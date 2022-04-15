using TMPro;
using UnityEngine;

namespace UI
{
    public class DamageNumbersUI : WorldSpaceUI
    {
        [SerializeField]
        public float damage = 200;
        
        [SerializeField]
        private float yOffset = 0.04f;

        [SerializeField]
        private float animationTime = 0.5f;

        [SerializeField]
        private AnimationCurve animationCurve;

        private float _time;
        private TextMeshProUGUI _text;

        public override void Start()
        {
            base.Start();
            _text = canvas.GetComponentInChildren<TextMeshProUGUI>();
            _text.text = $"{-damage}";
        }

        public override void Update()
        {
            base.Update();
            
            // Animation time
            _time += Time.deltaTime;
            var normalizedTime = _time / animationTime;
            
            // Set text and color
            _text.text = $"{-damage}";
            var color = _text.color;
            color.a = 1.0f - normalizedTime;
            _text.color = color;
            
            // Update position
            screenSpaceObject.position += yOffset * animationCurve.Evaluate(normalizedTime) * Vector3.up;
            
            // Destroy after animation
            if (_time > animationTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
