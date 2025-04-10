using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Managers;
using Systems;
using UnityEngine;

namespace Logs
{
    public class LogsWriter : MonoBehaviour
    {
        public static LogsWriter Instance { get; private set; }
        
        private List<LogsDataStructures.LogData> _logList = new List<LogsDataStructures.LogData>();
        
        private string _filePath = null;

        void Start()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            
            //Adding spawn log
            Vector3 spawnPosition = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, 0.912f, 
                GameObject.FindGameObjectWithTag("Player").transform.position.z);
            AddToLog("SPAWN", "SPAWN" , spawnPosition, 0);
        }
        
        private void OnApplicationQuit()
        {
            SaveLogToJson();
        }
        
        public void AddToLog(string objName, string objType, Vector3 objPosition, int usageNumber)
        {
            LogsDataStructures.LogData newLog = new LogsDataStructures.LogData
            {
                _objName = objName,
                _objType = objType,
                _objPosition = objPosition,
                _usageNumber = usageNumber
            };
            
            _logList.Add(newLog);
        }
        
        private void SaveLogToJson()
        {
            var buildPath = Directory.GetParent(Application.dataPath)?.FullName;
            var version = GamemodeManager.Instance._gamemode.ToString();
            var time = DateTime.Now;

            string fileNameStart;

            if (LeavingSystem.Instance._didFinishedGameByCar)
            {
                fileNameStart = "LogData_FINISHED_";
            }
            else
            {
                fileNameStart = "LogData_UNFINISHED_";
            }
            
            if (buildPath != null)
            {
                _filePath = Path.Combine(buildPath, fileNameStart + version + "_" + time.ToString("dd.MM.yyyy") + "_" + time.ToString("HH.mm.ss") + ".txt");
            }
            
            if(_filePath == null) return;
            
            string json = JsonUtility.ToJson(new LogsDataStructures.LogCollection { 
                    _version = version, 
                    _finishedByCar = LeavingSystem.Instance._didFinishedGameByCar,
                    _score = ScoreManager.Instance._score,
                    _time = time.ToString(new CultureInfo("pl-PL")),
                    _logs = _logList}, 
                true);

            try
            {
                File.WriteAllText(_filePath, json);
                Debug.Log($"Log saved to path: {_filePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error during saving to JSON: {e.Message}");
            }
        }
    }
}
