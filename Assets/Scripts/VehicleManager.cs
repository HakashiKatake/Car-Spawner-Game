using UnityEngine;
using UnityEngine.UI;
using TMPro;  // TMP support

public class VehicleManager : MonoBehaviour
{
    public GameObject vehiclePrefab;         // Vehicle prefab to spawn
    public Transform spawnArea;              // Reference to spawn position
    public TMP_InputField vehicleInputField; // TMP InputField to enter vehicle count
    public Button spawnButton;               // Button to trigger spawning

    public CameraController cameraController; // Reference to CameraController script

    private GameObject[] vehicles;  // Store spawned vehicles
    private const int MaxVehicles = 10;  // Limit to 10 vehicles

    void Start()
    {
        // Add listener to spawn button
        spawnButton.onClick.AddListener(SpawnVehicles);

        // Add listener for input validation (for non-numeric input)
        vehicleInputField.onValueChanged.AddListener(ValidateInput);
    }

    // Validates input to ensure only integers are entered
    private void ValidateInput(string input)
    {
        if (!int.TryParse(input, out _))
        {
            vehicleInputField.text = "";  // Clear the input field if invalid input
            vehicleInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Enter a valid number!";
        }
    }

    void SpawnVehicles()
    {
        // Parse the input field and ensure only valid input proceeds
        if (!int.TryParse(vehicleInputField.text, out int vehicleCount))
        {
            vehicleInputField.text = "Enter a valid integer!";
            return;
        }

        // Check if the vehicle count exceeds the maximum limit
        if (vehicleCount > MaxVehicles)
        {
            vehicleInputField.text = MaxVehicles.ToString();
            return;
        }

        // Initialize the arrays
        vehicles = new GameObject[vehicleCount];
        Camera[] followCameras = new Camera[vehicleCount];

        // Spawn vehicles and assign follow cameras
        for (int i = 0; i < vehicleCount; i++)
        {
            Vector3 spawnPos = spawnArea.position + new Vector3(i * 5, 0, 0);  // Spread out vehicles
            vehicles[i] = Instantiate(vehiclePrefab, spawnPos, Quaternion.identity);

            // Create a follow camera for each vehicle
            GameObject camObj = new GameObject($"FollowCamera_{i}");
            Camera followCamera = camObj.AddComponent<Camera>();
            camObj.transform.SetParent(vehicles[i].transform);  // Attach to vehicle
            camObj.transform.localPosition = new Vector3(0, 5, -10);  // Adjust position
            followCamera.enabled = false;  // Disable initially

            followCameras[i] = followCamera;
        }

        // Pass the cameras to the CameraController to generate buttons
        cameraController.InitializeCameras(followCameras);
    }
}

