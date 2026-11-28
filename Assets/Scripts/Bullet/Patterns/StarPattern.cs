using System.Collections;
using UnityEngine;

namespace ReVerse.Bullet
{
    [CreateAssetMenu(fileName = "Star Pattern", menuName = "ReVerse/Patterns/Star")]
    public class StarPattern : BulletPatternBase
    {
        [Header("星の形")]
        [SerializeField, Min(3)] private int _points = 5;
        [SerializeField, Range(0.1f, 0.9f)] private float _innerRatio = 0.38f;
        [SerializeField, Min(1)] private int _bulletsPerEdge = 4;
        [SerializeField] private float _startAngle = 90f;

        [Header("動き")]
        [SerializeField, Min(0f)] private float _speed = 2f;
        [SerializeField] private float _rotationSpeed = 0f;
        [SerializeField] private int _rotationDirection = 1;

        [Header("弾")]
        [SerializeField] private BulletType _bulletType = BulletType.SpiralBlue;

        public override IEnumerator Fire(PatternContext ctx)
        {
            float speed = _speed * ctx.SpeedMultiplier;
            int perEdge = Mathf.Max(
                1, 
                Mathf.RoundToInt(_bulletsPerEdge * ctx.DensityMultiplier));
            int count = _points * 2;
            float step = 360f / count;

            Vector2[] corners = new Vector2[count]; // 頂点の個数

            for (int k = 0; k < count; k++)
            {
                float radius = (k % 2 == 0) ? 1f : _innerRatio;
                float angle = (_startAngle + step * k) * Mathf.Deg2Rad;

                corners[k] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            }

            for (int k = 0; k < count; k++)
            {
                Vector2 start = corners[k];
                Vector2 end = corners[(k + 1) % count];

                for (int i = 0; i < perEdge; i++)
                {
                    float t = (float)i / perEdge;
                    Vector2 point = Vector2.Lerp(start, end, t);

                    float startAngle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
                    float bulletExpansion = speed * point.magnitude;

                    SpawnSpiralBullet(
                        ctx.Pool,
                        _bulletType,
                        ctx.Origin,
                        startAngle,
                        _rotationDirection,
                        _rotationSpeed,
                        bulletExpansion);
                }
            }

            yield break;
        }

        // スパイラル弾の生成
        private void SpawnSpiralBullet(
            BulletPooling pool,
            BulletType type,
            Vector2 center,
            float angleDeg,
            int direction,
            float rotationSpeed,
            float expansionSpeed)
        {
            PooledObject bullet = pool.Spawn(type, center, Quaternion.identity);

            if (!bullet.TryGetComponent(out SpiralBulletMovement movement))
            {
                Debug.LogError("弾のプレハブに SpiralBulletMovement が付いていません", bullet);
                return;
            }

            movement.Initialize(center, angleDeg, direction, rotationSpeed, expansionSpeed);
        }
    }
}
