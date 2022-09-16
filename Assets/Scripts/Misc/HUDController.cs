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

    [SerializeField] [Tooltip("Player GO")] private GameObject m_Player;
    #endregion
    
    #region Private Variables
    private float p_HealthBarOrigWidth;
    
    private float p_LaserOrigWidth;
    private float p_MegaLaserOrigWidth;
    private float p_SuperNovaOrigWidth;
    
    private Dictionary<string, float> cooldownTime;
    private Dictionary<string, float> activeTimer;
    private String[] p_AttackNames;
    #endregion

    #region Intialization
    private void Awake() {
        p_HealthBarOrigWidth = m_HealthBar.sizeDelta.x;
        p_LaserOrigWidth = m_Laser.sizeDelta.x;
        p_MegaLaserOrigWidth = m_MegaLaser.sizeDelta.x;
        p_SuperNovaOrigWidth = m_SuperNova.sizeDelta.x;
    }

    private void Start() {
        PlayerAttackInfo[] attacks = m_Player.GetComponent<PlayerController>().m_Attacks;
        foreach (var attack in attacks) {
            cooldownTime[attack.AttackName] = attack.Cooldown;
            activeTimer[attack.AttackName] = 0;
            Debug.Log(attack.AttackName);
        }
    }
    #endregion
    
    #region Update Health Bar
    public void UpdateHealth(float percent) {
        m_HealthBar.sizeDelta = new Vector2(p_HealthBarOrigWidth * percent, m_HealthBar.sizeDelta.y);
    }
    #endregion
    
    #region Update CD Bars
    private void UpdateCDBars() {
        m_Laser.sizeDelta = new Vector2(p_LaserOrigWidth * ((activeTimer["Laser"] / cooldownTime["Laser"])), m_Laser.sizeDelta.y);
        m_MegaLaser.sizeDelta = new Vector2(p_MegaLaserOrigWidth * (activeTimer["MegaLaser"] / cooldownTime["MegaLaser"]), m_MegaLaser.sizeDelta.y);
        m_SuperNova.sizeDelta = new Vector2(p_SuperNovaOrigWidth * (activeTimer["SuperNova"] / cooldownTime["SuperNova"]), m_SuperNova.sizeDelta.y);
    }
    #endregion
    
    #region Update
    void Update() {
        foreach (string name in activeTimer.Keys) {
            if (activeTimer[name] > 0) {
                var newtime = activeTimer[name] - Time.deltaTime;
                if (newtime <= 0) {
                    activeTimer[name] = 0;
                }
                else {
                    activeTimer[name] = (float)System.Math.Round((double)newtime, 2);
                }
            }
        }
        UpdateCDBars();
    }
    public void SetCD(string attackName) {
        Debug.Log(attackName + " cooldown started.");
        activeTimer[attackName] = cooldownTime[attackName];
    }
    #endregion
}
