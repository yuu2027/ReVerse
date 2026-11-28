using System.Collections;
using UnityEngine;

namespace ReVerse.Bullet
{
    public abstract class BulletPatternBase : ScriptableObject
    {
        [SerializeField, Range(0f, 1f)] private float _weight;
        [SerializeField, Min(0f)] private float _cost;
        [SerializeField, Min(0f)] private float _minDifficulty = 1f;
        [SerializeField, Min(0f)] private float _restAfter = 1f;

        public float Weight => _weight;
        public float Cost => _cost;

        public abstract IEnumerator Fire(PatternContext ctx);

        public virtual float GetWeight(float difficulty)
        {
            if (_minDifficulty > difficulty) return 0f;

            return _weight;
        }
    }
}
