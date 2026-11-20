using UnityEngine;

namespace ReVerse.Player
{
    /// <summary>
    /// プレイヤーの移動を管理するクラス
    /// </summary>
    [RequireComponent(typeof(PlayerInputReader), typeof(ReverseController))]
    public class PlayerLocomotion : MonoBehaviour
    {
        [Header("円周移動の設定")]
        [SerializeField] private Transform _centerPoint;
        [SerializeField] private float _radius = 2.0f;

        [Header("移動速度")]
        [SerializeField] private float _angularSpeed = 2.0f;


        private PlayerInputReader _input;
        private ReverseController _reverse;

        private float _currentAngle = 0.0f;
        private int _sign = 1;


        private void Awake()
        {
            _input = GetComponent<PlayerInputReader>();
            _reverse = GetComponent<ReverseController>();
        }

        private void Update()
        {
            if (_centerPoint == null) return;

            if (_input.MoveAction.magnitude < 0.001f) return;

            if (_reverse.IsReversed) _sign = -1;
            else _sign = 1;

            _currentAngle += _sign * _input.MoveAction.x * _angularSpeed * Time.deltaTime;

            //Debug.Log($"現在の角度{_currentAngle}");

            Vector3 direction = MoveDirection(_currentAngle);

            transform.position = direction;
        }

        /// <summary>
        /// 極座標変換を行い、移動方向を取得
        /// </summary>
        private Vector3 MoveDirection(float angle)
        {
            float x = _centerPoint.position.x + _radius * Mathf.Cos(angle);
            float y = _centerPoint.position.y + _radius * Mathf.Sin(angle);

            return new(x, y, transform.position.z);
        }
    }
}

