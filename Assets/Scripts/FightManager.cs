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

    private void OnEnable()
    {
        Initialize();

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
        m_allUnitsList = new UnitStats[10];

        //GameObject[] tab = GameObject.FindGameObjectsWithTag("Player") + GameObject.FindGameObjectsWithTag("Opponent");

        for (int i = 0; i < 10; i++)
        {
            //m_allUnitsList[i] = tab[i].GetComponent<UnitStats>();

        }
    }




}
