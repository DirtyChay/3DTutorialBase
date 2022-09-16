/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownTime : MonoBehaviour {
    private GameObject player;
    private Dictionary<string, float> cooldownTime;

    private Dictionary<string, float> activeTimer;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindWithTag("Player");
        PlayerAttackInfo[] attacks = player.GetComponent<PlayerController>().m_Attacks;
        foreach (var attack in attacks) {
            cooldownTime[attack.AttackName] = attack.Cooldown;
            activeTimer[attack.AttackName] = 0;
        }
    }

    // Update is called once per frame
    void Update() {
        foreach (var name in activeTimer.Keys) {
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
    }

    public void SetCD(string attackName) {
        Debug.Log(attackName + " cooldown started.");
        activeTimer[attackName] = cooldownTime[attackName];
    }
}*/