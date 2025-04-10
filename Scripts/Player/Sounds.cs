using UnityEngine;
using CustomAttributes;

public class Sounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] _stepSoundsConcrete;
    [SerializeField] private AudioClip[] _stepSoundsMetal;
    [SerializeField] private AudioClip[] _ladderSounds;
    [SerializeField] private AudioClip _shardPickupSound;
    
    [SerializeField] private float _stepInterval = 0.5f;
    [SerializeField] private float _ladderInterval = 0.5f;
    [SerializeField] private float _landInterval = 0.3f;
    
    private CustomTimer _stepTimer;
    private CustomTimer _ladderTimer;
    private CustomTimer _landTimer;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _stepTimer = gameObject.AddComponent<CustomTimer>();
        _stepTimer.InitializeTimer(_stepInterval);        
        
        _ladderTimer = gameObject.AddComponent<CustomTimer>();
        _ladderTimer.InitializeTimer(_ladderInterval);
        
        _landTimer = gameObject.AddComponent<CustomTimer>();
        _landTimer.InitializeTimer(_landInterval);
        
        _audioSource = GetComponent<AudioSource>();
    }

    public void CheckStepSound()
    {
        if (!_stepTimer._timerIsRunning)
        {
            _stepTimer.StartTimer();
            PlayStepSound();
        }
    }

    public void CheckLandingSound()
    {
        if (!_landTimer._timerIsRunning)
        {
            _landTimer.StartTimer();
            PlayStepSound();
            _stepTimer.StartTimer();
        }
    }
    
    public void CheckLadderSound()
    {
        if (!_ladderTimer._timerIsRunning)
        {
            _ladderTimer.StartTimer();
            _audioSource.PlayOneShot(_ladderSounds[UnityEngine.Random.Range(0, _ladderSounds.Length)]);
        }
    }

    public void PlayShardPickupSound()
    {
        _audioSource.PlayOneShot(_shardPickupSound);
    }
    
    private void PlayStepSound()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f)) 
        {
                
            if (hit.transform.TryGetComponent<SurfaceType>(out SurfaceType surface))
            {
                if (surface._surface == SurfaceType.Surface.Metal)
                {
                    _audioSource.PlayOneShot(_stepSoundsMetal[UnityEngine.Random.Range(0, _stepSoundsMetal.Length)]);
                }
            }
            else
            {
                _audioSource.PlayOneShot(_stepSoundsConcrete[UnityEngine.Random.Range(0, _stepSoundsConcrete.Length)]);
            }
        }
    }
}
