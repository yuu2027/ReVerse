using UnityEngine;

namespace ReVerse.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _rb.gravityScale = 0f;     // 重力を０
            _rb.freezeRotation = true; // 回転無
        }
    }
}

