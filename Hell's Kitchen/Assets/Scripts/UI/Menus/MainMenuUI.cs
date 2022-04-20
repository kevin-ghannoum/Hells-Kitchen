using Input;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MainMenuUI : MenuUI
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private GameObject canvas;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject connectingToServer;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject coopMenu;

        private void Awake()
        {
            title.outlineWidth = .05f;
            title.outlineColor = Color.black;
            animator.Play("MenuCamera");
            InputManager.Instance.Deactivate();
        }

        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                ShowConnectionMessage();
            }
            else
            {
                ShowMainMenu();
            }
        }

        public void OnPlay()
        {
            canvas.SetActive(false); // disable menu UI canvas
            animator.Play("PlayerCamera"); // camera transition
            Invoke(nameof(LoadRestaurantScene), .5f); // scene change
            InputManager.Instance.Activate();
        }

        public void CoopMode()
        {
            ShowCoopMenu();
        }

        public void Back()
        {
            ShowMainMenu();
        }

        public void ShowConnectionMessage()
        {
            connectingToServer.SetActive(true);
            mainMenu.SetActive(false);
            coopMenu.SetActive(false);
        }

        public void ShowMainMenu()
        {
            connectingToServer.SetActive(false);
            mainMenu.SetActive(true);
            coopMenu.SetActive(false);
        }

        public void ShowCoopMenu()
        {
            connectingToServer.SetActive(false);
            mainMenu.SetActive(false);
            coopMenu.SetActive(true);
        }
    }
}