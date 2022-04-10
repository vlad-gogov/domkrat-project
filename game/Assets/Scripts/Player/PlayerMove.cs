using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float RotateSensitivity;
    public float Speed;
    public Transform CameraTransform;
    public Rigidbody Rigidbody;


    public bool isDomkrat = false;
    [SerializeField] private GameObject PointerDomkrat;
    private GameObject PointerRuchka;
    private DomkratMoving moving;
    public float SpeedRotation = 20f;
    bool isDaun = false;

    void Start()
    {
        PointerRuchka = gameObject.transform.GetChild(0).gameObject;
    }

    public void PickUpDomkrat(GameObject SelectedObject)
    {
        moving = SelectedObject.GetComponent<DomkratMoving>();
        moving.StartMoving(PointerDomkrat);
    }

    private float _xRotation;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
        if (Input.GetMouseButton(1))
        {
            _xRotation -= Input.GetAxis("Mouse Y") * RotateSensitivity;
            _xRotation = Mathf.Clamp(_xRotation, -60f, 60f);
            CameraTransform.localEulerAngles = new Vector3(_xRotation, 0f, 0f);

            if (!isDomkrat)
                transform.Rotate(0, Input.GetAxis("Mouse X") * RotateSensitivity, 0);
        }
        MaybeZoom();
        MaybeCroach();
    }
    void MaybeZoom()
    {
        float sensitivity = 15;
        int minFov = 20;
        int maxFov = 60;
        var fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }
    void MaybeCroach()
    {
        GameObject selectedObject = PlayerRay.playerRay.GetSelected();
        if (Input.GetKey(KeyCode.LeftControl) && !isDaun)
        {
            isDaun = true;
            CameraTransform.position = new Vector3(CameraTransform.position.x, CameraTransform.position.y - 1f, CameraTransform.position.z);
            if (selectedObject != null && selectedObject.tag == "Ruchka")
            {
                PointerRuchka.transform.Translate(Vector3.forward * -0.8f);
            }
        }
        else if (!Input.GetKey(KeyCode.LeftControl) && isDaun)
        {
            isDaun = false;
            CameraTransform.position = new Vector3(CameraTransform.position.x, CameraTransform.position.y + 1f, CameraTransform.position.z);
            if (selectedObject != null && selectedObject.tag == "Ruchka")
            {
                PointerRuchka.transform.Translate(Vector3.forward * 0.8f);
            }
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
            transform.RotateAround(PointerDomkrat.transform.position, Vector3.up, angel);
            moving.Rotate(PointerDomkrat, angel);
        }
        Vector3 speedVector = transform.TransformVector(inputVector);
        speedVector *= Speed;
        Rigidbody.velocity = new Vector3(speedVector.x, Rigidbody.velocity.y, speedVector.z);
    }
}
