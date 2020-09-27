using UnityEngine;
using GD2Lib;

public class TileManager : MonoBehaviour
{
    //tile return type enum
    public enum TileType { CLICKED, CURRENT, ADJACENT, ATTACKED };

    private int m_nbTiles;
    private int m_length;

    [SerializeField]
    private ArrayData m_data;

    [SerializeField]
    private GameObject m_sampleTile;

    [SerializeField]
    private GD2Lib.Event m_onFetchTileEvnt;
    
    [SerializeField]
    private GD2Lib.Event m_onReturnTile;

    [SerializeField]
    private GD2Lib.Event m_onFetchFromIndex;

    private void OnEnable()
    {
        m_nbTiles = 100;
        m_length = (int)Mathf.Sqrt(m_nbTiles);

        m_data.m_tileArray = new Tile[m_nbTiles];

        InitializeTiles();

        if (m_onFetchTileEvnt!=null)
            m_onFetchTileEvnt.Register(HandleFetchTileFromPos);
        
        if (m_onFetchFromIndex != null)
            m_onFetchFromIndex.Register(HandleFetchTileFromIndex);
    }
    

    private void OnDisable()
    {
        if (m_onFetchTileEvnt != null)
            m_onFetchTileEvnt.Unregister(HandleFetchTileFromPos);
        
        if (m_onFetchFromIndex != null)
            m_onFetchFromIndex.Unregister(HandleFetchTileFromIndex);
    }

    private void HandleFetchTileFromPos(GD2Lib.Event p_event, object[] p_params)
    {
        if(GD2Lib.Event.TryParseArgs(out Vector3 p_pos, out TileType p_type, out string p_raisingObject, p_params))
        {

            Tile i = FindTileFromPos(p_pos);

            if (p_type == TileType.CLICKED || p_type == TileType.ATTACKED) p_raisingObject = string.Empty; //if it was clicked, we want all objects to update their m_clickedTile

            //send back the index of the given tile and dont do it if pos isnt matching any of the tile's positions
            if (m_onReturnTile != null && i.pos != Vector3.zero)
            {
                m_onReturnTile.Raise(i, p_type, p_raisingObject);
            }
            else
            {
                Debug.LogError("This is not a tile position");
            }

        } else
        {
            Debug.LogError("Invalid type of argument!");
        }
    }

    private void HandleFetchTileFromIndex(GD2Lib.Event p_event, object[] p_params)
    {
        if (GD2Lib.Event.TryParseArgs(out int p_index, out TileType p_type, p_params))
        {
            //Debug.Log(p_index);

            Tile j = ReturnTileFromIndex(p_index);


            if (m_onReturnTile != null && j.pos != Vector3.zero)
            {
                m_onReturnTile.Raise(j, p_type, string.Empty);
            } 
            else
            {
                Debug.LogError("Index out of bounds cant move there");
            }
        }
        else
        {
            Debug.LogError("Invalid type of argument!");
        }
    }


    private void InitializeTiles()
    {
        float tile_spacing = 1.2f;

        //initialize array
        for (int i = 0; i < m_length; i++)
        {
            for (int j = 0; j < m_length; j++)
            {
                //Debug.Log(transform.position + new Vector3(tile_spacing * j, 0, tile_spacing * i));
                GameObject go = Instantiate(m_sampleTile, transform.position + new Vector3(tile_spacing * j, 0, -tile_spacing * i), Quaternion.identity);
                go.name = $"{i}{j}";
                m_data.m_tileArray[i * 10 + j].pos = go.transform.position;
                m_data.m_tileArray[i * 10 + j].go = go;
                m_data.m_tileArray[i * 10 + j].isEmpty = true;

            }
        }
        
    }

    /// <summary>
    /// Find which tile is it based on a position
    /// </summary>
    private Tile FindTileFromPos(Vector3 p_pos)
    {
        //base tile
        Tile tileNb = new Tile{ pos = Vector3.zero, go = null, isEmpty = true };

        //Debug.Log(m_tileArray[22].pos.x);

        // fetch
        for (int i = 0; i < m_length; i++)
        {
            for (int j = 0; j < m_length; j++)
            {
                int tempIndex = i * 10 + j;

                if (m_data.m_tileArray[tempIndex].pos == p_pos)
                    tileNb = m_data.m_tileArray[tempIndex];
                    

            }
        }

        return tileNb;
    }

    /// <summary>
    /// Return which tile is it based on an Index
    /// </summary>
    private Tile ReturnTileFromIndex(int p_index)
    {
        //Tile tile = new Tile { pos = Vector3.zero, go = null, isEmpty = true };

        //if (p_index >= 0 && p_index < m_length) tile = m_tileArray[p_index]; Debug.Log(m_tileArray[p_index].pos);
        if (p_index >= 0 && p_index < m_nbTiles)
        {

            return m_data.m_tileArray[p_index];

        }
        else
            return new Tile { pos = Vector3.zero, go = null, isEmpty = true };
    }

}

/// <summary>
/// Tile struct, add stuff if needed
/// </summary>
public struct Tile
{
    // field
    //private bool empty;

    public Vector3 pos;

    // or mesh renderer ?
    public GameObject go;

    public bool isEmpty;

    // deactivate Go if tile isnt empty
    //public bool isEmpty {
    //    get { return empty; }
    //    set { 
    //        empty = value;
    //        //if (value)
    //        //{
    //        //    if(go!=null)
    //        //        go.SetActive(true);
    //        //} else if (!value)
    //        //{
    //        //    if (go != null)
    //        //        go.SetActive(false);
    //        //}
    //    }
    //}
}
