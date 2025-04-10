using UnityEngine;

namespace CustomAttributes
{
    public class CustomTimer : MonoBehaviour
    {
        public float _actualTime = 0f;
        public float _timeLenght;
        public bool _timerIsRunning = false;

        private void Update()
        {
            if (_timerIsRunning)
            {
                _actualTime += Time.deltaTime;
                if (_actualTime >= _timeLenght)
                {
                    _timerIsRunning = false;
                }
            }
        }
        
        public void InitializeTimer(float timeLenght)
        {
            _timeLenght = timeLenght;
        }

        public void StartTimer()
        {
            _actualTime = 0f;
            _timerIsRunning = true;
        }

        public void ResetTimer()
        {
            _actualTime = 0f;
        }

        public void StopTimer()
        {
            _timerIsRunning = false;
            ResetTimer();
        }
    }
}
