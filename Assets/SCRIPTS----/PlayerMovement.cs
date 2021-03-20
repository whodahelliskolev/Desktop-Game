using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float rotationSpeed;
    public GameObject camPos;

    private Rigidbody rb;
    private void Awake()
    {

        rb = gameObject.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Run();
        RotateWithCamera();
        Run();
    }

    void RotateWithCamera()
    {
        rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f, rotationSpeed * Input.GetAxis("Mouse X"), 0f));
    }

    void Run()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed, ForceMode.VelocityChange);

        if(rb.velocity.sqrMagnitude > maxSpeed)
        {
            rb.velocity *= maxSpeed;
        }
    }
}
