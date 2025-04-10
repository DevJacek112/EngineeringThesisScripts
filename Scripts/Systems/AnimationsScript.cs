using System;
using DG.Tweening;
using Managers;
using UIControllers;
using UnityEngine;

public class AnimationsScript : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    bool _isInAnimation = true;

    private void Start()
    {
        PlayStartingAnimation();
        PauseManager.Instance.EventUnpause += BlockMovement;
    }

    private void PlayStartingAnimation()
    {
        _isInAnimation = true;
        var playerTransform = _player.GetComponent<Transform>();
        
        BlockMovement(null, null);
        playerTransform.DORotate(new Vector3(0, -130, 0), 3, RotateMode.Fast).OnComplete(() =>
            playerTransform.DORotate(new Vector3(0, -50, 0), 6, RotateMode.Fast).OnComplete(()=>
                playerTransform.DORotate(new Vector3(0, -106, 0), 3, RotateMode.Fast).OnComplete(() =>
                    {
                        InputManager.Instance.EnableMovement();
                        HUDController.Instance._shardMessageTimer.InitializeTimer(HUDController.Instance._shardMessageTime);
                        _isInAnimation = false;
                    }
                    )));
    }
    
    private void BlockMovement(object sender, EventArgs e)
    {
        if (_isInAnimation)
        {
            InputManager.Instance.DisableMovement();
        }
    }
}
