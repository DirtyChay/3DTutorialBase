using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {
    #region Editor Variables
    [SerializeField] [Tooltip("The part of the health that decreases.")]
    private RectTransform m_HealthBar;

    [SerializeField] [Tooltip("Laser CD")]
    private RectTransform m_Laser;
    
    [SerializeField] [Tooltip("MegaLaser CD")]
    private RectTransform m_MegaLaser;
    
    [SerializeField] [Tooltip("SuperNova CD")]
    private RectTransform m_SuperNova;
    #endregion
    
    #region Private Variables
    private float p_HealthBarOrigWidth;
    
    private float p_LaserOrigWidth;
    private float p_MegaLaserOrigWidth;
    private float p_SuperNovaOrigWidth;
    #endregion

    #region Intialization
    private void Awake() {
        p_HealthBarOrigWidth = m_HealthBar.sizeDelta.x;
        p_LaserOrigWidth = m_Laser.sizeDelta.x;
        p_MegaLaserOrigWidth = m_MegaLaser.sizeDelta.x;
        p_SuperNovaOrigWidth = m_SuperNova.sizeDelta.x;
    }
    #endregion
    
    #region Update Health Bar
    public void UpdateHealth(float percent) {
        m_HealthBar.sizeDelta = new Vector2(p_HealthBarOrigWidth * percent, m_HealthBar.sizeDelta.y);
    }
    #endregion
    
    #region Update CD Bars
    public void UpdateCDBars(float percent) {
        m_HealthBar.sizeDelta = new Vector2(p_HealthBarOrigWidth * percent, m_HealthBar.sizeDelta.y);
        m_HealthBar.sizeDelta = new Vector2(p_HealthBarOrigWidth * percent, m_HealthBar.sizeDelta.y);
        m_HealthBar.sizeDelta = new Vector2(p_HealthBarOrigWidth * percent, m_HealthBar.sizeDelta.y);
    }
    
    
    #endregion
}
