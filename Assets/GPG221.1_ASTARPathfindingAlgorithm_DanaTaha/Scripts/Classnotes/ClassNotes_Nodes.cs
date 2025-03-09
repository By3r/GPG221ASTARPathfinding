//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ClassNotes_Nodes : MonoBehaviour
//{
//    #region Variables
//    public Node Parent { get; set; }
//    public Vector3 NodeWorldPosition { get; set; } // The reason we have both a world and a grid position is because the math calculation would be simpler.
//    public Vector3Int GridPosition { get; set; } // Top left of the grid is going to be 0,0,0. Every node has a size. 
//                                                 // In unity ^ you would use a vector3int instead of a vector 3. For optimization purposes we would have to remove this grid because it takes memory.

//    public bool IsWalkable { get; private set; }
//    public TMP_Text gCostText; public TMP_Text hCostText; public TMP_Text fCostText;


//#if ASTAR_DEBUG

//    public GameObject nodeGO;
//        {
//        get
//        {
//            return gCost;
//        }
//    }
//    set
//        {
//        nodeGo = value;
//        gcosttexy = nodego.transform.GetChild(0).GetChild(0)
//        }

//#endif

//        private int _gCost;
//    public int GCost
//    {
//        get { return _gCost; }
//        set { _gCost = value; gCostText.text = value.ToString; fCostText.text = fCost.ToString }
//    }

//    private int _hCost;
//    public int HCost
//    {
//        get { return _hCost; }
//        set { _hCost = value; hCostText.text = value.ToString; fCostText.text = fCost.ToString; }

//    }

//    private int _fCost;
//    public int FCost // The reason why this is a public get is so that we alwayas get the value without making it editable/ changeable by other coders.
//    {
//        get { return _gCost + _hCost; }
//    }

//    // Constructor
//    public Node Node
//    {
//        // World position 
//        // isWalkable bool
//        // grid position.
//    }
//        #endregion
//}
//    }

//        /// <Summary>
//        /// Project idea:
//        /// 
//        /// That one roblox game with pigs? Piggy? Chases you down lmao.
//        /// Do some actions before you get caught by piggy. Except that it won't be
//        /// the pig but instead it will be snowy or something like that.
//        /// Generate nodes (Tiles) which the AI can walk on. Define the type of nodes in the scene
//        /// I.e. Walkable, Teleportation, Not walkable (Obstacles), Player Spawn Area etc..
//        /// 
//        ///// The player's goal is to do three tasks in total. First task is close a window. 
//        /// Second task is match the bar on time to fix electric wires. Third task is to reach the outside of the house on time.
//        /// The enemy Ai is not meant to be super scary but rather intimidating.
//        /// 
//        /// Create a two story building or villa and a garden. Make it out of Generic blocks.
//        /// 
//        /// Features
//        /// {}
//        /// - Elevator
//        /// - Staircase
//        /// - 
//        /// </Summary>
