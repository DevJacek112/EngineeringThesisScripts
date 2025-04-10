using Managers;
using Systems;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIControllers
{
    public class LeavingMenuController : MonoBehaviour
    {
        private UIDocument _uiDocument; 
        public void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
        
            _uiDocument.rootVisualElement.Q<Button>("YesButton").RegisterCallback<ClickEvent>(evt =>
            {
                LeavingSystem.Instance.QuitGameByButton();
            });
        
            _uiDocument.rootVisualElement.Q<Button>("NoButton").RegisterCallback<ClickEvent>(evt =>
            {
                _uiDocument.rootVisualElement.Q<VisualElement>("LeavingMenu").AddToClassList("LeavingMenuHidden");
                PauseManager.Instance.Unpause();
                InputManager.Instance.EnableUIControls();
            });
        }

        public void ShowMenu()
        {
            _uiDocument.rootVisualElement.Q<VisualElement>("LeavingMenu").RemoveFromClassList("LeavingMenuHidden");
        }
    }
}
