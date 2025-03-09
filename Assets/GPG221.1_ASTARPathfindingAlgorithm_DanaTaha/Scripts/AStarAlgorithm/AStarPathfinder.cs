using System.Collections.Generic;
using UnityEngine;
using AStarPathFinding.Nodes;
using AStarPathFinding.MainGrid;

namespace AStarPathFinding.Path
{
    /// <summary>
    /// Runs through all nearby nodes in an open list to calculate the least expensive path possible to take.
    /// </summary>

    public class AStarPathfinder : MonoBehaviour
    {
        #region Variables
        [Header("Scripts")]
        [Tooltip("Assign the object that has GridManager script here.")]
        [SerializeField] private GridManager gridScript;

        [Tooltip("Changes the Colour of the Nodes in the Chosen Path.")]
        [SerializeField] private Color _chosenPathColour = Color.green;
        [Tooltip("Changes the Colours of the Neighbour Nodes.")]
        [SerializeField] private Color _neighbourColours = Color.yellow;
        [Tooltip("Changes the Colour of the Nodes in a Closed Set.")]
        [SerializeField] private Color _closedSet = Color.red;

        private int _pathVersion = 0;
        #endregion

        #region Public Functions
        /// <summary>
        /// Searches for the ideal path by searching for the lowest fCost possible
        /// to the end goal.
        /// </summary>
        /// <param name="startPos">Where the path starts.</param>
        /// <param name="endPos">Where the path ends.</param>
        /// <returns>Returns an empty list. (No path was found)</returns>
#if ASTAR_ALGORITHMDEBUG
        public List<Node> FindPath(Vector3 startPos, Vector3 endPos)
        {
            _pathVersion++;

            Node startNode = gridScript.GetNode(startPos);
            Node endNode = gridScript.GetNode(endPos);

            if (startNode == null || endNode == null)
            {
                return new List<Node>();
            }

            gridScript.RecalculateWalkability();

            if (!startNode.isWalkable || !endNode.isWalkable)
            {
                return new List<Node>();
            }

            List<Node> openSet = new List<Node> { startNode };
            HashSet<Node> closedSet = new HashSet<Node>();

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost ||
                       (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

#if ASTAR_COLORDEBUG
                currentNode.UpdateNodeColour(_closedSet);
#endif
                currentNode.nodeVersion = _pathVersion;

                if (currentNode == endNode)
                {
                    return RetracePath(startNode, endNode);
                }

                foreach (Node neighbour in gridScript.GetNeighbours(currentNode))
                {
                    if (!neighbour.isWalkable || closedSet.Contains(neighbour) || neighbour.nodeVersion == _pathVersion)
                        continue;

                    int newCostToNeighbour = currentNode.gCost + 10;

                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, endNode);
                        neighbour.parent = currentNode;
                        neighbour.nodeVersion = _pathVersion;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
#if ASTAR_COLORDEBUG
                            neighbour.UpdateNodeColour(_neighbourColours);
#endif
                        }
                    }
                }
            }
            return new List<Node>();
        }
#endif
        #endregion

        #region Private Functions
#if ASTAR_ALGORITHMDEBUG
        private List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Add(startNode);
            path.Reverse();

#if ASTAR_COLORDEBUG
            foreach (Node node in path)
            {
                node.UpdateNodeColour(_chosenPathColour);
            }
#endif
            return path;
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
            int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);
            return dstX + dstY;
        }
#endif
        #endregion
    }
}
