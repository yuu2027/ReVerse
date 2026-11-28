using UnityEngine;
using UnityEngine.Pool;

namespace ReVerse.Bullet
{
    public sealed class PooledObject : MonoBehaviour
    {
        [SerializeField] private float _margin = 0.1f; // 画面外の余白（ビューポート比率）
        private Camera _camera;
        private IObjectPool<PooledObject> _pool;
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _camera = Camera.main;
        }

        public void Initialize(IObjectPool<PooledObject> pool)
        {
            _pool = pool;
        }

        public void ReturnToPool()
        {
            if (!gameObject.activeSelf) return;

            _pool.Release(this);
        }

        private void Update()
        {
            Vector3 vp = _camera.WorldToViewportPoint(transform.position);

            if (vp.x < -_margin || vp.x > 1f + _margin ||
                vp.y < -_margin || vp.y > 1f + _margin)
            {
                ReturnToPool();
            }
        }
    }
}
