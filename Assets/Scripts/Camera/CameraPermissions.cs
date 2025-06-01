using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class CameraPermissions : MonoBehaviour
{
    private bool permissionResponded = false;
    private PermissionCallbacks permissionCallbacks;

    void Start()
    {
        StartCoroutine(WaitForCameraPermission());
    }

    private IEnumerator WaitForCameraPermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            // Setup permission callbacks
            permissionCallbacks = new PermissionCallbacks();
            permissionCallbacks.PermissionGranted += OnPermissionGranted;
            permissionCallbacks.PermissionDenied += OnPermissionDenied;
            permissionCallbacks.PermissionDeniedAndDontAskAgain += OnPermissionDenied;

            Permission.RequestUserPermission(Permission.Camera, permissionCallbacks);

            float maxWait = 30f;
            float elapsed = 0f;

            while (elapsed < maxWait && !permissionResponded)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        if (permissionCallbacks != null)
        {
            permissionCallbacks.PermissionGranted -= OnPermissionGranted;
            permissionCallbacks.PermissionDenied -= OnPermissionDenied;
            permissionCallbacks.PermissionDeniedAndDontAskAgain -= OnPermissionDenied;
        }


        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("X 1Camera");
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("NoCameraPrepp");
        }
    }

    private void OnPermissionGranted(string permissionName)
    {
        permissionResponded = true;
    }

    private void OnPermissionDenied(string permissionName)
    {
        permissionResponded = true;
    }
}
