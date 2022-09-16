using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperNovaAttack : Ability {
    public override void Use(Vector3 spawnPos) {
        var emitterShape = cc_PS.shape;
        emitterShape.radius = m_Info.Range;

        Collider[] hitColliders = Physics.OverlapSphere(spawnPos, m_Info.Range);
        
        foreach (var hit in hitColliders) {
            if (hit.GetComponent<Collider>().CompareTag("Enemy")) {
                hit.GetComponent<Collider>().GetComponent<EnemyController>().DecreaseHealth(m_Info.Power);
                Debug.Log("Smack");
            }
        }

        
        // emitterShape.length = m_Info.Range;
        cc_PS.Play();
    }
}