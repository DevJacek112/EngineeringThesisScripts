using System;
using System.Collections.Generic;
using Objects;
using UnityEngine;
using UnityEngine.UIElements;
using CustomAttributes;

namespace UIControllers
{
    public class HUDController : MonoBehaviour
    {
        public static HUDController Instance { get; private set; }
        
        private UIDocument _uiDocument;
        public CustomTimer _shardMessageTimer;

        private float _firstShardMessageTime = 12;
        public float _shardMessageTime = 6;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
        
        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            RefreshScore(0);
            _shardMessageTimer = gameObject.AddComponent<CustomTimer>();
            _shardMessageTimer.InitializeTimer(_firstShardMessageTime);
            ActivateShardMessage("Gdzie... gdzie ja jestem? Czy to sen? Wszystko jest znajome, ale jakieś inne... " +
                                 "Powinienem się rozejrzeć po okolicy i poszukać wskazówek jeśli chcę zrozumieć " +
                                 "co tu się dzieje... O, tam się coś świeci!");
            _uiDocument.rootVisualElement.Q<VisualElement>("ShardMessagesBox").RemoveFromClassList("ShardMessagesBoxHidden");
        }

        private void Update()
        {
            if (!_shardMessageTimer._timerIsRunning && 
                !_uiDocument.rootVisualElement.Q<VisualElement>("ShardMessagesBox").ClassListContains("ShardMessagesBoxHidden"))
            {
                DisableShardMessage();
            }
        }

        public void RefreshScore(int score)
        {
            _uiDocument.rootVisualElement.Q<Label>("ScoreLabel").text = "Zebrane myśli: " + score.ToString();
        }

        public void ActivateHint(InteractiveObject.TypeOfInteractiveObject type)
        {
            if (type == InteractiveObject.TypeOfInteractiveObject.STORAGE)
            {
                _uiDocument.rootVisualElement.Q<VisualElement>("F").RemoveFromClassList("ButtonHidden");
                _uiDocument.rootVisualElement.Q<VisualElement>("F").style.display = DisplayStyle.Flex;
            }
            else if (type == InteractiveObject.TypeOfInteractiveObject.LADDER)
            {
                _uiDocument.rootVisualElement.Q<VisualElement>("WS").RemoveFromClassList("ButtonHidden");
                _uiDocument.rootVisualElement.Q<VisualElement>("WS").style.display = DisplayStyle.Flex;
            }
            else if(type == InteractiveObject.TypeOfInteractiveObject.STAIRS)
            {
                _uiDocument.rootVisualElement.Q<VisualElement>("WSAD").RemoveFromClassList("ButtonHidden");
                _uiDocument.rootVisualElement.Q<VisualElement>("WSAD").style.display = DisplayStyle.Flex;
            }
            else if (type == InteractiveObject.TypeOfInteractiveObject.DOOR)
            {
                _uiDocument.rootVisualElement.Q<VisualElement>("F").RemoveFromClassList("ButtonHidden");
                _uiDocument.rootVisualElement.Q<VisualElement>("F").style.display = DisplayStyle.Flex;
            }
        }

        public void DisableHint()
        {
            _uiDocument.rootVisualElement.Q<VisualElement>("F").style.display = DisplayStyle.None;
            _uiDocument.rootVisualElement.Q<VisualElement>("WSAD").style.display = DisplayStyle.None;
            _uiDocument.rootVisualElement.Q<VisualElement>("WS").style.display = DisplayStyle.None;
            
            _uiDocument.rootVisualElement.Q<VisualElement>("F").AddToClassList("ButtonHidden");
            _uiDocument.rootVisualElement.Q<VisualElement>("WSAD").AddToClassList("ButtonHidden");
            _uiDocument.rootVisualElement.Q<VisualElement>("WS").AddToClassList("ButtonHidden");
            
        }
        public void ActivateShardMessage(string message)
        {
            _uiDocument.rootVisualElement.Q<Label>("ShardMessagesLabel").text = message;
            _uiDocument.rootVisualElement.Q<VisualElement>("ShardMessagesBox").RemoveFromClassList("ShardMessagesBoxHidden");
            _shardMessageTimer.StartTimer();
        }

        public void DisableShardMessage()
        {
            _uiDocument.rootVisualElement.Q<VisualElement>("ShardMessagesBox").AddToClassList("ShardMessagesBoxHidden");
        }

        public void ShowShardMessage()
        {
            _uiDocument.rootVisualElement.Q<VisualElement>("ShardMessagesBox").RemoveFromClassList("ShardMessagesBoxHidden");
        }
        
        public void DisableScore()
        {
            _uiDocument.rootVisualElement.Q<Label>("ScoreLabel").AddToClassList("ScoreHidden");
        }

        public void ShowScore()
        {
            _uiDocument.rootVisualElement.Q<Label>("ScoreLabel").RemoveFromClassList("ScoreHidden");
        }
        
        public void DisableMiddle()
        {
            _uiDocument.rootVisualElement.Q<VisualElement>("Middle").AddToClassList("MiddleHidden");
        }

        public void ShowMiddle()
        {
            _uiDocument.rootVisualElement.Q<VisualElement>("Middle").RemoveFromClassList("MiddleHidden");
        }
    }
}
