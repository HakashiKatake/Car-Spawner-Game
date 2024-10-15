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

    void Start()
    {
        // Add listener to spawn button
        spawnButton.onClick.AddListener(SpawnVehicles);
    }

    void SpawnVehicles()
    {
        // Parse the input field and handle non-numeric input gracefully
        int vehicleCount = int.TryParse(vehicleInputField.text, out int count) ? count : 1;

        vehicles = new GameObject[vehicleCount];
        Camera[] followCameras = new Camera[vehicleCount];

        // Spawn vehicles and assign follow cameras
        for (int i = 0; i < vehicleCount; i++)
        {
            Vector3 spawnPos = spawnArea.position + new Vector3(i * 5, 0, 0); // Spread out vehicles
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
