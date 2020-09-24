using System;
using UnityEngine;
using GD2Lib;


//// NOT USED ANYMORE

/// <summary>
/// Mother class
/// updates and determines which tile this object is standing on
/// has to be attached to a go with collider
/// </summary>
public class CurrentTile : MonoBehaviour
{

    [SerializeField]
    private GD2Lib.Event m_onFetchTileEvnt;

    [SerializeField]
    private GD2Lib.Event m_onReturnTile;

    [Tooltip("Current tile this object is standing on")]
    protected Tile m_currentTile;

    //Send tile pos when triggered
    protected void OnTriggerEnter(Collider p_other)
    {
        if (p_other.CompareTag("Tile"))
        {
            m_onFetchTileEvnt.Raise(p_other.gameObject.transform.position);
        }

    }

    private void OnEnable()
    {
        if (m_onReturnTile != null)
            m_onReturnTile.Register(HandleGetTile);
    }

    private void OnDisable()
    {
        if (m_onReturnTile != null)
            m_onReturnTile.Unregister(HandleGetTile);
    }

    //Get the tile found by the On fetch tile event
    private void HandleGetTile(GD2Lib.Event p_event, object[] p_params)
    {
        if (GD2Lib.Event.TryParseArgs(out Tile tile, p_params))
        {
            //reset current Tile before changing
            m_currentTile.isEmpty = true;
            m_currentTile = tile;
            m_currentTile.isEmpty = false;
            //Debug.Log(m_currentTile.pos);

        }
        else
        {
            Debug.LogError("Invalid type of argument!");
        }
    }


}
