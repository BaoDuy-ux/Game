using UnityEngine;

public class Init : MonoBehaviour
{
    private void Awake()
    {
        FixCameraPosition();
    }

    void Start()
    {
        GameObject selectedCharacter = CharacterSelect.selectedCharacter;
        GameObject playerObject = Instantiate(selectedCharacter, new Vector3(10f, 5f, 0f), Quaternion.identity);
        playerObject.name = "Player";

        if (!playerObject.CompareTag("Player"))
        {
            playerObject.tag = "Player";
        }

        SetupCamera(playerObject.transform);
    }

    void SetupCamera(Transform playerTransform)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindAnyObjectByType<Camera>();
        }

        if (mainCamera != null)
        {
            Vector3 camPos = new Vector3(playerTransform.position.x, playerTransform.position.y, -20f);
            mainCamera.transform.position = camPos;

            CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
            if (cameraFollow == null)
            {
                cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
            }

            cameraFollow.SetTarget(playerTransform);
        }
    }

    void FixCameraPosition()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera= FindAnyObjectByType<Camera>();
        }

        if (mainCamera)
        {
            Vector3 camPos = mainCamera.transform.position;
            camPos.z = -20;
            mainCamera.transform.position = camPos;
        }
    }

}
