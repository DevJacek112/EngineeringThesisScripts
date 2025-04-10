using CustomAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        //MOUSE SENSITIVITY
        [GreaterThanZero] public float _playerSensitivityX;
        [GreaterThanZero] public float _playerSensitivityY;

        //INPUTS
        private Controls _controls;

        //PLAYER
        [HideInInspector] public InputAction _move;
        [HideInInspector] public InputAction _jump;
        [HideInInspector] public InputAction _lookAround;
        [HideInInspector] public InputAction _interact;

        //UI
        [HideInInspector] public InputAction _pause;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;

            _controls = new Controls();

            DontDestroyOnLoad(gameObject);
            
            DisableCursor();
            EnableMovement();
            EnableUIControls();
        }

        public void EnableUIControls()
        {
            _pause = _controls.UI.Pause;
            _pause.Enable();
        }
        
        public void DisableUIControls()
        {
            _pause.Disable();
        }

        public void EnableMovement()
        {
            _move = _controls.Player.Move;
            _move.Enable();

            _jump = _controls.Player.Jump;
            _jump.Enable();
            
            _lookAround = _controls.Player.Look;
            _lookAround.Enable();

            _interact = _controls.Player.Interaction;
            _interact.Enable();
        }

        public void DisableMovement()
        {
            _move.Disable();
            _jump.Disable();
            _lookAround.Disable();
            _interact.Disable();
        }

        public void EnableCursor()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public void DisableCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
