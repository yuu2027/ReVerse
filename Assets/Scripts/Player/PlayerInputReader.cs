using UnityEngine;
using UnityEngine.InputSystem;

namespace ReVerse.Player
{
    /// <summary>
    ///  Input Systemからのキー入力を管理するクラス
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public sealed class PlayerInputReader : MonoBehaviour
    {
        private PlayerInput _input;

        private InputAction _moveAction;
        private InputAction _reverseAction; // 操作反転入力(テスト用)

        public Vector2 MoveAction => _moveAction.ReadValue<Vector2>();
        public bool ReverseAction => _reverseAction.IsPressed();

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();

            _moveAction = _input.actions.FindAction("Move");
            _reverseAction = _input.actions.FindAction("Reverse");
        }
    }
}

