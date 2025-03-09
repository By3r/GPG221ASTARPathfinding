using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStarPathFinding.Path;
using AStarPathFinding.Nodes;
using AStarPathFinding.MainGrid;

namespace AStarPathFinding.PathFinder
{
    public class CapsuleMover : MonoBehaviour
    {
        #region Variables
        [Tooltip("Assign the object that represents the end point/ you want this capsule to follow.")]
        public Transform endPoint;

        [Header("Scripts")]
        public GridManager gridScript;
        public AStarPathfinder pathfinderScript;

        [Header("Tweakable Values")]
        public float movementSpeed = 2f;
        [SerializeField] private int capsuleHeightOffset = 1;

        private List<Node> _path;
        private int _targetIndex = 0;
        private bool _isMoving = false;
        private Vector3 _lastEndPoint;
        #endregion

        private void Start()
        {
#if ASTAR_ALGORITHMDEBUG
            _lastEndPoint = endPoint.position;
            StartCoroutine(FollowPath());
#endif
        }

        private void Update()
        {
#if ASTAR_ALGORITHMDEBUG
            if (Vector3.Distance(endPoint.position, _lastEndPoint) > 0.1f)
            {
                _lastEndPoint = endPoint.position;
                _path = pathfinderScript.FindPath(transform.position, endPoint.position);
                _targetIndex = 0;
            }
#endif
        }

        #region Private Functions
#if ASTAR_ALGORITHMDEBUG
        private IEnumerator FollowPath()
        {
            _isMoving = true;

            while (true)
            {
                if (_path == null || _path.Count == 0)
                {
                    yield return null;
                    continue;
                }

                if (_targetIndex >= _path.Count)
                {
                    _isMoving = false;
                    yield return null;
                    continue;
                }

                Vector3 targetPosition = _path[_targetIndex].worldPosition + new Vector3(0, capsuleHeightOffset, 0);

                while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                    yield return null;

                    if (Vector3.Distance(endPoint.position, _lastEndPoint) > 0.1f)
                    {
                        break;
                    }
                }
                _targetIndex++;
            }
        }
#endif
        #endregion
    }
}