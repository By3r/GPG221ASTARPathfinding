using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStarPathFinding.MainGrid;
using AStarPathFinding.Nodes;
using AStarPathFinding.Path;

namespace Gameplay.ASTAR
{
    /// <summary>
    /// Utilises ASTAR to move the cat towards an end goal. Unlike Capsule Mover, 
    /// the script allows the object to face the direction it is moving towards.
    /// </summary>
    public class CatMover : MonoBehaviour
    {
        #region Variables
        [Header("Movement")]
        public float movementSpeed = 15f;

        [Header("Script References")]
        public AStarPathfinder pathfinder;
        [SerializeField] private GridManager gridManager;

        private List<Node> _path;
        private int _targetIndex = 0;
        private const float _heightOffset = 1f;
        private bool _isMoving = false;
        private Vector3 _lastEndpointPosition;
        #endregion

        #region Public Function
        public void MoveTo(Vector3 destination)
        {
            #region Recalculate path if destination changed
            if (destination != _lastEndpointPosition)
            {
                _lastEndpointPosition = destination;
                _path = pathfinder.FindPath(transform.position, destination);
                _targetIndex = 0;
            }
            #endregion

            if (_isMoving) return;

            #region Move to destination
            _isMoving = true;
            if (_path != null && _path.Count > 0)
            {
                StopAllCoroutines();
                StartCoroutine(FollowPath());
            }
            #endregion
        }
        #endregion

#if ASTAR_ALGORITHMDEBUG
        private IEnumerator FollowPath()
        {
            while (_targetIndex < _path.Count)
            {
                Vector3 targetPos = _path[_targetIndex].worldPosition + new Vector3(0, _heightOffset, 0);

                #region Move until you reach the target 
                while (Vector3.Distance(transform.position, targetPos) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);

                    Vector3 direction = (targetPos - transform.position).normalized;
                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * movementSpeed);
                    }

                    yield return null;
                }
                #endregion

                _targetIndex++;
            }
            _isMoving = false;
        }
#endif
    }
}