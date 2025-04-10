using CustomAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UIControllers
{
    public class MainMenuController : MonoBehaviour
    {
        private UIDocument _uiDocument;
        [SerializeField, NotNull] private string _level1SceneName;
        void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
        
            _uiDocument.rootVisualElement.Q<Button>("PlayGameButton").RegisterCallback<ClickEvent>(evt =>
            {
                SceneManager.LoadScene(_level1SceneName);
            });
            
            _uiDocument.rootVisualElement.Q<Button>("CreditsButton").RegisterCallback<ClickEvent>(evt =>
            {
                _uiDocument.rootVisualElement.Q<VisualElement>("Credits").style.display = DisplayStyle.Flex;
            });
        
            _uiDocument.rootVisualElement.Q<Button>("QuitGameButton").RegisterCallback<ClickEvent>(evt =>
            {
                Application.Quit();
            });
            
            _uiDocument.rootVisualElement.Q<Button>("CloseCreditsButton").RegisterCallback<ClickEvent>(evt =>
            {
                _uiDocument.rootVisualElement.Q<VisualElement>("Credits").style.display = DisplayStyle.None;
            });
        }
    
    

    }
}
