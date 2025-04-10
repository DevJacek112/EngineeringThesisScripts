using System;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIControllers
{
    public class PauseMenuController : MonoBehaviour
    {
        private UIDocument _uiDocument;
        [SerializeField] private HUDController _hud;
        
        void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            //RESUME BUTTON
            _uiDocument.rootVisualElement.Q<Button>("ResumeGameButton").RegisterCallback<ClickEvent>(OnResumeClick);
        
            //QUIT BUTTON
            _uiDocument.rootVisualElement.Q<Button>("QuitGameButton").RegisterCallback<ClickEvent>(OnQuitClick);
        }

        private void OnEnable()
        {
            //RESUME BUTTON
            _uiDocument.rootVisualElement.Q<Button>("ResumeGameButton").RegisterCallback<ClickEvent>(OnResumeClick);
        
            //QUIT BUTTON
            _uiDocument.rootVisualElement.Q<Button>("QuitGameButton").RegisterCallback<ClickEvent>(OnQuitClick);
        }

        private void OnDisable()
        {
            
            //RESUME BUTTON
            _uiDocument.rootVisualElement.Q<Button>("ResumeGameButton").UnregisterCallback<ClickEvent>(OnResumeClick);
        
            //QUIT BUTTON
            _uiDocument.rootVisualElement.Q<Button>("QuitGameButton").UnregisterCallback<ClickEvent>(OnQuitClick);
        }

        public void ShowPauseMenu()
        {
            _uiDocument.rootVisualElement.Q<VisualElement>("PauseMenu").RemoveFromClassList("PauseMenuHidden");
            
            _hud.DisableShardMessage();
            _hud.DisableScore();
            _hud.DisableMiddle();
        }

        public void HidePauseMenu()
        {
            _uiDocument.rootVisualElement.Q<VisualElement>("PauseMenu").AddToClassList("PauseMenuHidden");
            
            _hud.ShowShardMessage();
            _hud.ShowScore();
            _hud.ShowMiddle();
        }

        private void OnResumeClick(ClickEvent evt)
        {            
            PauseManager.Instance.Unpause(); 
            HidePauseMenu();
        }

        private void OnQuitClick(ClickEvent evt)
        {
            Application.Quit();
        }
    }
}
