using DG.Tweening;
using Managers;
using UIControllers;
using UnityEngine;

namespace Objects
{
    public class MemoryShard : MonoBehaviour
    {
        private Camera _camera;

        [SerializeField] private string _shardText;
        
        private void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, _camera.transform.position) < 3)
            {
                transform.DOMove(_camera.transform.position, 1);
            }
        
            if (Vector3.Distance(transform.position, _camera.transform.position) < 0.2)
            {
                _camera.transform.GetComponentInParent<Sounds>().PlayShardPickupSound();
                ScoreManager.Instance.Score();
                transform.DOKill();
                Destroy(gameObject);
                HUDController.Instance.ActivateShardMessage(_shardText);
            }
        }
    }
}
