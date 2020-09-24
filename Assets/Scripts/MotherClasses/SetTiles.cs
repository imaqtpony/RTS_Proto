using System;
using UnityEngine;
using GD2Lib;
using static TileManager;
using System.Collections;

/// <summary>
/// Mother class, Meant to be inherited by playable objects
/// updates and determines which tile this object is standing on
/// problems encountered : cant use events here because the tiles do not belong to the same instantiation the child class has
/// </summary>
public class SetTiles : MonoBehaviour
{

    [SerializeField]
    protected GD2Lib.Event m_onFetchTileEvnt;

    [SerializeField]
    protected GD2Lib.Event m_onReturnTile;

    [SerializeField]
    protected GD2Lib.Event m_onFetchFromIndex;

    [SerializeField] private LayerMask m_ignoreRaycastMask;

    [Tooltip("The tile that was clicked")]
    protected Tile m_clickedTile;

    [Tooltip("Current tile this object is standing on")]
    protected Tile m_currentTile;
    
    [Tooltip("Next tile this object is looking at to move")]
    protected Tile m_nextTile;

    

    private void Awake()
    {
        m_ignoreRaycastMask = ~m_ignoreRaycastMask;
    }


    //private void OnEnable()
    //{
    //    // set to ignore raycast (2)
    //    //m_ignoreRaycastMask = 2;
    //    //invert bitmask to hit anything but ignoreraycastmask
    //    m_ignoreRaycastMask = ~m_ignoreRaycastMask;

    //    if (m_onReturnTile != null)
    //        m_onReturnTile.Register(HandleGetTile);
    //}

    //private void OnDisable()
    //{
    //    if (m_onReturnTile != null)
    //        m_onReturnTile.Unregister(HandleGetTile);
    //}
    

    /// <summary>
    /// updates the m_nextTile var, called when the unit is checking the next tile to move in
    /// </summary>
    /// <param name="index"> send in index </param>
    protected void UpdateNextTile(int index)
    {
        m_onFetchFromIndex.Raise(index, TileType.ADJACENT);
    }

    protected int CalculateTileIndex(String p_goName)
    {
        string emptyStr = String.Empty;

        foreach (var c in p_goName)
        {
            // Check for numeric characters
            if ((c >= '0' && c <= '9'))
            {
                emptyStr = String.Concat(emptyStr, c.ToString());
            }
        }

        int i = int.Parse(emptyStr);

        return i;
    }

}
