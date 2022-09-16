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
    private PlayerAttackInfo[] attacks;
    private List<string> p_Names;
    #endregion

    #region Intialization
    private void Awake() {
        p_HealthBarOrigWidth = m_HealthBar.sizeDelta.x;
        p_LaserOrigWidth = m_Laser.sizeDelta.x;
        p_MegaLaserOrigWidth = m_MegaLaser.sizeDelta.x;
        p_SuperNovaOrigWidth = m_SuperNova.sizeDelta.x;
    }

    private void Start() {
        cooldownTime = new Dictionary<string, float>();
        activeTimer = new Dictionary<string, float>(); 
        attacks = m_Player.GetComponent<PlayerController>().Attacks;
        p_Names = new List<string>();

        foreach (var info in attacks) {
            cooldownTime[info.AttackName] = info.CooldownTimer;
            activeTimer[info.AttackName] = info.Cooldown;
            p_Names.Add(info.AttackName);
            Debug.Log(info.AttackName);
            Debug.Log(info.CooldownTimer);
            Debug.Log(info.Cooldown);
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
        // float laser_percent = 1.0f * (cooldownTime["Laser"] - activeTimer["Laser"]) / cooldownTime["Laser"];
        float laser_percent = 1.0f * (activeTimer["Laser"] / cooldownTime["Laser"]);
        float mega_laser_percent = 1.0f * (activeTimer["MegaLaser"] / cooldownTime["MegaLaser"]);
        float super_nova_percent = 1.0f * (activeTimer["SuperNova"] / cooldownTime["SuperNova"]);
        m_Laser.sizeDelta = new Vector2(p_LaserOrigWidth * laser_percent, m_Laser.sizeDelta.y);
        m_MegaLaser.sizeDelta = new Vector2(p_MegaLaserOrigWidth * mega_laser_percent, m_MegaLaser.sizeDelta.y);
        m_SuperNova.sizeDelta = new Vector2(p_SuperNovaOrigWidth * super_nova_percent, m_SuperNova.sizeDelta.y);
    }
    #endregion
    
    #region Update
    void Update() {
        foreach (string name in p_Names) {
            if (activeTimer[name] > 0) {
                var newtime = activeTimer[name] - Time.deltaTime;
                if (newtime <= 0) {
                    activeTimer[name] = 0;
                }
                else {
                    activeTimer[name] = newtime;
                }
            }
        }
        UpdateCDBars();
    }
    public void SetCD(string attackName) {
        activeTimer[attackName] = cooldownTime[attackName];
        Debug.Log(attackName + ": " + activeTimer[attackName] + " left on cd");
    }
    #endregion
}
