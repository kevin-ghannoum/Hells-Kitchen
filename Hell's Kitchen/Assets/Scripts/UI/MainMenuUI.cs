using Common.Enums;
using Input;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private GameObject canvas;
        [SerializeField] private Animator animator;

        private void Awake()
        {
            title.outlineWidth = .05f;
            title.outlineColor = Color.black;
            InputManager.Instance.Deactivate();
        }

        public void OnPlay()
        {
            canvas.SetActive(false); // disable menu UI canvas
            animator.Play("PlayerCamera"); // camera transition
            Invoke(nameof(LoadRestaurantScene), .5f); // scene change
            InputManager.Instance.Activate();
        }

        private void LoadRestaurantScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.Restaurant);
        }

        public void OnQuit()
        {
            Application.Quit();
        }
    }
}