using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    public static ScoreManager singleton;

    #region Private Variables
    private int m_CurScore;
    #endregion

    #region Initialization
    private void Awake() {
        if (singleton == null) {
            singleton = this;
        }
        else if (singleton != this) {
            Destroy(gameObject);
        }

        m_CurScore = 0;
    }
    #endregion

    #region Score Methods
    public void IncreaseScore(int amount) {
        m_CurScore += amount;
        UpdateHighScore(); // Added this myself
    }

    private void UpdateHighScore() {
        Debug.Log("In UpdateHighScore Function");
        if (!PlayerPrefs.HasKey("HS")) {
            PlayerPrefs.SetInt("HS", m_CurScore);
            return;
        }

        Debug.Log("Found the key, update if better.");
        int hs = PlayerPrefs.GetInt("HS");
        Debug.Log("Stored High Score is: " + hs + ", current score is: " + m_CurScore);
        if (hs < m_CurScore) {
            Debug.Log("Overwriting high score with current score");
            PlayerPrefs.SetInt("HS", m_CurScore);
        }
    }
    #endregion

    #region Destruction
    private void OnDisable() {
        Debug.Log("Disable screen reached");
        UpdateHighScore();
    }
    #endregion
}