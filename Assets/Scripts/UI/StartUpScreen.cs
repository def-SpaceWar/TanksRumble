using UnityEngine;

public class StartUpScreen : MonoBehaviour {

    private Rigidbody2D m_Rb;
    private bool m_IsFalling;
    public static bool HasFallen;

    private void Awake()
    {
        if (HasFallen)
            gameObject.SetActive(false);
    }

    private void Update()
    {
        if (m_IsFalling)
            m_Rb.AddForce(Vector2.down * Global.StartUpScreenFallPower);

        if (transform.position.y < -5000)
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }

    public void FallDown()
    {
        gameObject.AddComponent<Rigidbody2D>();

        Invoke(nameof(Fall), 0.02f);
    }

    private void Fall()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        m_Rb.drag = 0;
        m_Rb.angularDrag = 0;
        m_Rb.gravityScale = 0;

        m_IsFalling = true;
        HasFallen = true;
    }

}
