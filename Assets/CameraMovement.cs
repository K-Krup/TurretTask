using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float cameraSpeed;

    [SerializeField, Range(1, 100)]
    float sensitivity;

    void Update()
    {
        float xMouseInput = Input.GetAxis("Horizontal");
        float yMouseInput = Input.GetAxis("Vertical");

        if(Mathf.Abs(xMouseInput) > 1 / sensitivity || Mathf.Abs(yMouseInput) > 1 / sensitivity)
        {
            transform.position += new Vector3(xMouseInput*cameraSpeed*Time.deltaTime, 0, yMouseInput * cameraSpeed * Time.deltaTime);
		}

    }
}
