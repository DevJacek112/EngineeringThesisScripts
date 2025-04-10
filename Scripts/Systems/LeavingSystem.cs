using System;
using Logs;
using Managers;
using UIControllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems
{
    public class LeavingSystem : MonoBehaviour
    {
        [SerializeField] LeavingMenuController _leavingMenuController;
        public static LeavingSystem Instance { get; private set; }
        
        public bool _didFinishedGameByCar = false;
        
        private void Start()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<CharacterController>(out CharacterController cc) && ScoreManager.Instance._score >= 2)
            {
                PauseManager.Instance.Pause();
                _leavingMenuController.ShowMenu();
                InputManager.Instance.DisableUIControls();
            }
        }

        public void QuitGameByButton()
        {
            LogsWriter.Instance?.AddToLog("EXIT", "EXIT", transform.Find("LinePoint").position, 0);
            _didFinishedGameByCar = true;
            Application.Quit();
        }
    }
}
