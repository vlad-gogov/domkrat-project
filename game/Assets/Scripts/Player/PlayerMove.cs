﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    float fps = 60;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Singleton.Instance.UIManager.pauseDialog.OnShow();
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
        // MovePlayer();

        // Вручную изменять разрешение на всякий случай
        // if (Input.GetKeyDown(KeyCode.PageUp))
        // {
        //     allowAutoResolution = false;
        //     ScaleResolution(resolutionScale + 0.1f);
        // }
        // else if (Input.GetKeyDown(KeyCode.PageDown))
        // {
        //     allowAutoResolution = false;
        //     ScaleResolution(resolutionScale - 0.1f);
        // }
    }

    int lowFps = 0;
    int decentFps = 0;

    void FPSStatus()
    {
        int low_fps = 24;
        int decent_fps = 55;

        if (fps < low_fps)
        {
            lowFps++;
            decentFps = 0;
        }
        else if (fps > decent_fps)
        {
            lowFps = 0;
            decentFps++;
        }
        else
        {
            lowFps = 0;
            decentFps = 0;
        }

        if (lowFps > low_fps * 2) // примерное через 2 секунды низкого FPS'а
        {
            lowFps = 0;
            decentFps = 0;
            GraphicsManager.LowerSettings();
        }
        if (decentFps > decent_fps * 30) // примерно через 30 секунд высокого FPS'а
        {
            lowFps = 0;
            decentFps = 0;
            GraphicsManager.IncreaseSettings();
        }
    }

    private void OnGUI()
    {
        fps = 1.0f / Time.deltaTime;
        string text = "FPS: " + ((int)fps).ToString();

        if (fps < 24)
        {
            text = $"<color=red>{text}</color>";
        }
        else if (fps < 40)
        {
            text = $"<color=orange>{text}</color>";
        }
        else
        {
            text = $"<color=green>{text}</color>";
        }
        text += $"\nResolution: {Screen.width}x{Screen.height}";
        GUI.Label(new Rect(0, 0, 300, 100), text);
        if (CrossScenesStorage.isAdaptiveResoulution)
        {
            FPSStatus();
        }
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
