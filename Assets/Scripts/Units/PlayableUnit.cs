
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using static TileManager;


/// <summary>
/// Attached to a playable entity
/// inherits from GetTiles
/// don't forget to use base. otherwise you are not refering to the instance of the protected thing you are looking for ? to be determined
/// </summary>
public class PlayableUnit : SetTiles
{
    //whenever this unit is selected, draws a outline as a feedback
    private bool m_field;
    public bool m_isSelected
    {
        get { return m_field; }
        set
        {
            m_field = value;
            if (value)
            {
                m_rend.material.shader = m_shaderOutline;
            } else
            {
                m_rend.material.shader = m_shaderEmpty;
            }
        }
    }

    [SerializeField]
    private GD2Lib.Event m_onMoveUnit;

    [Tooltip("The number of tiles this go has to move horizontally")]
    private int m_nbT2MoveHorizontal;

    [Tooltip("The number of tiles this go has to move vertically")]
    private int m_nbT2MoveVertical;

    [SerializeField]
    [Tooltip("This unit's speed")]
    private float m_speed;

    private bool m_isMoving;

    [SerializeField]
    private Renderer m_rend;

    [SerializeField]
    private Shader m_shaderOutline;

    [SerializeField]
    private Shader m_shaderEmpty;

    private UnitStats m_unitStats;


    private void OnEnable()
    {
        m_isMoving = false;
        m_isSelected = false;
        m_nbT2MoveVertical = 0;
        m_nbT2MoveHorizontal = 0;

        m_unitStats = GetComponent<UnitStats>();


        if (m_onMoveUnit != null)
            m_onMoveUnit.Register(HandleMovement);

        if (m_onReturnTile != null)
            m_onReturnTile.Register(HandleGetTile);

    }
    
    private void OnDisable()
    {

        if (m_onMoveUnit != null)
            m_onMoveUnit.Unregister(HandleMovement);

        if (m_onReturnTile != null)
            m_onReturnTile.Unregister(HandleGetTile);
    }

    //Send tile pos when triggered
    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.CompareTag("Tile") && m_currentTile.pos != gameObject.transform.position)
        {
            m_onFetchTileEvnt.Raise(p_other.gameObject.transform.position, TileType.CURRENT, gameObject.name);
        }

    }

    private void HandleMovement(GD2Lib.Event p_event, object[] p_params)
    {
        if (GD2Lib.Event.TryParseArgs(out TileType p_type, p_params))
        {
            if (m_isSelected) //if this unit is selected
            {

                if (m_clickedTile.isEmpty)
                {
                    // ?
                    //Debug.Log(base.m_clickedTile.go.name);

                    MoveTo(p_type);

                }
                else
                {
                    Debug.Log("it's not empty");
                }
            }

        }
        else
        {
            Debug.LogError("Invalid type of argument!");
        }

    }

    /// <summary>
    /// Get the tile found by the On fetch tile event
    /// </summary>
    private void HandleGetTile(GD2Lib.Event p_event, object[] p_params)
    {
        if (GD2Lib.Event.TryParseArgs(out Tile p_tile, out TileType p_type, out string p_raisingObject, p_params))
        {

            if (p_type == TileType.CLICKED || p_type == TileType.ATTACKED) // returned tile from clicking
            {
                //raised by the SelectionManager script
                //given by the Tilemanager .cs
                m_clickedTile = p_tile;


            }
            else if (p_type == TileType.CURRENT && p_raisingObject == gameObject.name) // the tile the object is standing on
            {
                //reset current Tile before changing
                m_currentTile.isEmpty = true;
                m_currentTile = p_tile;
                m_unitStats.currentTileIndex = CalculateTileIndex(m_currentTile.go.name);
                m_currentTile.isEmpty = false;

            }
            else if (p_type == TileType.ADJACENT) //if ADJACENT we ask to update the nextTile
            {
                //Debug.Log(p_tile.pos);
                m_nextTile = p_tile;

                LookForward();

            } 
            //else if(p_type == TileType.ATTACKED)
            //{
            //    // nextTile from there
            //    // and clicked tile becomes this one adjacent to the original clic


            //}

        }
        else
        {
            Debug.LogError("Invalid type of argument!");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveTo(TileType p_type) 
    {
        CalculatePath();

        // modify destination if its an attack move
        if (p_type == TileType.ATTACKED)
        {

            if (m_nbT2MoveVertical != 0)
                if (m_nbT2MoveVertical < 0)
                    m_nbT2MoveVertical++;
                else
                    m_nbT2MoveVertical--;
            else if (m_nbT2MoveHorizontal != 0)
                if (m_nbT2MoveHorizontal < 0)
                    m_nbT2MoveHorizontal++;
                else
                    m_nbT2MoveHorizontal--;


            p_type = TileType.CLICKED;
        }


        MoveTileByTile();

        //it can teleport them by a bit
        //check there if the unit is already moving
        // if you spam clic it teleports it to the currenttile pos  at each clic // erk
        if (gameObject.transform.position != m_currentTile.pos) gameObject.transform.position = m_currentTile.pos; //smoothing

    }



    //recursivité
    private bool MoveTileByTile()
    {

        //if this unit has reached its destination
        if (/*m_clickedTile.pos == m_currentTile.pos &&*/ m_nbT2MoveHorizontal == 0 && m_nbT2MoveVertical == 0)  
        {
            return true;
        }


        //Debug.Log(" Executed");
        //Debug.Log(m_nextTile.pos);

        //Move horizontally first
        if (m_nbT2MoveHorizontal != 0)
        {
            // if >0 unit goes to the right else left
            if (m_nbT2MoveHorizontal > 0)
            {

                MoveSequence(1);
                m_nbT2MoveHorizontal--; 

            }
            else
            {

                MoveSequence(-1);
                m_nbT2MoveHorizontal++;

            }
        }
        else if (m_nbT2MoveVertical != 0) //then vertically
        {

            if (m_nbT2MoveVertical > 0)
            {

                MoveSequence(-10);
                m_nbT2MoveVertical--;

            }
            else
            {

                MoveSequence(10);
                m_nbT2MoveVertical++;

            }

        }

        //reinitialize to avoid stack overflow //or stuck between 2 tiles
        m_nextTile = new Tile { pos = Vector3.zero, go = null, isEmpty = true };

        //if (m_currentTile.pos == m_nextTile.pos)
        //if (m_currentTile.pos == gameObject.transform.position)
        //{
        //    //Debug.Log(m_nbT2MoveHorizontal);
        //    //Debug.Log(m_nbT2MoveVertical);

        //  
        //    m_isMoving = false;
        //    Debug.Log(" loop again ");
        //    MoveTileByTile();

        //}

        //if the vertical if is after the other and not its else, it will go one up then left, etc
        //whereas in the else it will be all left, then all up
        
        return false;
    }


    /// <summary>
    /// Coroutine to animate the path
    /// Maybe lerp to get a nice acceleration curve?
    /// </summary>
    public IEnumerator MoveToPos(GameObject p_objectToMove, Vector3 p_location, float p_spd)
    {
        while (p_objectToMove.transform.position != p_location)
        {
            p_objectToMove.transform.position = Vector3.MoveTowards(p_objectToMove.transform.position, p_location, p_spd * Time.deltaTime);
            //yield return new WaitForSeconds(2);
            yield return new WaitForEndOfFrame();
        }

        //recursion
        //Debug.Log(" loop again ");
        m_isMoving = false;
        MoveTileByTile();

    }

    /// <summary>
    /// Execute this code for each movement this object has to do
    /// </summary>
    private void MoveSequence(int p_indexModifier)
    {

        if (m_isMoving == false) // if this object is not moving
        {
            m_isMoving = true;

            UpdateNextTile(CalculateTileIndex(m_currentTile.go.name) + p_indexModifier);

            //Debug.Log(m_nextTile.go.name);

            if (m_nextTile.go != null)
            {
                Vector3 nextTilePos = m_nextTile.pos; // so far it can move across other stuff because its not checking if Tile is empty
                StartCoroutine(MoveToPos(gameObject, nextTilePos, m_speed));

            }
        }
        
    }


    /// <summary>
    /// Mathf.Round(a/10) < Mathf.Round(a/10) on monte de res a - res b else on descend
    /// a = position de l'unité
    /// b = position a rejoindre
    /// b%10 - a%10 = nb cases horizontales a parcourir
    /// </summary>
    private void CalculatePath()
    {
        int startingTileIndex = CalculateTileIndex(m_currentTile.go.name);
        int destinationIndex = CalculateTileIndex(m_clickedTile.go.name);

        //Debug.Log(startingTileIndex);
        //Debug.Log(destinationIndex);

        //if positive move right else move left
        m_nbT2MoveHorizontal = destinationIndex % 10 - startingTileIndex % 10;

        int startTens = Mathf.FloorToInt(startingTileIndex / 10);
        int destTens = Mathf.FloorToInt(destinationIndex / 10);

        // if negative move down if positive move up
        m_nbT2MoveVertical = startTens - destTens;

    }

    private void LookForward()
    {
        if (m_isSelected)
        {
            //Look at next Tile
            Vector3 direction = m_nextTile.pos - transform.position;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            transform.LookAt(transform.position + direction);
        }
    }



}

