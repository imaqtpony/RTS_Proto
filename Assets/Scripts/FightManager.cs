using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script has the sum up of all of the units stats
/// Calculates range between them, and resolve attacks
/// attach this to the tilemanager go
/// </summary>
public class FightManager : MonoBehaviour
{

    private UnitStats[] m_allUnitsList;

    [SerializeField]
    private ArrayData m_data; // not used yet

    private int m_framesCount;

    private void OnEnable()
    {
        Initialize();

        StartCoroutine(UpdateTargetsInRange());

        //if(event != null)
        //    event.Register(HandleAttack);

    }

    private void OnDisable()
    {
        //if (event != null)
        //    event.Unregister(HandleAttack);
    }

    private void Initialize()
    {

        GameObject[] tab = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] tab2 = GameObject.FindGameObjectsWithTag("Opponent");

        int length = tab.Length + tab2.Length;

        m_allUnitsList = new UnitStats[length];

        for (int i = 0; i < length; i++)
        {
            if (tab.Length > i)
            {
                m_allUnitsList[i] = tab[i].GetComponent<UnitStats>();
                m_allUnitsList[i].arrayIndex = i;
                m_allUnitsList[i].isPlayable = true;

            }
            else
            {
                m_allUnitsList[i] = tab2[i- tab.Length].GetComponent<UnitStats>();
                m_allUnitsList[i].arrayIndex = i;

            }
            //if (m_allUnitsList[i] != null)
            //    Debug.Log(m_allUnitsList[i].health);

        }
    }


    //replace update with this
    private IEnumerator UpdateTargetsInRange()
    {
        while (true)
        {

            SetUpFights(true);

            yield return new WaitForSeconds(0.5f);

            SetUpFights(false);

            //update targets every 0.5s for each
            // also means global attack speed is 1s
            yield return new WaitForSeconds(0.5f); 
        }
    }

    private void SetUpFights(bool p_isUnitPlayable)
    {
        

        for (int i = 0; i < m_allUnitsList.Length; i++)
        {
            UnitStats currUnit = m_allUnitsList[i];

            //temp var to attack only once
            bool hasAttacked = false;

            //if isPlayable is true, and curr unit also is, then resolve against opponents
            //if isPlayable is false, and curr unit also is, then resolve against player units
            if (p_isUnitPlayable == currUnit.isPlayable)
            {
                for (int j = 0; j < m_allUnitsList.Length; j++)
                {
                    if (!p_isUnitPlayable == m_allUnitsList[j].isPlayable) //attack the opposite
                    {
                        UnitStats currTarget = m_allUnitsList[j];
                        // horizontal dist between target and player
                        int hDist = currTarget.currentTileIndex % 10 - currUnit.currentTileIndex % 10;

                        //vertical dist between oppo and player
                        int vDist = Mathf.FloorToInt(currUnit.currentTileIndex/10) - Mathf.FloorToInt(currTarget.currentTileIndex/10);

                        //range needed to reach
                        int isInRange = Mathf.Abs(hDist) + Mathf.Abs(vDist);

                        //if (isUnitPlayable == false)
                        //{
                        //    Debug.LogWarning("AI ATTACKING");

                        //}

                        if (isInRange <= currUnit.range) // then you can hit it
                        {
                            if(hasAttacked == false) //remove this when creating array later ^^
                            {
                                hasAttacked = true;

                                if (currUnit.rangedAttack != 0 && isInRange != 1)
                                {
                                    //Debug.Log("Ranged unit shooting");

                                    //create array of units in range and shoot the closest ? how bad it is half null arrays ?

                                    ResolveAttack(currUnit, currTarget, true);

                                }
                                else if ((currUnit.rangedAttack != 0) && isInRange == 1)
                                {
                                    //Debug.Log("Ranged unit going in melee");
                                    ResolveAttack(currUnit, currTarget, false);

                                }

                                if (currUnit.range == 1)
                                {
                                    //Debug.Log("Melee unit fighting");
                                    ResolveAttack(currUnit, currTarget, false);

                                }
                            }
                            
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// Not done yet
    /// This is the damage resolver, health < 0 still left to do
    /// same goes for health bar somewhere else
    /// </summary>
    private void ResolveAttack(UnitStats p_attacker, UnitStats p_defender, bool p_isRanged)
    {
        if (p_isRanged)
        {
            //calculate damage taken : durability gives % dmg reduction 
            float damageReduction = p_attacker.rangedAttack * p_defender.durability / 100; 

            p_defender.health -= p_attacker.rangedAttack - damageReduction;
            //feedback damage taken
            //coroutine lasting as long as the animation

        }
        else
        {
            //same but for melee
            float damageReduction = p_attacker.meleeAttack * p_defender.durability / 100;

            p_defender.health -= p_attacker.meleeAttack - damageReduction;
        }


    }



}
