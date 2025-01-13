using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("Which Color is Required?")]
    public CubeColor requiredColor;

    [Header("Visual Press Settings (Optional)")]
    public float pressedOffset = 0.1f;    // How far the platform moves down
    public float pressSpeed = 5f;         // Lerp speed

    // Tracks whether the correct color is on the platform
    public bool isCorrectlyPlaced = false;

    // Internal references
    private Vector3 _originalPosition;
    private List<ColorCube> _objectsOnPlatform = new List<ColorCube>();

    private float _debounceTime = 0.2f; // Minimum time between activations
    private float _lastActivationTime = 0f;

    private void Start()
    {
        _originalPosition = transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Time.time - _lastActivationTime < _debounceTime) return;

        // Check if the colliding object has a ColorCube script
        ColorCube colorCube = other.gameObject.GetComponent<ColorCube>();
        if (colorCube != null)
        {
            _lastActivationTime = Time.time;

            // Add this cube to our tracking list
            _objectsOnPlatform.Add(colorCube);
            UpdatePlatformStatus();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (Time.time - _lastActivationTime < _debounceTime) return;

        ColorCube colorCube = other.gameObject.GetComponent<ColorCube>();
        if (colorCube != null && _objectsOnPlatform.Contains(colorCube))
        {
            _lastActivationTime = Time.time;
            // Remove the cube from our list when it leaves
            _objectsOnPlatform.Remove(colorCube);
            UpdatePlatformStatus();
        }
    }

    private void UpdatePlatformStatus()
    {
        // If any object on the platform matches the required color, we are satisfied
        isCorrectlyPlaced = false;
        foreach (ColorCube cube in _objectsOnPlatform)
        {
            if (cube.color == requiredColor)
            {
                isCorrectlyPlaced = true;
                break;
            }
        }
    }

    private void Update()
    {
        // Optional: visually depress the platform if isCorrectlyPlaced
        Vector3 targetPos = _originalPosition;
        if (isCorrectlyPlaced)
        {
            targetPos.y -= pressedOffset;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, pressSpeed * Time.deltaTime);
    }
}
