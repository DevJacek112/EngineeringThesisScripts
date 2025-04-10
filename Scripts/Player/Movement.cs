using System;
using CustomAttributes;
using DG.Tweening;
using Logs;
using Managers;
using Objects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        //NEEDED COMPONENTS
        [SerializeField, NotNull] private CharacterController _characterController;

        //VARIABLES
        private Vector3 _playerVelocity;
        public bool _isPlayerGrounded;
        [SerializeField, GreaterThanZero] private float _playerSpeed;
        [SerializeField, GreaterThanZero] private float _playerLadderSpeed;
        
        [SerializeField, GreaterThanZero] private float _jumpHeight;
        [SerializeField, GreaterThanZero] private float _ladderJumpStrength;
        [SerializeField] private float _gravityValue;

        private float _actualLadderTop = 999999;
        private float _actualLadderBottom = -999999;
        private Transform _feet;

        private bool _isPlayerOnStairs = false;
        
        private Sounds _playerSounds;

        private bool _lastFrameIsGrounded = true;

        private enum MovementState
        {
            NORMAL,
            LADDER
        }

        private MovementState _movementState = MovementState.NORMAL;

        private void Start()
        {
            _feet = transform.Find("Feet");
            _playerSounds = GetComponent<Sounds>();
        }

        private void Update()
        {
            CheckLadder(transform.position, 0.6f);
            CheckStairs();

            switch (_movementState)
            {
                case MovementState.NORMAL:
                    NormalMove();
                    break;
                case MovementState.LADDER:
                    LadderMove();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void NormalMove()
        {
            _isPlayerGrounded = _characterController.isGrounded;
            
            //Landing
            if (!_lastFrameIsGrounded && _isPlayerGrounded)
            {
                _playerSounds.CheckLandingSound();
            }
            
            //Moving
            var inputMoveDirection = InputManager.Instance._move.ReadValue<Vector2>();
            if (_isPlayerGrounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            var moveVector = new Vector3(inputMoveDirection.x, 0, inputMoveDirection.y);
            _characterController.Move(transform.TransformDirection(moveVector) * (Time.deltaTime * _playerSpeed));

            if (moveVector != Vector3.zero && _isPlayerGrounded)
            {
                _playerSounds.CheckStepSound();
            }

            //Jumping
            if (InputManager.Instance._jump.ReadValue<float>() > 0 && _isPlayerGrounded)
            {
                _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
            }
            
            _lastFrameIsGrounded = _isPlayerGrounded;

            _playerVelocity.y += _gravityValue * Time.deltaTime;
            _characterController.Move(_playerVelocity * Time.deltaTime);
        }

        private void LadderMove()
        {

            if (CheckIfPlayerIsAboveLadderTop())
            {
                ExitLadderMode();
            }
            
            //Moving on ladder
            var inputMoveDirection = InputManager.Instance._move.ReadValue<Vector2>();

            var moveVector = new Vector3(/*inputMoveDirection.x*/ 0, inputMoveDirection.y, 0);
            
            if (CheckIfPlayerIsBelowLadderBottom())
            {
                if (inputMoveDirection.y < 0)
                {
                    moveVector.z = inputMoveDirection.y;
                }
            }
            
            _characterController.Move(transform.TransformDirection(moveVector) * (Time.deltaTime * _playerLadderSpeed));

            if (moveVector != Vector3.zero)
            {
                _playerSounds.CheckLadderSound();
            }
            
            //Jumping from ladder
            /*if (InputManager.Instance._jump.ReadValue<float>() > 0)
            {
                moveVector = new Vector3(0, 0, -_ladderJumpStrength);
                ExitLadderMode();
                _characterController.Move(transform.TransformDirection(moveVector) * (Time.deltaTime * _ladderJumpStrength));
            }*/
        }

        private void CheckLadder(Vector3 center, float radius)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform.CompareTag("Ladder"))
                {
                    if (_movementState == MovementState.NORMAL && !CheckIfPlayerIsAboveLadderTop()) //We will know that this is first interaction with ladder
                    {
                        InteractiveObject script = hitCollider.transform.GetComponent<InteractiveObject>();
                        script._usageNumber++;
                        
                        Vector3? linePointPos = hitCollider.transform.Find("LinePoint")?.gameObject.transform.position;
                        if (linePointPos == null)
                        {
                            linePointPos = hitCollider.gameObject.transform.position;
                        }
                        
                        LogsWriter.Instance?.AddToLog(hitCollider.transform.name, script._typeOfInteractiveObject.ToString(), linePointPos.Value, script._usageNumber);
                        EnterLadderMode(hitCollider.transform);
                    }
                    return;
                }

                if (CheckIfPlayerIsBelowLadderBottom())
                {
                    ExitLadderMode();
                }
            }
        }

        private void CheckStairs()
        {
            RaycastHit hit;
            if (Physics.Raycast(_feet.position, Vector3.down, out hit, 1.5f) && hit.transform.CompareTag("Stairs"))
            {
                if (!_isPlayerOnStairs)
                {
                    InteractiveObject script = hit.transform.GetComponent<InteractiveObject>();
                    script._usageNumber++;
                    
                    Vector3? linePointPos = hit.transform.Find("LinePoint")?.gameObject.transform.position;
                    if (linePointPos == null)
                    {
                        linePointPos = hit.transform.position;
                    }
                    
                    LogsWriter.Instance?.AddToLog(hit.transform.name, script._typeOfInteractiveObject.ToString(), linePointPos.Value, script._usageNumber);
                    _isPlayerOnStairs = true;
                }
            }
            else
            {
                if (_isPlayerOnStairs)
                {
                    _isPlayerOnStairs = false;
                }
            }
        }

        private void EnterLadderMode(Transform ladder)
        {
            _movementState = MovementState.LADDER;
            _actualLadderTop = ladder.transform.Find("Top").transform.position.y;
            _actualLadderBottom = ladder.transform.Find("Bottom").transform.position.y;
        }

        private void ExitLadderMode()
        {
            _movementState = MovementState.NORMAL;
            _actualLadderTop = 999999;
            _actualLadderBottom = -999999;
        }

        private bool CheckIfPlayerIsAboveLadderTop()
        {
            if (_feet.position.y > _actualLadderTop)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckIfPlayerIsBelowLadderBottom()
        {
            if (_feet.position.y < _actualLadderBottom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
