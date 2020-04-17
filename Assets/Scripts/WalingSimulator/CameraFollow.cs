using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    float distance;
    Vector3 offsitePos;
    public float maxDistance;
    public float minDistance;

    public float maxRotate;
    public float minRotate;

    public float scrollSpeed;
    public float rotateSpeed;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target").transform;
        if (target == null)
            return;
        transform.LookAt(transform);
        distance = minDistance;
        offsitePos = distance * (transform.position - target.position).normalized;
        transform.position = offsitePos + target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;
        RotateView();
        transform.position = offsitePos + target.position;
    }
    
    void RotateView()
    {
        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
            target.RotateAround(target.position, Vector3.up, rotateSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);

            Vector3 originalPos = transform.position;
            Quaternion originalRot = transform.rotation;
            transform.RotateAround(target.position, transform.right, -rotateSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime);
            if (transform.eulerAngles.x < minRotate || transform.eulerAngles.x > maxRotate)
            {
                transform.position = originalPos;
                transform.rotation = originalRot;
            }
            offsitePos = (transform.position - target.position).normalized * distance;

        }
    }

}
