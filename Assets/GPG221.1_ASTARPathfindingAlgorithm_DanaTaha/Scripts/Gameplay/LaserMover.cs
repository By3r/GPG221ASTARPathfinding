using System;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Moves the laser and informs relevant scripts of its movement.
    /// </summary>
    public class LaserMover : MonoBehaviour
    {
        #region Variables
        public event Action<Vector3> OnLaserMoved;

        [Tooltip("How high above ground is the laser?")]
        [SerializeField] private float yPosition = 0f;

        private const float NewPositionDistance = 0.3f;
        private Camera _mainCamera;
        private Vector3 _lastPosition;
        #endregion

        private void Start()
        {
            _mainCamera = Camera.main;
            _lastPosition = transform.position;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryMoveLaser();
            }
        }

        private void TryMoveLaser()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.up, Vector3.up * yPosition);

            if (!plane.Raycast(ray, out float enter)) return;
            var hitPoint = ray.GetPoint(enter);

            if (Vector3.Distance(hitPoint, _lastPosition) < NewPositionDistance)
                return;

            transform.position = hitPoint;
            _lastPosition = hitPoint;
            OnLaserMoved?.Invoke(hitPoint);
        }
    }
}
