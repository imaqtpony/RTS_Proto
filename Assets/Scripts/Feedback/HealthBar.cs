using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Transform m_parent;

    [SerializeField]
    private Slider m_health;

    private UnitStats m_stats;

    private int m_frames;

    private void OnEnable()
    {
        m_frames = 0;

        m_stats = GetComponentInParent<UnitStats>();

        m_health.maxValue = m_stats.health;

    }

    private void LateUpdate()
    {
        m_frames++;
        if (m_frames % 10 == 0)// every 10 frames
        {
            m_health.value = m_stats.health;

            transform.rotation = Quaternion.Euler(40, 0, 0);

            m_frames = 0;
        }
        
    }


}
