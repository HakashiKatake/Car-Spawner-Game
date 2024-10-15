using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Camera topViewCamera;             // Overview camera
    public Canvas cameraSelectionUI;         // UI Canvas to hold dynamic buttons

    private Camera[] followCameras;  // Array to store follow cameras

    // Initialize the cameras and generate the camera selection buttons
    public void InitializeCameras(Camera[] cameras)
    {
        followCameras = cameras;

        // Generate buttons dynamically
        GenerateCameraButtons();
        ActivateCamera(topViewCamera); // Default to overview camera
    }

    // Generate buttons for each camera option
    private void GenerateCameraButtons()
    {
        // Clear existing buttons
        foreach (Transform child in cameraSelectionUI.transform)
            Destroy(child.gameObject);

        // Button for overview camera
        CreateButton("Overview", () => ActivateCamera(topViewCamera));

        // Buttons for follow cameras
        for (int i = 0; i < followCameras.Length; i++)
        {
            int index = i;  // Store index to use inside lambda function
            CreateButton($"Vehicle {index + 1}", () => ActivateCamera(followCameras[index]));
        }
    }

    // Helper function to create a button dynamically
    private void CreateButton(string text, UnityEngine.Events.UnityAction onClick)
    {
        GameObject buttonObj = new GameObject(text);
        buttonObj.AddComponent<RectTransform>();  // Ensure it has a RectTransform

        Button button = buttonObj.AddComponent<Button>();
        Text buttonText = buttonObj.AddComponent<Text>();
        buttonText.text = text;
        buttonText.alignment = TextAnchor.MiddleCenter;

        // Set button's visual and interaction properties
        button.transform.SetParent(cameraSelectionUI.transform, false);
        button.onClick.AddListener(onClick);
    }

    // Activate the selected camera and disable all others
    private void ActivateCamera(Camera activeCamera)
    {
        // Disable all follow cameras
        foreach (Camera cam in followCameras)
            cam.enabled = false;

        // Disable overview camera
        topViewCamera.enabled = false;

        // Enable the selected camera
        activeCamera.enabled = true;
    }
}

