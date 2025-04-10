using System;
using Logs;
using Managers;
using UnityEngine;

namespace Objects
{
    public class NoninteractiveObject : MonoBehaviour
    {
        public enum TypeOfNoninteractiveObject
        {
            WAYPOINT,
            FAKEDOOR,
            FAKESTORAGE
        }

        public TypeOfNoninteractiveObject _typeOfNoninteractiveObject;
        
        public int _usageNumber = 0;

        private Vector3 _linePoint = Vector3.zero;
        
        private void Start()
        {
            var transformPosition = transform.Find("LinePoint")?.gameObject.transform.position;
            if (transformPosition != null)
            {
                _linePoint = (Vector3)transformPosition;
            }
            else
            {
                _linePoint = transform.position;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_typeOfNoninteractiveObject == TypeOfNoninteractiveObject.WAYPOINT)
            {
                _usageNumber++;
                LogsWriter.Instance?.AddToLog(gameObject.name, _typeOfNoninteractiveObject.ToString(), _linePoint, _usageNumber);
            }
        }

        public void Interact()
        {
            if (_typeOfNoninteractiveObject == TypeOfNoninteractiveObject.FAKEDOOR)
            {
                //print("Fake door");
                _usageNumber++;
                LogsWriter.Instance?.AddToLog(gameObject.name, _typeOfNoninteractiveObject.ToString(), _linePoint, _usageNumber);
            }
            else if (_typeOfNoninteractiveObject == TypeOfNoninteractiveObject.FAKESTORAGE)
            {
                //print("Fake storage");
                _usageNumber++;
                LogsWriter.Instance?.AddToLog(gameObject.name, _typeOfNoninteractiveObject.ToString(), _linePoint, _usageNumber);
            }
        }
    }
}
