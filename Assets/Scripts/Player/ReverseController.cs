using UnityEngine;
using ReVerse.Core;
using System;

namespace ReVerse.Player
{
    public class ReverseController : MonoBehaviour
    {
        [SerializeField] private float _reverseInterval = 4.0f;
        [SerializeField] private GameManager _gameManager;

        private float _previousTime;

        public bool IsReversed {  get; private set; }

        public event Action<bool> OnReverseChanged;

        private void Awake()
        {
            if ( _gameManager == null)
            {
                Debug.LogError("GameManagerが参照されていない");
                enabled = false;
            } 
        }

        private void Update()
        {
            if (_gameManager.CurrentTime -  _previousTime >= _reverseInterval)
            {
                IsReversed = !IsReversed;
                OnReverseChanged?.Invoke(IsReversed);
                _previousTime = _gameManager.CurrentTime;
            }
        }

    }
}
