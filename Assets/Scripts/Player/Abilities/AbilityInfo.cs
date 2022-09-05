using System;
using UnityEngine;

[Serializable]
public class AbilityInfo {
    #region Editor Variables
    [SerializeField] [Tooltip("How much power this ability has")]
    private int m_Power;

    public int Power => m_Power;

    [SerializeField]
    [Tooltip("If this is an attack that shoots something out, this value describes how far the attack can shoot.")]
    private float m_Range;

    public float Range => m_Range;
    #endregion
}