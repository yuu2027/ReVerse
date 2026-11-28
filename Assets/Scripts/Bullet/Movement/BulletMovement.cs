using UnityEngine;

namespace ReVerse.Bullet
{
    /// <summary>
    /// 弾が直進するときの移動用スクリプト
    /// </summary>
    public sealed class BulletMovement : MonoBehaviour
    {
        private Vector2 _direction;
        private float _speed;

        public void Initialize(Vector2 direction, float speed)
        {
            _direction = direction;
            _speed = speed;
        }

        private void Update()
        {
            if (_direction.magnitude < 0.001f || _speed <= 0) return;

            transform.position += (Vector3)(_direction * _speed * Time.deltaTime);
        }
    }
}
