using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Tank))]
public class TankMovement : MonoBehaviour {

    public int tankNumber;
    public float movePower;
    public float turnSpeed;

    private Rigidbody2D m_Rb;

    public bool keepMoving;
    public bool jammed;
    public bool jamVelocity;

    private void Start()
    {
        StartCoroutine(InitProperties());
    }

    private IEnumerator InitProperties()
    {
        m_Rb = GetComponent<Rigidbody2D>();

        keepMoving = false;
        jammed = false;
        jamVelocity = false;

        yield return new WaitForSeconds(Global.WaitToSetTankNumber);

        ReInitVars();

        yield return new WaitForSeconds(Global.WaitToInitControls);

        foreach (var movementControls in FindObjectsOfType<TankMovementControls>())
        {
            if (movementControls.tankNumber == tankNumber)
            {
                movementControls.tankMovement = this;
                movementControls.Initialize();
            }
        }
    }

    public void ReInitVars()
    {
        tankNumber = GetComponent<Tank>().tankNumber;
        movePower = GetComponent<Tank>().movePower;
        turnSpeed = GetComponent<Tank>().turnSpeed;
    }

    public void TurnRight()
    {
        if (jammed)
            return;

        keepMoving = true;

        transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime));
    }

    public void TurnLeft()
    {
        if (jammed)
            return;

        keepMoving = true;

        transform.Rotate(new Vector3(0, 0, turnSpeed * Time.deltaTime));
    }

    public void MoveForward()
    {
        if (jammed)
            return;

        m_Rb.AddRelativeForce(Vector2.right * movePower * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (GetComponent<TankHealth>() != null)
        {
            if (GetComponent<TankHealth>().health <= 0)
            {
                keepMoving = false;
                jammed = true;

                if (m_Rb.velocity.magnitude < 0.01f)
                {
                    jamVelocity = true;
                }
            }
            else
            {
                jammed = false;
                jamVelocity = false;
            }
        }

        if (jamVelocity)
        {
            m_Rb.velocity *= 0;
        }
    }

    private void FixedUpdate()
    {
        if (keepMoving)
        {
            MoveForward();
        }
    }

}
