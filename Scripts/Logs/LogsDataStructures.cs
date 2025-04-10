using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logs
{
    public class LogsDataStructures : MonoBehaviour
    {
        [Serializable]
        public class LogData
        {
            public string _objName;
            public string _objType;
            public Vector3 _objPosition;
            public int _usageNumber;
        }

        [Serializable]
        public class LogCollection
        {
            public string _version;
            public bool _finishedByCar;
            public int _score;
            public string _time;
            public List<LogData> _logs;
        }
        
        [Serializable]
        public class ObjectsConnection
        {
            public string _connectionName;
            
            public string _firstObjName;
            public Vector3 _firstObjPosition;
            
            public string _secondObjName;
            public Vector3 _secondObjPosition;

            public int _howManyTimes;
        }
    }
}
