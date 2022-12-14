using UnityEngine;

public class PacmanMovement : MonoBehaviour {

    private void LateUpdate()
    {
        if (transform.position.x < -10.5f)
        {
            transform.position = new Vector3(10.5f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 10.5f)
        {
            transform.position = new Vector3(-10.5f, transform.position.y, transform.position.z);
        }

        if (transform.position.y < -5.5f)
        {
            transform.position = new Vector3(transform.position.x, 5.5f, transform.position.z);
        }
        else if (transform.position.y > 5.5f)
        {
            transform.position = new Vector3(transform.position.x, -5.5f, transform.position.z);
        }
    }

}
