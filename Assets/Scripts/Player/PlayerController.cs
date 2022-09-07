using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    #region Editor Variables
    [SerializeField] [Tooltip("How fast the player should move when running around.")]
    private float m_Speed;

    [SerializeField] [Tooltip("The transform of the camera following the player.")]
    private Transform m_CameraTransform;

    [SerializeField] [Tooltip("A list of all attacks and information about them.")]
    private PlayerAttackInfo[] m_Attacks;

    [SerializeField] [Tooltip("Amount of health that the player starts with.")]
    private int m_MaxHealth;

    [SerializeField] [Tooltip("The HUD script.")]
    private HUDController m_HUD;
    #endregion

    #region Cached References
    private Animator cr_Anim;
    private Renderer cr_Renderer;
    #endregion

    #region Cached Components
    private Rigidbody cc_Rb;
    #endregion

    #region Private Variables
    // The current move direction of the player. Does NOT include magnitude.
    private Vector2 p_Velocity;

    // In order to do anything, we can't be frozen (timer must be 0).
    private float p_FrozenTimer;

    // The default color. Cached so we can switch between colors.
    private Color p_DefaultColor;

    // Current amount of health.
    private float p_CurHealth;
    #endregion

    #region Initialization
    private void Awake() {
        p_Velocity = Vector2.zero;
        cc_Rb = GetComponent<Rigidbody>();
        cr_Anim = GetComponent<Animator>();
        cr_Renderer = GetComponentInChildren<Renderer>();
        p_DefaultColor = cr_Renderer.material.color;

        p_FrozenTimer = 0;
        p_CurHealth = m_MaxHealth;
        for (int i = 0; i < m_Attacks.Length; i++) {
            PlayerAttackInfo attack = m_Attacks[i];
            attack.Cooldown = 0;

            if (attack.WindUpTime > attack.FrozenTime) {
                Debug.LogError(attack.AttackName +
                               " has a windup time longer than the amount of time that the player is frozen for.");
            }
        }
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region Main Updates
    private void Update() {
        // Set how hard the player is pressing movement buttons
        if (p_FrozenTimer > 0) {
            p_Velocity = Vector2.zero;
            p_FrozenTimer -= Time.deltaTime;
            return;
        }
        else {
            p_FrozenTimer = 0;
        }

        // Ability Use
        for (int i = 0; i < m_Attacks.Length; i++) {
            PlayerAttackInfo attack = m_Attacks[i];

            if (attack.isReady()) {
                if (Input.GetButtonDown(attack.Button)) {
                    p_FrozenTimer = attack.FrozenTime;
                    DecreaseHealth(attack.HealthCost);
                    StartCoroutine(UseAttack(attack));
                    break;
                }
            }
            else if (attack.Cooldown > 0) {
                attack.Cooldown -= Time.deltaTime;
            }
        }


        var forward = Input.GetAxis("Vertical");
        var right = Input.GetAxis("Horizontal");

        // Updating the animation
        cr_Anim.SetFloat("Speed", Mathf.Clamp01(Mathf.Abs(forward) + Math.Abs(right)));

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

    private void FixedUpdate() {
        // Update the position of the player
        cc_Rb.MovePosition(cc_Rb.position + transform.forward * (m_Speed * Time.fixedDeltaTime * p_Velocity.magnitude));

        // Update the rotation of the player
        cc_Rb.angularVelocity = Vector3.zero;

        if (p_Velocity.sqrMagnitude > 0) {
            var angleToRotCam = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.up, p_Velocity);
            var camForward = m_CameraTransform.forward;
            var newRot = new Vector3(
                Mathf.Cos(angleToRotCam) * camForward.x - Mathf.Sin(angleToRotCam) * camForward.z,
                0,
                Mathf.Cos(angleToRotCam) * camForward.z - Mathf.Sin(angleToRotCam) * camForward.x);
            var theta = Vector3.SignedAngle(transform.forward, newRot, Vector3.up);
            cc_Rb.rotation = Quaternion.Slerp(cc_Rb.rotation, cc_Rb.rotation * Quaternion.Euler(0, theta, 0), 0.2f);
        }
    }
    #endregion

    #region Health/Dying Methods
    public void DecreaseHealth(float amount) {
        p_CurHealth -= amount;
        m_HUD.UpdateHealth(1.0f * p_CurHealth / m_MaxHealth);
        if (p_CurHealth <= 0) {
            ScoreManager.singleton.UpdateHighScore();
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void IncreaseHealth(float amount) {
        p_CurHealth += amount;
        if (p_CurHealth > m_MaxHealth) {
            p_CurHealth = m_MaxHealth;
        }

        m_HUD.UpdateHealth(1.0f * p_CurHealth / m_MaxHealth);
    }
    #endregion

    #region Attack Methods
    private IEnumerator UseAttack(PlayerAttackInfo attack) {
        cc_Rb.rotation = Quaternion.Euler(0, m_CameraTransform.eulerAngles.y, 0);
        cr_Anim.SetTrigger(attack.TriggerName);
        IEnumerator toColor = ChangeColor(attack.AbilityColor, 10);
        StartCoroutine(toColor);
        yield return new WaitForSeconds(attack.WindUpTime);


        Vector3 offset = transform.forward * attack.Offset.z + transform.right * attack.Offset.x +
                         transform.up * attack.Offset.y;
        GameObject go = Instantiate(attack.AbilityGo, transform.position + offset, cc_Rb.rotation);
        go.GetComponent<Ability>().Use(transform.position + offset);


        StopCoroutine(toColor);
        StartCoroutine(ChangeColor(p_DefaultColor, 50));
        yield return new WaitForSeconds(attack.Cooldown);

        attack.ResetCooldown();
    }
    #endregion

    #region Misc Methods
    private IEnumerator ChangeColor(Color newColor, float speed) {
        Color curColor = cr_Renderer.material.color;
        while (curColor != newColor) {
            curColor = Color.Lerp(curColor, newColor, speed / 100);
            cr_Renderer.material.color = curColor;
            yield return null;
        }
    }
    #endregion

    #region Collision Methods
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("HealthPill")) {
            IncreaseHealth(other.GetComponent<HealthPill>().HealthGain);
            Destroy(other.gameObject);
        }
    }
    #endregion
}