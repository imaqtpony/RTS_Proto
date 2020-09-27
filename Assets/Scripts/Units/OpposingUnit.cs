using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileManager;

public class OpposingUnit : MonoBehaviour
{

    private UnitStats m_unitStats;

    private Tile m_currentTile;

    [SerializeField]
    private ArrayData m_data;

    private void Start()
    {
        m_unitStats = GetComponent<UnitStats>();

        //temp method
        m_currentTile = FindCurrentOccupiedTile();

        m_unitStats.currentTileIndex = CalculateTileIndex(m_currentTile.go.name);

    }

    // temp method to get currentTile
    private Tile FindCurrentOccupiedTile()
    {
        Tile t = new Tile { pos = Vector3.zero, go = null, isEmpty = true };

        for (int i=0; i < m_data.m_tileArray.Length;i++) 
        {
            if(m_data.m_tileArray[i].pos == gameObject.transform.position)
            {
                t = m_data.m_tileArray[i];
            }
        }

        return t;
    }


    private int CalculateTileIndex(String p_goName)
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
