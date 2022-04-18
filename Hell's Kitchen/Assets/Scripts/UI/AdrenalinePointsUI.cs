using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class AdrenalinePointsUI : WorldSpaceUI
    {
        [SerializeField]
        public float value = 200;
        
        [SerializeField]
        public string stringText = "";
        
        [SerializeField]
        private float offsetY = 50;
        
        [SerializeField]
        private Color color = Color.white;

        [SerializeField]
        private float animationTime = 0.5f;

        [SerializeField]
        private AnimationCurve animationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        private float _time;
        private TextMeshProUGUI _text;

        public override void Start()
        {
            base.Start();
            _text = canvas.GetComponentInChildren<TextMeshProUGUI>();
            _text.text = string.IsNullOrEmpty(stringText) ? $"{(value > 0 ? "+" : "")}{value}" : stringText;
            _text.color = color;
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            
            // Animation time
            _time += Time.deltaTime;
            var normalizedTime = _time / animationTime;
            
            // Set text and color
            _text.text = string.IsNullOrEmpty(stringText) ? $"{(value > 0 ? "+" : "")}{value}" : stringText;
            var textColor = new Color(color.r, color.g, color.b, 1.0f - normalizedTime);
            _text.color = textColor;
            
            // Update position
            screenSpaceObject.position += offsetY * animationCurve.Evaluate(normalizedTime) * Vector3.up;
            
            // Destroy after animation
            if (_time > animationTime)
            {
                Destroy(gameObject);
            }
        }

        public static void SpawnGoldNumbers(Vector3 worldPosition, float value)
        {
            SpawnAdrenalineNumbers(worldPosition, value, 70, Color.yellow);
        }

        public static void SpawnDamageNumbers(Vector3 worldPosition, float value)
        {
            SpawnAdrenalineNumbers(worldPosition, value, 70, new Color(0.9058824f, 0.2941177f, 0.2941177f));
        }

        public static void SpawnIngredientString(Vector3 worldPosition, string value)
        {
            SpawnAdrenalineText(worldPosition, value, 70, Color.grey);
        }

        private static void SpawnAdrenalineNumbers(Vector3 worldPosition, float value, float offsetY, Color color)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/AdrenalinePointsUI.prefab");
            var points = Instantiate(prefab, worldPosition + new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.5f, .5f)), Quaternion.identity);
            var adrenalinePointsUI = points.GetComponent<AdrenalinePointsUI>();
            if (adrenalinePointsUI)
            {
                adrenalinePointsUI.value = value;
                adrenalinePointsUI.stringText = "";
                adrenalinePointsUI.color = color;
                adrenalinePointsUI.offsetY = offsetY;
            }
        }
        
        private static void SpawnAdrenalineText(Vector3 worldPosition, string text, float offsetY, Color color)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/AdrenalinePointsUI.prefab");
            var points = Instantiate(prefab, worldPosition + new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.5f, .5f)), Quaternion.identity);
            var adrenalinePointsUI = points.GetComponent<AdrenalinePointsUI>();
            if (adrenalinePointsUI)
            {
                adrenalinePointsUI.value = 0;
                adrenalinePointsUI.stringText = text;
                adrenalinePointsUI.color = color;
                adrenalinePointsUI.offsetY = offsetY;
            }
        }
    }
}
