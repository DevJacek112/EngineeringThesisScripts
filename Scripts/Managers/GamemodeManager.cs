using System;
using System.Collections.Generic;
using System.IO;
using Objects;
using UnityEngine;

namespace Managers
{
    public class GamemodeManager : MonoBehaviour
    {
        public static GamemodeManager Instance { get; private set; }
    
        public enum Gamemode
        {
            FULL,
            MID,
            NONE
        }
    
        public Gamemode _gamemode;
    
        private Dictionary<InteractiveObject.TypeOfInteractiveObject, VisualProperitiesData> _interactiveObjectPropertiesDictionary = 
            new Dictionary<InteractiveObject.TypeOfInteractiveObject, VisualProperitiesData>();        

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        
            _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STORAGE] = new VisualProperitiesData();
            _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.LADDER] = new VisualProperitiesData();
            _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STAIRS] = new VisualProperitiesData();
            _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.DOOR] = new VisualProperitiesData();
        
            StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/gamemode.txt");
            var readGamemode = reader.ReadToEnd();
            reader.Close();
            
            if (Enum.TryParse(readGamemode, true, out Gamemode parsedGamemode))
            {
                _gamemode = parsedGamemode;
            }
            else
            {
                Debug.LogWarning($"Wrong gamemode in file - {readGamemode}, loaded value from editor.");
            }
            
            switch (_gamemode)
            {
                case Gamemode.FULL:
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STORAGE]._shouldOutline = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.LADDER]._shouldOutline = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STAIRS]._shouldOutline = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.DOOR]._shouldOutline = true;
                
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STORAGE]._shouldHint = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.LADDER]._shouldHint = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STAIRS]._shouldHint = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.DOOR]._shouldHint = true;
                
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STORAGE]._shoulYellowModel = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.LADDER]._shoulYellowModel = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STAIRS]._shoulYellowModel = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.DOOR]._shoulYellowModel = true;
                    break;
            
                case Gamemode.MID:
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STORAGE]._shouldOutline = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.LADDER]._shouldOutline = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STAIRS]._shouldOutline = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.DOOR]._shouldOutline = true;
                
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STORAGE]._shouldHint = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.LADDER]._shouldHint = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STAIRS]._shouldHint = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.DOOR]._shouldHint = true;
                
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STORAGE]._shoulYellowModel = true;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.LADDER]._shoulYellowModel = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STAIRS]._shoulYellowModel = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.DOOR]._shoulYellowModel = true;
                    break;
            
                case Gamemode.NONE:
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STORAGE]._shouldOutline = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.LADDER]._shouldOutline = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STAIRS]._shouldOutline = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.DOOR]._shouldOutline = false;
                
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STORAGE]._shouldHint = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.LADDER]._shouldHint = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STAIRS]._shouldHint = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.DOOR]._shouldHint = false;
                
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STORAGE]._shoulYellowModel = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.LADDER]._shoulYellowModel = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.STAIRS]._shoulYellowModel = false;
                    _interactiveObjectPropertiesDictionary[InteractiveObject.TypeOfInteractiveObject.DOOR]._shoulYellowModel = false;
                    break;
            }
        }
    
        public bool ShouldDisplayOutline(InteractiveObject.TypeOfInteractiveObject type)
        {
            if (_interactiveObjectPropertiesDictionary.TryGetValue(type, out VisualProperitiesData properties))
            {
                return properties._shouldOutline;
            }
            return false;
        }

        public bool ShouldDisplayHint(InteractiveObject.TypeOfInteractiveObject type)
        {
            if (_interactiveObjectPropertiesDictionary.TryGetValue(type, out VisualProperitiesData properties))
            {
                return properties._shouldHint;
            }
            return false;
        }
    
        public bool ShouldYellowModel(InteractiveObject.TypeOfInteractiveObject type)
        {
            if (_interactiveObjectPropertiesDictionary.TryGetValue(type, out VisualProperitiesData properties))
            {
                return properties._shoulYellowModel;
            }
            return false;
        }
    }
}
