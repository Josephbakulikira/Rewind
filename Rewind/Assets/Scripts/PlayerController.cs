using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cam_transform;
    public float add_gravity = 45;

    public float smooth_rotation = 10f;


    public float cam_rotation_speed = 5f;
    public float walk_speed = 9f;
    public float run_speed = 14f;
    public float max_speed = 20f;
    public float jump_power = 30f;

    // rotation limitaion for the camera
    public float cam_min_y = -60f;
    public float cam_max_y = 75f;

    private Rigidbody rigidbody;
    private Vector3 x_heading_direction;
    private Vector3 y_heading_direction;
    private float x_rotation;
    private float cam_rotation_y;
    private bool is_grounded;
    float speed;



    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        LooK();
        Move();
    }
    void LooK()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        x_rotation = x_rotation + Input.GetAxis("Mouse X") * cam_rotation_speed;
        cam_rotation_y = cam_rotation_y + Input.GetAxis("Mouse Y") * cam_rotation_speed;

        cam_rotation_y = Mathf.Clamp(cam_rotation_y, cam_min_y, cam_max_y);

        Quaternion cam_target_rotation = Quaternion.Euler(-cam_rotation_y, 0, 0);
        Quaternion body_target_rotation = Quaternion.Euler(0, x_rotation, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, body_target_rotation, Time.deltaTime * smooth_rotation);

        cam_transform.localRotation = Quaternion.Lerp(cam_transform.localRotation, cam_target_rotation, Time.deltaTime * smooth_rotation);
    }
    void Move()
    {
        x_heading_direction = cam_transform.right;
        x_heading_direction.y = 0;
        x_heading_direction.Normalize();

        y_heading_direction = cam_transform.forward;
        y_heading_direction.y = 0;
        y_heading_direction.Normalize();

        rigidbody.velocity = y_heading_direction * Input.GetAxis("Vertical") *
                            speed + x_heading_direction * Input.GetAxis("Horizontal") * speed
                            + Vector3.up * rigidbody.velocity.y;
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, max_speed);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = run_speed;
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            speed = walk_speed;
        }
    }
}
