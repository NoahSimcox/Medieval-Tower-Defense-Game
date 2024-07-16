using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private float sensX = 100;
    [SerializeField] private float sensY = 100;
    private float _translateX;
    private float _translateY;
    void Start()
    {
        _transform = transform;
    }

    
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            _translateX += mouseX;
            _translateY += mouseY;

            _transform.position = new Vector3(-_translateX, -_translateY, -10);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView<=125)
                Camera.main.fieldOfView +=2;
            if (Camera.main.orthographicSize<=20)
                Camera.main.orthographicSize +=0.5f;
 
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView>2)
                Camera.main.fieldOfView -=2;
            if (Camera.main.orthographicSize>=1)
                Camera.main.orthographicSize -= 0.5f;
        }
    }
}
