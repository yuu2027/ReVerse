using System.Collections;
using UnityEngine;

namespace ReVerse.Bullet
{
    /// <summary>
    /// 弾幕の方向・位置・弾数の計算
    /// </summary>
    [RequireComponent(typeof(BulletPooling))]
    public class BulletPattern : MonoBehaviour
    {
        [Header("プレイヤーの座標")]
        [SerializeField] private Transform _playerPos;

        private BulletPooling _pool;

        private void Awake()
        {
            _pool = GetComponent<BulletPooling>();
        }

        private void Start()
        {
            //StartCoroutine(ExpandingSpiralBullet(20, 5f, 2f, 0.5f));

            SpiralStarBullet(4, 1, 2, 2);
        }

        // 単発誘導弾
        public IEnumerator SingleGuidedBullet(float speed)
        {
            if (speed <= 0) yield break;

            Vector2 direction = CalculateDirectionToPlayer();

            PooledObject bullet = _pool.Spawn(BulletType.Normal, transform.position, Quaternion.identity);

            BulletMovement movement = bullet.GetComponent<BulletMovement>();
            movement.Initialize(direction.normalized, speed);

            yield return null;
        }

        // 全方位弾
        public IEnumerator OmnidirectionalBullet(float fireInterval, float speed)
        {
            if (fireInterval <= 0 || speed <= 0) yield break;

            for (float i = 0; i < 360; i += fireInterval)
            {
                float angle = i * Mathf.Deg2Rad;

                Vector2 direction = new(Mathf.Cos(angle), Mathf.Sin(angle));

                PooledObject bullet = _pool.Spawn(BulletType.Normal, transform.position, Quaternion.identity);
                
                BulletMovement movement = bullet.GetComponent<BulletMovement>();
                movement.Initialize(direction.normalized, speed);

                yield return null;
            }
        }

        // ワインダー弾
        public IEnumerator WinderBullet(
            float shotCount, 
            float angleStep, 
            float waveFrequency, 
            float fireInterval, 
            float speed)
        {
            if (speed <= 0) yield break;

            Vector2 directionToPlayer = CalculateDirectionToPlayer();
            int count = 0;

            while (count < shotCount)
            {
                //Vector2 direction = baseDirection * Mathf.Sin(count * Mathf.Deg2Rad) * 30.0f;

                for (float i = -60; i <= 60; i += angleStep)
                {
                    float angle = i + Mathf.Sin(count * waveFrequency) * 30.0f;

                    Vector2 direction = Quaternion.Euler(0f, 0f, angle) * directionToPlayer;

                    PooledObject bullet = _pool.Spawn(BulletType.Normal, transform.position, Quaternion.identity);

                    BulletMovement movement = bullet.GetComponent<BulletMovement>();
                    movement.Initialize(direction.normalized, speed);
                }

                yield return new WaitForSeconds(fireInterval);

                count++;
            }
            yield return null;
        }

        // 渦巻き弾
        public IEnumerator SpiralBullet(int angleInterval, float fireInterval, float speed)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 360; j+= angleInterval)
                {
                    float angle = j * Mathf.Deg2Rad;

                    Vector2 direction = new(Mathf.Cos(angle), Mathf.Sin(angle));

                    PooledObject bullet = _pool.Spawn(BulletType.Normal, transform.position, Quaternion.identity);

                    BulletMovement movement = bullet.GetComponent<BulletMovement>();
                    movement.Initialize(direction.normalized, speed);

                    yield return new WaitForSeconds(fireInterval);
                }
            }
            
            yield return null;
        }

        // 拡散渦巻き弾：時計回り・反時計回りの2発を全方位に発射
        public IEnumerator ExpandingSpiralBullet(
            float angleStep,      // 何度おきに撃つか
            float startAngle,     // 開始角度（度）
            float rotationSpeed,  // 回る速さ（度/秒）
            float expansionSpeed) // 広がる速さ（単位/秒）
        {
            if (angleStep <= 0f) yield break;

            Vector2 center = transform.position;

            for (int i = 0; i < 4; i++)
            {
                for (float j = 0f; j < 360f; j += angleStep)
                {
                    float angle = startAngle + j;

                    SpawnSpiralBullet(BulletType.SpiralBlue, center, angle, 1, rotationSpeed, expansionSpeed); // 反時計回り
                    SpawnSpiralBullet(BulletType.SpiralGreen, center, angle, -1, rotationSpeed, expansionSpeed); // 時計回り
                }

                yield return new WaitForSeconds(1.5f);
            }
            

            yield break;
        }

        public IEnumerator StarBullet(int bulletsPerEdge, float speed)
        {
            Vector2[] corners = new Vector2[10]; // 10個の頂点

            for (int k = 0; k < 10; k++)
            {
                float radius = (k % 2 == 0) ? 1f : 0.38f;
                float angle = (90f + 36f * k) * Mathf.Deg2Rad;

                corners[k] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            }

            for (int k = 0; k < 10; k++)
            {
                Vector2 start = corners[k];
                Vector2 end = corners[(k + 1) % 10];

                for (int i = 0; i < bulletsPerEdge; i++)
                {
                    float t = (float)i / bulletsPerEdge;
                    Vector2 point = Vector2.Lerp(start, end, t);

                    Vector2 direction = point.normalized;
                    float bulletSpeed = speed * point.magnitude;

                    PooledObject bullet = _pool.Spawn(BulletType.Normal, transform.position, Quaternion.identity);

                    BulletMovement movement = bullet.GetComponent<BulletMovement>();
                    movement.Initialize(direction.normalized, bulletSpeed);

                    yield return null;
                }
            }
        }

        public IEnumerator SpiralStarBullet(
            int bulletsPerEdge, 
            int rotationDirection, 
            float rotationSpeed, 
            float expansionSpeed)
        {
            Vector2[] corners = new Vector2[10];

            for (int k = 0; k < 10; k++)
            {
                float radius = (k % 2 == 0) ? 1f : 0.38f;
                float angle = (90f + 36f * k) * Mathf.Deg2Rad;

                corners[k] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            }

            for (int k = 0; k < 10; k++)
            {
                Vector2 start = corners[k];
                Vector2 end = corners[(k + 1) % 10];

                Vector2 center = transform.position;

                for (int i = 0; i < bulletsPerEdge; i++)
                {
                    float t = (float)i / bulletsPerEdge;
                    Vector2 point = Vector2.Lerp(start, end, t);

                    float startAngle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;
                    float bulletExpansion = expansionSpeed * point.magnitude;

                    SpawnSpiralBullet(
                        BulletType.SpiralBlue,
                        center,
                        startAngle,
                        rotationDirection,
                        rotationSpeed,
                        bulletExpansion);

                    yield return null;
                }
            }
        }

        // スパイラル弾の生成
        private IEnumerator SpawnSpiralBullet(
            BulletType type, 
            Vector2 center, 
            float angleDeg, 
            int direction,
            float rotationSpeed, 
            float expansionSpeed)
        {
            PooledObject bullet = _pool.Spawn(type, center, Quaternion.identity);

            if (!bullet.TryGetComponent(out SpiralBulletMovement movement))
            {
                Debug.LogError("弾のプレハブに SpiralBulletMovement が付いていません", bullet);
                yield break;
            }

            movement.Initialize(center, angleDeg, direction, rotationSpeed, expansionSpeed);

            yield return null;
        }

        // プレイヤーとの角度
        private float CalculateAngleToPlayer()
        {
            float dx = _playerPos.position.x - transform.position.x;
            float dy = _playerPos.position.y - transform.position.y;

            return Mathf.Atan2(dy, dx);
        }

        // プレイヤーとの方向
        private Vector2 CalculateDirectionToPlayer()
        {
            float dx = _playerPos.position.x - transform.position.x;
            float dy = _playerPos.position.y - transform.position.y;

            Vector2 direction = new Vector2(dx, dy);

            return direction.normalized;
        }
    }
}
