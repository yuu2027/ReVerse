using UnityEngine;

namespace ReVerse.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerVitals : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null) return;

            Debug.Log($"衝突した：{collision.gameObject.name}");

            gameObject.SetActive(false);
        }
    }
}

