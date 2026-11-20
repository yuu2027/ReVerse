using UnityEngine;
using TMPro;

namespace ReVerse.Core
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timeText;

        private float _startTime;
        private float _elapsedTime;

        public float CurrentTime { get; private set; }

        private void Start()
        {
            _startTime = Time.time;
        }

        private void Update()
        {
            _elapsedTime = Time.time - _startTime;
            CurrentTime = _elapsedTime;

            _timeText.text = $"{CurrentTime:F1}";
        }
    }
}

