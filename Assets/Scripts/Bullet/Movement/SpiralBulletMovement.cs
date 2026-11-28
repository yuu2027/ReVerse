using UnityEngine;

namespace ReVerse.Bullet
{
    /// <summary>
    /// 中心の周りを回りながら広がっていく弾の移動
    /// </summary>
    public sealed class SpiralBulletMovement : MonoBehaviour
    {
        private Vector2 _centerPosition;
        private float _angle;          // 現在の角度（ラジアン）
        private float _radius;         // 中心からの距離
        private int _rotationDirection; // 1 = 反時計回り, -1 = 時計回り
        private float _rotationSpeed;  // 回る速さ（度/秒）
        private float _expansionSpeed; // 広がる速さ（単位/秒）

        public void Initialize(
            Vector2 center,
            float startAngleDeg,
            int rotationDirection,
            float rotationSpeed,
            float expansionSpeed)
        {
            _centerPosition = center;
            _angle = startAngleDeg * Mathf.Deg2Rad;
            _radius = 0f;
            _rotationDirection = rotationDirection >= 0 ? 1 : -1;
            _rotationSpeed = rotationSpeed;
            _expansionSpeed = expansionSpeed;

            transform.position = center;
        }

        private void Update()
        {
            // 角度と半径を「累積」する
            _angle += _rotationSpeed * Mathf.Deg2Rad * _rotationDirection * Time.deltaTime;
            _radius += _expansionSpeed * Time.deltaTime;

            Vector2 offset = new(Mathf.Cos(_angle), Mathf.Sin(_angle));
            transform.position = _centerPosition + offset * _radius;
        }
    }
}