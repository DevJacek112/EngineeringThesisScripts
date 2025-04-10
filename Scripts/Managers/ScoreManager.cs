using Objects;
using UIControllers;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using CustomAttributes;
using UnityEngine.Serialization;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }
        public int _score;
        [SerializeField, NotNull] private HUDController _HUDController;

        private void Start()
        {
            _score = 0;
            
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        public void Score()
        {
            _score++;
            _HUDController.RefreshScore(_score);
        }
    }
}
