using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    public float walkSpeed;
    public float rotateSpeed;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Climb();
    }
    void Move()
    {
        if (target == null)
            return;
        var v = walkSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        var h = walkSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        var offsite = transform.forward * v + transform.right * h;
        target.position += offsite;
    }
    void Climb()
    {
        if (target == null)
            return;
        if (Input.GetKey(KeyCode.Q)) target.position += transform.up * walkSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.E)) target.position -= transform.up * walkSpeed * Time.deltaTime;
    }
}
