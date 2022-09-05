using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    #region Editor Variables
    [SerializeField] [Tooltip("How fast the player should move when running around.")]
    private float m_Speed;

    [SerializeField] [Tooltip("The transform of the camera following the player.")]
    private Transform m_CameraTransform;
    #endregion

    #region Cached Components
    private Rigidbody cc_Rb;
    #endregion

    #region Private Variables
    // The current move direction of the player. Does NOT include magnitude.
    private Vector2 p_Velocity;
    #endregion

    #region Initialization
    private void Awake() {
        p_Velocity = Vector2.zero;
        cc_Rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region Main Updates
    private void Update() {
        // Set how hard the player is pressing movement buttons
        var forward = Input.GetAxis("Vertical");
        var right = Input.GetAxis("Horizontal");

        // Updating velocity
        var moveThreshold = 0.3f;

        if (forward > 0 && forward < moveThreshold) {
            forward = 0;
        }
        else if (forward < 0 && forward > -moveThreshold) {
            forward = 0;
        }

        if (right > 0 && right < moveThreshold) {
            right = 0;
        }

        if (right < 0 && right > -moveThreshold) {
            right = 0;
        }

        p_Velocity.Set(right, forward);
    }
    #endregion

    private void FixedUpdate() {
        // Update the position of the player
        cc_Rb.MovePosition(cc_Rb.position + transform.forward * (m_Speed * Time.fixedDeltaTime * p_Velocity.magnitude));

        // Update the rotation of the player
        cc_Rb.angularVelocity = Vector3.zero;

        if (p_Velocity.sqrMagnitude > 0) {
            float angleToRotCam = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.up, p_Velocity);
            Vector3 camForward = m_CameraTransform.forward;
            Vector3 newRot = new Vector3(
                Mathf.Cos(angleToRotCam) * camForward.x - Mathf.Sin(angleToRotCam) * camForward.z,
                0,
                Mathf.Cos(angleToRotCam) * camForward.z - Mathf.Sin(angleToRotCam) * camForward.x);
            float theta = Vector3.SignedAngle(transform.forward, newRot, Vector3.up);
            cc_Rb.rotation = Quaternion.Slerp(cc_Rb.rotation, cc_Rb.rotation * Quaternion.Euler(0, theta, 0), 0.2f);
        }
    }
}