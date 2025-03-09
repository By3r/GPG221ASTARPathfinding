using UnityEngine;

namespace AStarPathFinding.Nodes
{
    /// <summary>
    /// Represents information a Node needs to have for seamless path calculations.
    /// </summary>

    public class Node
    {
        #region Variables
        [Tooltip("If true, a path can be formed with this Node. If not true, The node is skipped.")]
        public bool isWalkable;
        [Tooltip("The Node's location on the Grid.")]
        public Vector3Int gridPosition;
        [Tooltip("The Node's global world location.")]
        public Vector3 worldPosition;
        [Tooltip("What this Node is the Child of.")]
        public Node parent;
        [Tooltip("Preferrably square shaped. (For better functionality, Squares and Cubes.)")]
        public GameObject nodePrefab;

        [Header("Path Calculation")]
        public int gCost, hCost;
        public int fCost => gCost + hCost;

        [Tooltip("Path Calculation Version")]
        public int nodeVersion = 0;

        private Renderer _nodeRenderer;

        public Node(bool walkable, Vector3Int gridPos, Vector3 worldPos)
        {
            isWalkable = walkable;
            gridPosition = gridPos;
            worldPosition = worldPos;
        }
        #endregion

        #region Public Functions

        /// <summary>
        /// Instantiates the Node and sets it to be the child of the grid (ideally the grid.) and 
        /// then changes the Node's colour to white to represent Nodes in an open list.
        /// </summary>
        /// <param name="nodePrefab">The node gameobject gets placed here.</param>
        /// <param name="parent">The Node's parent gets placed here. prefferably have it be the grid gameobject's transform.</param>
        public void FormANodeVisual(GameObject nodePrefab, Transform parent)
        {
            this.nodePrefab = GameObject.Instantiate(nodePrefab, worldPosition, Quaternion.identity, parent);

#if ASTAR_COLORDEBUG
_nodeRenderer = this.nodePrefab.GetComponent<Renderer>();
            UpdateNodeColour(Color.white);
#endif
        }
#if ASTAR_COLORDEBUG
        /// <summary>
        /// Updates the Node's Colour.
        /// </summary>
        /// <param name="colour">Color. the colour you want to use.</param>
        public void UpdateNodeColour(Color colour)
        {
            if (_nodeRenderer != null)
                _nodeRenderer.material.color = colour;
        }
#endif
        #endregion
    }
}
