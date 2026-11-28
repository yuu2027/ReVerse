using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ReVerse.Bullet
{
    public enum BulletType
    {
        Normal,
        SpiralBlue,
        SpiralGreen,
    }

    /// <summary>
    /// オブジェクトプール用スクリプト
    /// </summary>
    public sealed class BulletPooling : MonoBehaviour
    {
        [System.Serializable]
        private struct PoolSetting
        {
            public BulletType Type;
            public PooledObject Prefab;
            public int InitialSize;
            public int MaxSize;
        }

        [Header("プールする時の設定")]
        [SerializeField] private List<PoolSetting> _settings;

        Dictionary<BulletType, ObjectPool<PooledObject>> _pools;

        //public int CountActive => _pool.CountActive;     // 使用中の数
        //public int CountInactive => _pool.CountInactive; // 待機中の数
        //public int CountAll => _pool.CountAll;           // プールが管理する総数

        private void Awake()
        {
            _pools = new Dictionary<BulletType, ObjectPool<PooledObject>>();

            foreach(var setting in _settings)
            {
                if (_pools.ContainsKey(setting.Type))
                {
                    Debug.LogError($"{setting.Type}が重複して登録されています");
                    continue;
                }

                ObjectPool<PooledObject> pool = null;
                pool = new ObjectPool<PooledObject>(
                    createFunc: () => CreateBullet(setting.Prefab, pool),
                    actionOnGet: OnGet,
                    actionOnRelease: OnRelease,
                    actionOnDestroy: OnDestroyBullet,
                    collectionCheck: true,
                    defaultCapacity: setting.InitialSize,
                    maxSize: setting.MaxSize);

                _pools.Add(setting.Type, pool);

                Prewarm(setting, pool);
            }

            //Prewarm();
        }

        // 新しいオブジェクトを生成
        private PooledObject CreateBullet(PooledObject prefab, IObjectPool<PooledObject> pool)
        {
            PooledObject obj = Instantiate(prefab, transform);

            obj.gameObject.SetActive(false);
            obj.Initialize(pool);

            return obj;
        }

        // プールから取得したとき
        private void OnGet(PooledObject obj)
        {
            obj.gameObject.SetActive(true);
        }

        // プールへ返却したとき
        private void OnRelease(PooledObject obj)
        {
            obj.gameObject.SetActive(false);
        }

        // 保持上限を超えたオブジェクトなどを破棄
        private void OnDestroyBullet(PooledObject obj)
        {
            Destroy(obj.gameObject);
        }

        // 事前に弾を生成
        private void Prewarm(PoolSetting setting, ObjectPool<PooledObject> pool)
        {
            int count = Mathf.Min(setting.InitialSize, setting.MaxSize);
            var objects = new List<PooledObject>(count);

            for (int i = 0; i < count; i++)
            {
                objects.Add(pool.Get());
            }

            foreach (PooledObject obj in objects)
            {
                pool.Release(obj);
            }
        }

        public PooledObject Spawn(BulletType type, Vector3 position, Quaternion rotation)
        {
            if (!_pools.TryGetValue(type, out var pool))
            {
                Debug.LogError("弾丸種類が登録されてない");
                return null;
            }

            PooledObject obj = pool.Get();

            obj.transform.SetPositionAndRotation(position, rotation);

            return obj;
        }

        public void Despawn(BulletType type, PooledObject obj)
        {
            if (!_pools.TryGetValue(type, out var pool))
            {
                Debug.LogError("弾丸種類が登録されてない");
                return;
            }

            pool.Release(obj);
        }

        private void OnDestroy()
        {
            if (_pools == null) return;

            _pools?.Clear();
        }
    }
}
