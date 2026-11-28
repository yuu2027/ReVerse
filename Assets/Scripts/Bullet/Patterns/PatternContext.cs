using UnityEngine;

namespace ReVerse.Bullet
{
    public class PatternContext
    {
        public BulletPooling Pool;
        public Vector2 Origin;          // 発射位置
        public Transform Player;
        public float Difficulty;        // 0〜1
        public float SpeedMultiplier;
        public float DensityMultiplier;
    }
}
