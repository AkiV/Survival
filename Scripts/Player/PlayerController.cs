using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    public LayerMask raycastLayer;

    private Rigidbody rigidBody;
    private Circle gameArea;

    float movementSpeed = 10.0f;
    float rotationSpeed = 7.0f;

    void Awake()
    {
        gameArea = GameObject.Find("GameArea").GetComponent<Circle>();
        rigidBody = GetComponent<Rigidbody>();
    }

    public void ProcessMovement(Vector2 dir)
    {
        Vector2 move = (dir * movementSpeed * Time.deltaTime);

        Vector3 newPosition = rigidBody.position + new Vector3(move.x, 0, move.y);

        if (gameArea.InsideCircle(newPosition))
            rigidBody.MovePosition(newPosition);
    }

    public void Rotate(Vector2 dir)
    {
        // The player must rotate towards the direction of the input vector.
        // Since we know x and y components, angle can be obtained with arctan,
        // which is basically converting a vector to polar coordinates.
        float rotation = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        Quaternion newRotation = Quaternion.Euler(0, rotation, 0);

        transform.rotation = Quaternion.Lerp(rigidBody.rotation, newRotation,
                                                rotationSpeed * Time.deltaTime);
    }

    public Vector2 GetDirection()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        return new Vector2(x, y).normalized;
    }

    public Vector2 GetDirectionMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000.0f, raycastLayer.value);

        Vector3 dir = (hit.point - transform.position).normalized;

        return new Vector2(dir.x, dir.z);
    }
}