using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Logs;
using Managers;
using UnityEngine.Serialization;

namespace Objects
{
    public class InteractiveObject : MonoBehaviour
    {
        [SerializeField] private StructInteractiveObjectData  _data;
        [SerializeField] private GameObject _memoryShard;
        [SerializeField] private bool _isClosed = true;

        public int _usageNumber = 0;
        
        [SerializeField] private AudioClip _openingSound = null;
        [SerializeField] private AudioClip _closingSound = null;
        
        [SerializeField] private Mesh _interactiveMesh = null;
        
        private AudioSource _audioSource;

        private bool _isOutlineFixed = false;

        private Vector3 _linePoint = Vector3.zero;

        public enum TypeOfInteractiveObject
        {
            STORAGE,
            LADDER,
            STAIRS,
            DOOR
        }
        
        public TypeOfInteractiveObject _typeOfInteractiveObject;
        
        //Group settings
        [SerializeField] private List<InteractiveObject> _connectedInteractiveObjects;
        [SerializeField] private string _groupName = "DefaultGroupName";
        [SerializeField] private GameObject _groupLinePoint;
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            if (_interactiveMesh != null && GamemodeManager.Instance.ShouldYellowModel(_typeOfInteractiveObject))
            {
                GetComponent<MeshFilter>().mesh = _interactiveMesh;
            }

            //outline settings
            if (!GetComponent<Outline>())
            {
                var outline = gameObject.AddComponent<Outline>();

                if (_typeOfInteractiveObject == TypeOfInteractiveObject.LADDER || _typeOfInteractiveObject == TypeOfInteractiveObject.STAIRS)
                {
                    outline.OutlineMode = Outline.Mode.OutlineVisible;
                }
                else
                {
                    outline.OutlineMode = Outline.Mode.OutlineAll;
                }
                outline.OutlineColor = Color.white;
                outline.OutlineWidth = 5f;
                //outline.enabled = false;
            }

            //Setting up line point
            var transformPosition = transform.Find("LinePoint")?.gameObject.transform.position;
            if (transformPosition != null)
            {
                _linePoint = (Vector3)transformPosition;
            }
            else
            {
                _linePoint = transform.position;
            }
            
            if (_connectedInteractiveObjects != null && _connectedInteractiveObjects.Count > 0)
            {
                _linePoint = _groupLinePoint.transform.position;
            }
        }

        private void LateUpdate()
        {
            if (!_isOutlineFixed)
            {
                _isOutlineFixed = true;
                GetComponent<Outline>().enabled = false;
            }
        }

        public void Interact()
        {
            if(_typeOfInteractiveObject == TypeOfInteractiveObject.LADDER || _typeOfInteractiveObject == TypeOfInteractiveObject.STAIRS) return;
            Opening();
            _usageNumber++;
            
            if (_connectedInteractiveObjects != null && _connectedInteractiveObjects.Count > 0)
            {
                foreach (var obj in _connectedInteractiveObjects)
                {
                    obj.Opening();
                    obj._usageNumber++;
                    LogsWriter.Instance?.AddToLog(gameObject.transform.parent.name + "/" +  _groupName, _typeOfInteractiveObject.ToString(), _linePoint, _usageNumber);
                }
            }
            else
            {
                LogsWriter.Instance?.AddToLog(gameObject.transform.parent.name + "/" + gameObject.name, _typeOfInteractiveObject.ToString(), _linePoint, _usageNumber);
            }
        }

        private void Opening()
        {
            if (_isClosed)
            {
                _isClosed = false;
                transform.DOLocalMove(_data._openPosition, 0.3f);
                transform.DOLocalRotate(_data._openRotation, 0.3f);

                if (_memoryShard != null && !_memoryShard.activeInHierarchy)
                {
                    _memoryShard.SetActive(true);
                }

                if (_openingSound != null)
                {
                    if (_audioSource.enabled)
                    {
                        _audioSource.PlayOneShot(_openingSound);
                    }
                }
            }
            else
            {
                _isClosed = true;
                transform.DOLocalMove(_data._closePosition, 0.3f);
                transform.DOLocalRotate(_data._closeRotation, 0.3f);
                
                if (_openingSound != null)
                {
                    if (_audioSource.enabled)
                    {
                        _audioSource.PlayOneShot(_closingSound);
                    }
                }
            }
        }

        public void ActivateOutline()
        {
            GetComponent<Outline>().enabled = true;

            foreach (var obj in _connectedInteractiveObjects)
            {
                obj.TryGetComponent<Outline>(out Outline outline);
                outline.enabled = true;
            }
        }

        public void DeactivateOutline()
        {
            GetComponent<Outline>().enabled = false;

            foreach (var obj in _connectedInteractiveObjects)
            {
                obj.TryGetComponent<Outline>(out Outline outline);
                outline.enabled = false;
            }
        }
    }
}
