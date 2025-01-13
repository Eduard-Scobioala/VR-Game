using UnityEngine;

public class Door : MonoBehaviour
{
    public PhysicalButton[] buttons; // Array of buttons required to open the door
    public float openDistance = 5f; // Distance to slide on Z-axis
    public float openSpeed = 2f; // Speed of door movement

    private Vector3 initialPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    void Start()
    {
        initialPosition = transform.position;
        openPosition = initialPosition + new Vector3(0, 0, openDistance);
    }

    public void CheckButtons()
    {
        foreach (var button in buttons)
        {
            if (!button.isPressed) // If any button is not pressed
            {
                CloseDoor();
                return;
            }
        }
        OpenDoor(); // All buttons are pressed
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            StopAllCoroutines();
            SoundManager.Instance.PlaySFX("open_door");
            StartCoroutine(SlideDoor(openPosition));
            isOpen = true;
        }
    }

    void CloseDoor()
    {
        if (isOpen)
        {
            StopAllCoroutines();
            SoundManager.Instance.PlaySFX("open_door");
            StartCoroutine(SlideDoor(initialPosition));
            isOpen = false;
        }
    }

    System.Collections.IEnumerator SlideDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, openSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
