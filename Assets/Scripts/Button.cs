using UnityEngine;

public class PhysicalButton : MonoBehaviour
{
    public Transform buttonTop; // Reference to the button's top object
    public float pressDepth = 0.014f; // Depth the button presses down
    public Door door; // Reference to the associated door
    public bool isPressed = false; // Tracks button state

    private Vector3 initialPosition;
    private float _debounceTime = 0.2f; // Minimum time between activations
    private float _lastActivationTime = 0f;

    void Start()
    {
        initialPosition = buttonTop.position; // Store initial position
    }

    void OnTriggerEnter(Collider other)
    {
        if (Time.time - _lastActivationTime < _debounceTime) return;

        if (other.CompareTag("Cube"))
        {
            buttonTop.position = initialPosition - new Vector3(0, pressDepth, 0); // Simulate pressing
            isPressed = true;
            _lastActivationTime = Time.time;
            door.CheckButtons(); // Notify the door
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (Time.time - _lastActivationTime < _debounceTime) return;

        if (other.CompareTag("Cube"))
        {
            buttonTop.position = initialPosition; // Reset position
            _lastActivationTime = Time.time;
            isPressed = false;
            door.CheckButtons(); // Notify the door
        }
    }
}
