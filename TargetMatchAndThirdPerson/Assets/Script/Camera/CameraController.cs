using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 targetRotation;

    Vector3 cameraDir;
    float camDis;

    private void Start()
    {
        cameraDir = GameManager.Instance.cameraTransform.localPosition.normalized;
        camDis = cameraDir.y;
    }

    void Update()
    {
        CameraMove();
        CameraRotation();
        AimSetUp();
        CameraCollision(GameManager.Instance.cameraTransform);
    }

    void CameraMove()
    {
        Vector3 desiredPos = PlayerManager.Instance.cameraPivot.position;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, PlayerManager.Instance.cameraSmoothMove * Time.deltaTime);
        transform.position = smoothPos;
    }

    void CameraRotation()
    {
        Vector2 viewInput = PlayerManager.Instance.viewInput;

        float mouseYsen = PlayerManager.Instance.senY;
        float mouseXsen = PlayerManager.Instance.senX;

        targetRotation.x += (PlayerManager.Instance.invertY ? -(viewInput.y * mouseYsen) : (viewInput.y * mouseYsen)) * Time.deltaTime;
        targetRotation.y += (PlayerManager.Instance.invertX ? -(viewInput.x * mouseXsen) : (viewInput.x * mouseXsen)) * Time.deltaTime;

        targetRotation.x = Mathf.Clamp(targetRotation.x, PlayerManager.Instance.YClampMin, PlayerManager.Instance.YClampMax);

        transform.rotation = Quaternion.Euler(targetRotation);

    }

    void CameraCollision(Transform cam)
    {
        if (!PlayerManager.Instance.isAim)
        {
            Vector3 disiredCamPos = transform.TransformPoint(cameraDir * PlayerManager.Instance.camDisMax);
            RaycastHit hit;
            if (Physics.Linecast(transform.position, disiredCamPos, out hit))
            {
                camDis = Mathf.Clamp(hit.distance, PlayerManager.Instance.camDisMin, PlayerManager.Instance.camDisMax);

            }
            else
            {
                camDis = PlayerManager.Instance.camDisMax;
            }
            Vector3 pos = cameraDir * camDis;
            cam.localPosition = Vector3.Lerp(cam.localPosition, pos, PlayerManager.Instance.rotationDamp * Time.deltaTime);
        }
    }

    void AimSetUp()
    {
        if (PlayerManager.Instance.isAim)
        {
            PlayerManager.Instance.isRun = false;
            Quaternion currentRotation = PlayerManager.Instance.transform.rotation;

            Vector3 newRot = currentRotation.eulerAngles;
            newRot.y = targetRotation.y;

            currentRotation = Quaternion.Lerp(currentRotation, Quaternion.Euler(newRot), PlayerManager.Instance.rotationDamp * Time.deltaTime);

            PlayerManager.Instance.transform.rotation = currentRotation;
            Vector3 AimPos = new Vector3(1f, 0, -2f);
            GameManager.Instance.cameraTransform.localPosition = Vector3.Lerp(GameManager.Instance.cameraTransform.localPosition, AimPos, PlayerManager.Instance.rotationDamp * Time.deltaTime);
        }

    }

}
