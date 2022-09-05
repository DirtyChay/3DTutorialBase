using UnityEngine;

public class EnemyController : MonoBehaviour {
    #region Editor Variables
    [SerializeField] [Tooltip("How much health this enemy has.")]
    private int m_MaxHealth;

    [SerializeField] [Tooltip("How fast the enemy can move.")]
    private float m_Speed;

    [SerializeField] [Tooltip("Approximate amount of damage dealt per frame.")]
    private float m_Damage;

    [SerializeField] [Tooltip("The explosion that occurs when this enemy dies.")]
    private ParticleSystem m_DeathExplosion;
    #endregion

    #region Private Variables
    private float p_curHealth;
    #endregion

    #region Cached Components
    private Rigidbody cc_Rb;
    #endregion

    #region Cached References
    private Transform cr_Player;
    #endregion

    #region Initialization
    private void Awake() {
        p_curHealth = m_MaxHealth;

        cc_Rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        cr_Player = FindObjectOfType<PlayerController>().transform;
    }
    #endregion

    #region Main Updates
    private void FixedUpdate() {
        var dir = cr_Player.position - transform.position;
        dir.Normalize();
        cc_Rb.MovePosition(cc_Rb.position + dir * (m_Speed * Time.fixedDeltaTime));
    }
    #endregion

    #region Collision Methods
    private void OnCollisionStay(Collision collision) {
        var other = collision.collider.gameObject;
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().DecreaseHealth(m_Damage);
        }
    }
    #endregion

    #region Health Methods
    public void DecreaseHealth(float amount) {
        Instantiate(m_DeathExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    #endregion
}