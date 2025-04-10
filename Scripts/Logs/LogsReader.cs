using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Serialization;

namespace Logs
{
    public class LogsReader : MonoBehaviour
    {
        public List<LogsDataStructures.LogCollection> _allLogsCollections;
        public List<LogsDataStructures.ObjectsConnection> _connections;
        public List<GameObject> _lines;
        
        [SerializeField] private GameObject _tenements;
        [SerializeField] private GameObject _props;
        
        [SerializeField] private Material _semiTransparentMaterial;
        
        private void Start()
        {
            //Read all logs
            var buildPath = Directory.GetParent(Application.dataPath)?.FullName;
            ReadAllLogs(buildPath);
            
            //Detect specific connections in all logs
            DetectConnections(_allLogsCollections);

            //Detect maximum and minimum usages from all connections and initialize line factory
            Vector2 connectionMinMaxValues = new Vector2(99999, 0);
            foreach (var connection in _connections)
            {
                if (connection._howManyTimes > connectionMinMaxValues.y)
                {
                    connectionMinMaxValues.y = connection._howManyTimes;
                }

                if (connection._howManyTimes < connectionMinMaxValues.x)
                {
                    connectionMinMaxValues.x = connection._howManyTimes;
                }
            }

            if (connectionMinMaxValues.y > 10)
            {
                connectionMinMaxValues.y = 11;
            }
            
            if (connectionMinMaxValues.x < 3)
            {
                connectionMinMaxValues.x = 2;
            }

            LogsLineFactory.Instance.InitializeFactory(connectionMinMaxValues);
            
            //Create all lines
            foreach (var connection in _connections)
            {
                //if (connection._howManyTimes < 10 && connection._howManyTimes > 3)
                {
                    var line = LogsLineFactory.Instance.CreateLine(connection);
                    line.AddComponent<LineData>();
                    line.GetComponent<LineData>()._numberOfUsages = connection._howManyTimes;
                    _lines.Add(line);
                }
            }

            //Set objects in scene
            var meshRenderers = _tenements.transform.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = _semiTransparentMaterial;
            }
            
            meshRenderers = _props.transform.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = false;
            }
        }

        private void ReadAllLogs(string pathToLogsFolder)
        {
            _allLogsCollections = new List<LogsDataStructures.LogCollection>();
            
            if (!Directory.Exists(pathToLogsFolder))
            {
                Debug.LogError($"Folder not found at path: {pathToLogsFolder}");
                return;
            }
            
            string[] logFiles = Directory.GetFiles(pathToLogsFolder, "*.txt");

            foreach (string logFile in logFiles)
            {
                LogsDataStructures.LogCollection logCollection = LoadLogFromJson(logFile);
                if (logCollection != null)
                {
                    _allLogsCollections.Add(logCollection);
                }
            }

            //Debug.Log($"Loaded {_allLogsGroup.Count} log(s) from folder: {pathToLogsFolder}");
        }
        
        private LogsDataStructures.LogCollection LoadLogFromJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"File not found at path: {filePath}");
                return null;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                
                LogsDataStructures.LogCollection logCollection = JsonUtility.FromJson<LogsDataStructures.LogCollection>(json);

                //Debug.Log($"Log successfully loaded from path: {filePath}");
                return logCollection;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error during loading from JSON: {e.Message}");
                return null;
            }
        }

        private void DetectConnections(List<LogsDataStructures.LogCollection> allLogsCollections)
        {
            foreach (var collection in allLogsCollections)
            {
                var logsInCollection = collection._logs;

                for (int i = 0; i < logsInCollection.Count - 1; i++)
                {
                    //Prevent from making connection between same object
                    if (logsInCollection[i]._objName != logsInCollection[i + 1]._objName)
                    {
                        LogsDataStructures.ObjectsConnection actualObjectsConnection = new LogsDataStructures.ObjectsConnection();
                        actualObjectsConnection._firstObjName = logsInCollection[i]._objName;
                        actualObjectsConnection._firstObjPosition = logsInCollection[i]._objPosition;
                        
                        actualObjectsConnection._secondObjName = logsInCollection[i + 1]._objName;
                        actualObjectsConnection._secondObjPosition = logsInCollection[i + 1]._objPosition;

                        LogsDataStructures.ObjectsConnection existingConnection = GetExistingConnection(actualObjectsConnection);

                        if (existingConnection != null)
                        {
                            existingConnection._howManyTimes++;
                        }
                        else
                        {
                            actualObjectsConnection._howManyTimes = 1;
                            actualObjectsConnection._connectionName = actualObjectsConnection._firstObjName + "-" + actualObjectsConnection._secondObjName;
                            _connections.Add(actualObjectsConnection);
                        }

                        if (IsThereOppositeConnection(actualObjectsConnection))
                        {
                            actualObjectsConnection._firstObjPosition.x += 0.5f;
                            actualObjectsConnection._secondObjPosition.x += 0.5f;
                            
                            actualObjectsConnection._firstObjPosition.z += 0.5f;
                            actualObjectsConnection._secondObjPosition.z += 0.5f;
                        }
                    }
                }
            }
        }

        private bool IsThereOppositeConnection(LogsDataStructures.ObjectsConnection actualObjectsConnection)
        {
            foreach (var connection in _connections)
            {
                if (connection._firstObjName == actualObjectsConnection._secondObjName &&
                         connection._secondObjName == actualObjectsConnection._firstObjName)
                {
                    return true;
                }
            }

            return false;
        }

        private LogsDataStructures.ObjectsConnection GetExistingConnection(LogsDataStructures.ObjectsConnection actualObjectsConnection)
        {
            foreach (var connection in _connections)
            {
                if (connection._firstObjName == actualObjectsConnection._firstObjName &&
                    connection._secondObjName == actualObjectsConnection._secondObjName)
                {
                    return connection;
                }
            }
            
            return null;
        }
    }
}
