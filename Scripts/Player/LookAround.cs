using System;
using CustomAttributes;
using Managers;
using UnityEngine;

namespace Player
{
    public class LookAround : MonoBehaviour
    {
        private float _mouseX;
        private float _mouseY;
        
        [SerializeField, NotNull] private Camera _playerCamera;
        [SerializeField, GreaterThanZero] private float _xClamp;

        private Transform _playerCameraTransform;
        private float _currentXRotation;

        private void Awake()
        {
            _playerCameraTransform = _playerCamera.GetComponent<Transform>();
        }

        void Update()
        {
            _mouseX = InputManager.Instance._lookAround.ReadValue<Vector2>().x * InputManager.Instance._playerSensitivityX;
            _mouseY  = InputManager.Instance._lookAround.ReadValue<Vector2>().y * InputManager.Instance._playerSensitivityY;
            
            //HORIZONTAL
            transform.Rotate(Vector3.up, _mouseX);

            //VERTICAL
            _currentXRotation -= _mouseY;
            _currentXRotation = Mathf.Clamp(_currentXRotation, -_xClamp, _xClamp);
            Vector3 targetRotation = transform.eulerAngles;
            targetRotation.x = _currentXRotation;
            _playerCameraTransform.eulerAngles = targetRotation;
        }
    }
}
