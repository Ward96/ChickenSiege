using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform targetForward; //where the camera moves forward
    public Transform targetBackward; //where the camera moves backward
    public float smoothSpeed = 0.125f; // camera speed
    public bool isMoving = false;
    public bool isMovingForward = false; //boolean to track the direction of movement
    private MainMenuLevelSelect MainMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        MainMenuUI = GameObject.Find("MainMenuManager").GetComponent<MainMenuLevelSelect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (isMovingForward)
            {
                MoveCamera(targetForward);
            }
            else
            {
                MoveCamera(targetBackward);
            }
        }
    }

    void MoveCamera(Transform target)
    {
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, target.position, smoothSpeed * Time.deltaTime);
        cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, target.rotation, smoothSpeed * Time.deltaTime);

        //check if the camera is close enough to the target position
        if (Vector3.Distance(cameraTransform.position, target.position) < 0.0005f)
        {
            //ensure the camera reaches the exact target position
            cameraTransform.position = target.position;
            cameraTransform.rotation = target.rotation;
            isMoving = false;
            if (isMovingForward)
            {
                MainMenuUI.ShowLS();
            }
            else
            {
                MainMenuUI.ShowMM();
            }
        }
    }

    public void SetMoveTrue()
    {
        isMoving = true;
        isMovingForward = !isMovingForward; //toggle the direction
    }
}
