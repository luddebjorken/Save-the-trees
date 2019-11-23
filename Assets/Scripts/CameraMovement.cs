using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float CameraSpeed;
    private Camera camera;
    Vector2 lowerLimit;
    Vector2 upperLimit;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        Vector2 cameraSize = new Vector2(camera.orthographicSize*camera.aspect, camera.orthographicSize);
        Vector2 cameraWorldSize = new Vector2(1,1)*World.singleton.Size.magnitude;
        cameraWorldSize.x /= 2;
        cameraWorldSize.y *= Mathf.Sin(transform.eulerAngles.x*Mathf.PI/180);
        lowerLimit = new Vector2(cameraSize.x - cameraWorldSize.x,cameraSize.y+1);
        upperLimit = cameraWorldSize - cameraSize;
        upperLimit.y += 2.5f;
        camera.transform.localPosition = Vector3.Lerp(lowerLimit, upperLimit, 0.5f);
        //upperLimit.y -= cameraSize.y/Mathf.Tan(transform.eulerAngles.x*Mathf.PI/180);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(Mathf.Clamp(Input.GetAxis("Horizontal") + transform.localPosition.x, lowerLimit.x, upperLimit.x) , Mathf.Clamp(Input.GetAxis("Vertical") * CameraSpeed + transform.localPosition.y, lowerLimit.y, upperLimit.y));
    }
}
