using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float RotateSensitivity;
    public float Speed;
    public Transform CameraTransform;
    public Rigidbody Rigidbody;


    public bool isDomkrat = false;
    [SerializeField] private GameObject Pointer;
    private DomkratMoving moving;
    public float SpeedRotation = 10f;


    public void PickUpDomkrat(GameObject SelectedObject)
    {
        moving = SelectedObject.GetComponent<DomkratMoving>();
        moving.StartMoving(Pointer);
    }

    private float _xRotation;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _xRotation -= Input.GetAxis("Mouse Y") * RotateSensitivity;
            _xRotation = Mathf.Clamp(_xRotation, -60f, 60f);
            CameraTransform.localEulerAngles = new Vector3(_xRotation, 0f, 0f);

            if (!isDomkrat)
                transform.Rotate(0, Input.GetAxis("Mouse X") * RotateSensitivity, 0);
        }
    }

    private void FixedUpdate()
    {
        Vector3 inputVector;
        if (!isDomkrat)
        {
            inputVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        } 
        else
        {
            inputVector = new Vector3(0f, 0f, Input.GetAxis("Vertical"));
            inputVector /= 2;
            float angel = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedRotation;
            transform.RotateAround(Pointer.transform.position, Vector3.up, -angel);
            moving.Rotate(Pointer, -angel);
        }
        Vector3 speedVector = transform.TransformVector(inputVector);
        speedVector *= Speed;
        Rigidbody.velocity = new Vector3(speedVector.x, Rigidbody.velocity.y, speedVector.z);
    }
}
