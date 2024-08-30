using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private float sensX = 100;
    [SerializeField] private float sensY = 100;
    private float _translateX;
    private float _translateY;
    private Vector3 _center;
    private readonly float _defaultCameraSize = 8;
    private Camera _mainCamera;
    
    void Start()
    {
        _transform = transform;
         _center = GameObject.FindWithTag("Ground Tilemap").transform.position;
        _transform.position = new Vector3(_center.x, _center.y, _transform.position.z);
        _translateX = _center.x;
        _translateY = _center.y;
        
        _mainCamera = Camera.main;
    }

    
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX * (_mainCamera.orthographicSize/_defaultCameraSize);
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY * (_mainCamera.orthographicSize/_defaultCameraSize);

            _translateX -= mouseX;
            _translateY -= mouseY;

            _transform.position = new Vector3(_translateX, _translateY, -10);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (_mainCamera.fieldOfView<=125)
                _mainCamera.fieldOfView +=2;
            if (_mainCamera.orthographicSize<=20)
                _mainCamera.orthographicSize +=0.5f;
 
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (_mainCamera.fieldOfView>2)
                _mainCamera.fieldOfView -=2;
            if (_mainCamera.orthographicSize>=1)
                _mainCamera.orthographicSize -= 0.5f;
        }
    }
}
