using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraController : MonoBehaviour
{
    public Camera topViewCamera;            // Overview camera
    public Canvas cameraSelectionUI;        // Canvas for buttons
    public GameObject buttonPrefab;         // Button prefab (with TMP support)

    private Camera[] followCameras;         // Store follow cameras

    // Initialize cameras and set up UI buttons
    public void InitializeCameras(Camera[] cameras)
    {
        followCameras = cameras;
        GenerateCameraButtons();  // Create buttons for each camera
        ActivateCamera(topViewCamera);  // Default to top view camera
    }

    // Generate buttons dynamically for each camera and top view
    private void GenerateCameraButtons()
    {
        // Clear any existing buttons
        Transform buttonContainer = cameraSelectionUI.transform.Find("ButtonContainer");
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Create a button for the top view camera
        CreateButton("Top View", () => ActivateCamera(topViewCamera), buttonContainer);

        // Create buttons for each follow camera
        for (int i = 0; i < followCameras.Length; i++)
        {
            int index = i;  // Capture index for lambda function
            CreateButton($"Vehicle {index + 1}", () => ActivateCamera(followCameras[index]), buttonContainer);
        }
    }

    // Helper function to create a button dynamically
    private void CreateButton(string label, UnityEngine.Events.UnityAction onClick, Transform parent)
    {
        // Instantiate the button from the prefab
        GameObject buttonObj = Instantiate(buttonPrefab, parent);

        // Set up the Button and TMP_Text components
        Button button = buttonObj.GetComponent<Button>();
        TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();

        if (button != null && buttonText != null)
        {
            buttonText.text = label;  // Set button text
            button.onClick.AddListener(onClick);  // Assign click event
        }
        else
        {
            Debug.LogError("Button prefab is missing Button or TMP_Text component.");
        }
    }

    // Activate the selected camera and disable others
    private void ActivateCamera(Camera activeCamera)
    {
        // Disable all follow cameras
        foreach (Camera cam in followCameras)
        {
            cam.enabled = false;
        }

        // Disable the top view camera
        topViewCamera.enabled = false;

        // Enable the selected camera
        activeCamera.enabled = true;
    }
}





