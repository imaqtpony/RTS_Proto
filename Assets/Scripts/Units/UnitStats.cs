using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attach this to any unit controlled by the AI or played
/// Set up their stats, this script is individual to all units
/// Holds their stats, for combat purposes
/// </summary>
public class UnitStats : MonoBehaviour
{
    
    //public for all

    public float health;

    public float meleeAttack;

    public float rangedAttack;

    [Tooltip("The range this unit has, more than 1 means its a ranged unit")]
    public int range = 1;

    [Tooltip("% damage reduction")]
    public float durability;

    //cover, shields, anything that dodges attacks ?
    [Tooltip("Not implemented yet")]
    public int blockValue;

    public int arrayIndex;

    [Tooltip("is it a player-controlled unit ?")]
    public bool isPlayable = false;

    public int currentTileIndex;

}
