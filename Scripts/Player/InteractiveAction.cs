using System;
using CustomAttributes;
using Managers;
using Objects;
using UIControllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InteractiveAction : MonoBehaviour
    {
        //NEEDED COMPONENTS
        [SerializeField, NotNull] private CharacterController _characterController;
        [SerializeField, NotNull] private HUDController _hudController;
        [SerializeField, NotNull] private Camera _playerCamera;
        
        //VARIABLES
        [SerializeField, GreaterThanZero] private float _raycastDistance;
        
        [SerializeField] private Outline _outlineOfActiveInteractive;
        
        private void Start()
        {
            InputManager.Instance._interact.performed += OnInteract;
        }

        private void Update()
        {
            OutlineOfObject();
            HintOfObject();
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            GameObject reachedObject = IsAnyObjectReachable();
            if (reachedObject != null)
            {
                if (reachedObject.GetComponent<InteractiveObject>())
                {
                    reachedObject.GetComponent<InteractiveObject>().Interact();
                }
                else if (reachedObject.GetComponentInParent<InteractiveObject>())
                {
                    reachedObject.GetComponentInParent<InteractiveObject>().Interact();
                }
                else if (reachedObject.GetComponent<NoninteractiveObject>())
                {
                    reachedObject.GetComponent<NoninteractiveObject>().Interact();
                }
            }
        }

        private void HintOfObject()
        {
            
            GameObject reachedObject = IsAnyObjectReachable();

            if (reachedObject == null)
            {
                _hudController.DisableHint();
                return;
            }
            
            if (!reachedObject.TryGetComponent(out InteractiveObject obj))
            {
                Transform parentTransform = reachedObject.transform.parent;
                while (obj == null && parentTransform != null)
                {
                    parentTransform.TryGetComponent(out obj);
                    parentTransform = parentTransform.parent;
                }
            }

            if (obj == null)
            {
                _hudController.DisableHint();
                return;
            }

            if (!GamemodeManager.Instance.ShouldDisplayHint(obj._typeOfInteractiveObject))
            {
                _hudController.DisableHint();
                return;
            }
            
            _hudController.ActivateHint(obj._typeOfInteractiveObject);
        }


        private void OutlineOfObject()
        {
            GameObject reachedObject = IsAnyObjectReachable();
            Outline outline = null;

            if (reachedObject != null)
            {
                if (!reachedObject.TryGetComponent(out outline))
                {
                    Transform parentTransform = reachedObject.transform.parent;
                    while (outline == null && parentTransform != null)
                    {
                        parentTransform.TryGetComponent(out outline);
                        parentTransform = parentTransform.parent;
                    }
                }
            }

            if (outline == null)
            {
                if (_outlineOfActiveInteractive != null)
                {
                    _outlineOfActiveInteractive.transform.GetComponent<InteractiveObject>().DeactivateOutline();
                    _outlineOfActiveInteractive = null;
                }
            }
            else
            {
                if (_outlineOfActiveInteractive != outline)
                {
                    if (_outlineOfActiveInteractive != null)
                    {
                        _outlineOfActiveInteractive.transform.GetComponent<InteractiveObject>().DeactivateOutline();
                    }
                    
                    var interactiveObject = reachedObject.GetComponentInParent<InteractiveObject>();
                    if (interactiveObject == null || !GamemodeManager.Instance.ShouldDisplayOutline(interactiveObject._typeOfInteractiveObject)) 
                        return;

                    _outlineOfActiveInteractive = outline;
                    _outlineOfActiveInteractive.transform.GetComponent<InteractiveObject>().ActivateOutline();
                }
            }
        }

        
        private GameObject IsAnyObjectReachable()
        {
            Vector3 origin = _playerCamera.transform.position;
            Vector3 direction = _playerCamera.transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, _raycastDistance))
            {
                return hit.collider.gameObject;
            }

            return null;
        }
    }
}
