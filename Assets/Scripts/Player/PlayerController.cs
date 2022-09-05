using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Editor Variables

    [SerializeField] [Tooltip("How fast the player should move when running around.")]
    private float m_Speed;

    #endregion

    #region Private Variables

    // The current move direction of the player. Does NOT include magnitude.
    private Vector2 p_Velocity;

    #endregion

    #region Initialization

    private void Awake()
    {
        p_Velocity = Vector2.zero;
    }

    #endregion

    #region Main Updates

    private void Update()
    {
        // Set how hard the player is pressing movement buttons
        var forward = Input.GetAxis("Vertical");
        var right = Input.GetAxis("Horizontal");

        // Updating velocity
        var moveThreshold = 0.3f;

        if (forward > 0 && forward < moveThreshold)
            forward = 0;
        else if (forward < 0 && forward > -moveThreshold) forward = 0;

        if (right > 0 && right < moveThreshold) right = 0;

        if (right < 0 && right > -moveThreshold) right = 0;

        p_Velocity.Set(right, forward);
    }

    #endregion
}