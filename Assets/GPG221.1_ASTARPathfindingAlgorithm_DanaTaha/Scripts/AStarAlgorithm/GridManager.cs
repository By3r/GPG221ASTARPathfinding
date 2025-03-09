using UnityEngine;
using System.Collections.Generic;
using AStarPathFinding.Nodes;

namespace AStarPathFinding.MainGrid
{
    /// <summary>
    /// Responsible for creating a grid and Finding the Nodes' Neighbours.
    /// </summary>

    public class GridManager : MonoBehaviour
    {
        #region Variables
        [Tooltip("How many rows does the grid hold?")]
        [SerializeField] private int gridWidth = 10;
        [Tooltip("How many columns does the grid hold?")]
        [SerializeField] private int gridHeight = 10;
        [Tooltip("Look for the ideal size, one that lacks spacing but does not overstep others.")]
        [SerializeField] private float nodeSize = 2.29f;
        [Tooltip("Obstacle detection cast. Set as 0.4 to detect obstacles that take more than the half size of the node. the bigger the number the more false positives it will have.")]
        [SerializeField] private float SphereCastRadiusForObstacles = 0.4f;
        [Tooltip("Assign obstacle layer to obstacles and select that layer.")]
        [SerializeField] private LayerMask obstacleLayer;
        [Tooltip("Whatever prefab your heart desires, preferrably square shaped. (For better functionality, Squares and Cubes.)")]
        [SerializeField] private GameObject nodePrefab;

        private Node[,] _grid;
        #endregion

        private void Awake()
        {
            CreateGrid();
        }

        #region Public Functions
       

        /// <summary>
        /// Accesses a Node within the grid array through its world position. If the position
        /// is within grid bounds and the array contains the node, it will then be returned.
        /// </summary>
        /// <param name="worldPosition">The world position a node is in to be converted into grid position.</param>
        /// <returns>A Node at a specific grid positions</returns>
        public Node GetNode(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt((worldPosition.x - transform.position.x + gridWidth / 2) / nodeSize);
            int z = Mathf.RoundToInt((worldPosition.z - transform.position.z + gridHeight / 2) / nodeSize);

            if (x < 0 || x >= gridWidth || z < 0 || z >= gridHeight)
            {
                return null;
            }

            if (_grid[x, z] == null)
            {
                return null;
            }

            return _grid[x, z];
        }

        /// <summary>
        /// Loops through potential current node neighbours and returns whatever pass the 'security' check.
        /// i.e. informs what neighbours a Node has depending gon whether the node is outside of bounds or not.
        /// </summary>
        /// <param name="node">Current node to check the neighbours of</param>
        /// <returns>a list of neighbour nodes</returns>
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            int[,] offsets = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            for (int i = 0; i < offsets.GetLength(0); i++)
            {
                int checkX = node.gridPosition.x + offsets[i, 0];
                int checkY = node.gridPosition.y + offsets[i, 1];

                if (checkX >= 0 && checkX < gridWidth && checkY >= 0 && checkY < gridHeight)
                    neighbours.Add(_grid[checkX, checkY]);
            }

            return neighbours;
        }

        /// <summary>
        /// Loops through Nodes to figure out which nodes are blocked by obstacles and which aren't.
        /// It exists to update changed obstacle positions everytime a new path is created.
        /// </summary>
        public void RecalculateWalkability()
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector3 worldPos = _grid[x, y].worldPosition;
                    bool isWalkable = (Physics.OverlapSphere(worldPos, nodeSize * SphereCastRadiusForObstacles, obstacleLayer).Length == 0);
                    _grid[x, y].isWalkable = isWalkable;

#if ASTAR_COLORDEBUG
                    if (_grid[x, y].nodePrefab != null)
                    {
                        _grid[x, y].UpdateNodeColour(isWalkable ? Color.white : Color.magenta);
                    }
#endif
                }
            }
        }
        #endregion

        #region Private Functions
        private void CreateGrid()
        {
            _grid = new Node[gridWidth, gridHeight];
            Vector3 gridOrigin = transform.position - new Vector3(gridWidth / 2, 0, gridHeight / 2);

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector3 worldPos = gridOrigin + new Vector3(x * nodeSize, 0, y * nodeSize);
                    bool isWalkable = !Physics.CheckSphere(worldPos, nodeSize * SphereCastRadiusForObstacles, obstacleLayer);

                    Node newNode = new Node(isWalkable, new Vector3Int(x, y, 0), worldPos);
                    if (nodePrefab != null)
                        newNode.FormANodeVisual(nodePrefab, transform);
                    _grid[x, y] = newNode;
                }
            }
        }
#endregion
    }
}