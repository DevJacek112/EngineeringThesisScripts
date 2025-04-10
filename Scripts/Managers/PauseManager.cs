using System;
using UnityEngine;
using UnityEngine.InputSystem;
using CustomAttributes;
using UIControllers;

namespace Managers
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance { get; private set; }
        [SerializeField, NotNull] private PauseMenuController _pauseMenuController;
        
        public event EventHandler EventUnpause;
    
        private static bool _paused = false;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        
        void Start()
        {
            InputManager.Instance._pause.performed += OnPauseKeyPressed;
        }

        private void OnPauseKeyPressed(InputAction.CallbackContext context)
        {
            if (!_paused)
            {
                Pause();  
                _pauseMenuController.ShowPauseMenu();
            }
            else if (_paused)
            {
                Unpause();
                _pauseMenuController.HidePauseMenu();
            }
        }

        public void Pause()
        {
            _paused = true;
            Time.timeScale = 0f;
            InputManager.Instance.EnableCursor();
            InputManager.Instance.DisableMovement();
        }

        public void Unpause()
        {
            _paused = false;
            Time.timeScale = 1f;
            InputManager.Instance.DisableCursor();
            InputManager.Instance.EnableMovement();
            
            EventUnpause?.Invoke(this, EventArgs.Empty);
        }
    }
}
