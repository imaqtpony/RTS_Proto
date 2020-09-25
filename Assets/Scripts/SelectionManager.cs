using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileManager;


/// <summary>
/// handles the player actions On mouse clic (game controller)
/// Attach this to Game Zone Object
/// </summary>
public class SelectionManager : MonoBehaviour
{
    [SerializeField] private LayerMask m_ignoreRaycastMask;

    [SerializeField]
    private GD2Lib.Event m_onFetchTileEvnt;
    
    [SerializeField]
    private GD2Lib.Event m_onMoveUnit;

    private PlayerUnit[] m_unitsList;
    private PlayerUnit m_currentSelectedUnit;

    private int m_nbUnits;


    private void OnEnable()
    {
        m_ignoreRaycastMask = ~m_ignoreRaycastMask;

        m_nbUnits = 2;
        // array[nb playable units]
        m_unitsList = new PlayerUnit[m_nbUnits];

        // Initialize unit list
        GameObject[] tab = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < m_nbUnits; i++)
        {
            m_unitsList[i].obj = tab[i];
            m_unitsList[i].unitScript = m_unitsList[i].obj.GetComponent<PlayableUnit>();
            //Debug.Log(m_unitsList[i]);
        }
    }


    /// <summary>
    /// If the mouse hovers the game zone collider, a clic enters the onmousedown
    /// </summary>
    private void OnMouseOver()
    {

        //bool pressed = Input.GetMouseButtonUp(1);

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            //Debug.Log(m_currentSelectedUnit.obj);
            gameObject.layer = 2;

            Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(castPoint, out RaycastHit hit, Mathf.Infinity, m_ignoreRaycastMask))
            {
                //Debug.Log(hit.collider.gameObject.transform.position);
                //Debug.Log(hit.collider.gameObject.name);


                //Whichever clic = raise 
                if (hit.collider.gameObject.CompareTag("Tile"))
                {
                    
                    // raise the pos of the tile being hit to determine clicked tile
                    m_onFetchTileEvnt.Raise(hit.collider.gameObject.transform.position, TileType.CLICKED, string.Empty);

                    //if its a tile, current unit moves to this on rightclic
                    if (Input.GetMouseButtonDown(1))
                    {
                        m_onMoveUnit.Raise(TileType.CLICKED);
                    }

                } 
                else if (hit.collider.gameObject.CompareTag("Player"))
                {
                    // else, update the selected unit
                    for (int i = 0; i < m_nbUnits; i++)
                    {
                        if (hit.collider.gameObject == m_unitsList[i].obj)
                        {
                            if (m_currentSelectedUnit.unitScript != null)
                                m_currentSelectedUnit.unitScript.m_isSelected = false;
                            m_unitsList[i].unitScript.m_isSelected = true;
                            m_currentSelectedUnit = m_unitsList[i];
                        }
                    }
                } else if (hit.collider.gameObject.CompareTag("Opponent"))
                {
                    // attack opponent
                    //move until range
                    //resolve attack
                    if (Input.GetMouseButtonDown(1))
                    {
                        //still raise as if it was a tile because in this case the unit will move to the nearest tile to attack this
                        m_onFetchTileEvnt.Raise(hit.collider.gameObject.transform.position, TileType.ATTACKED, string.Empty);

                        m_onMoveUnit.Raise(TileType.ATTACKED);
                    }
                }

            }
        }

    }

    //set back the layer or else you cant call on mouse down anymore
    private void OnMouseExit()
    {
        gameObject.layer = 0;
    }
}

public struct PlayerUnit
{
    public PlayableUnit unitScript;

    public GameObject obj;

    //other things such as Transform, name, etc

}
