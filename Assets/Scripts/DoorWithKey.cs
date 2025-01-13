using UnityEngine;

public class DoorWithKeyController : MonoBehaviour
{
    [Header("Key Settings")]
    [Tooltip("Tag on the key object")]
    public string keyTag = "Key";

    [Header("Door Movement")]
    [Tooltip("How far (and in which direction) the door will slide when opened")]
    public Vector3 openOffset = new Vector3(-3f, 0f, 0f);
    [Tooltip("Speed at which the door slides open/close")]
    public float moveSpeed = 3f;

    public SceneTransitionManager sceneTransitionManager;

    // Internal state
    private bool isOpen = false;
    private bool loadingScene = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        // Record the initial (closed) position of the door
        closedPosition = transform.position;
        // Calculate where the door will be when fully open
        openPosition = closedPosition + openOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the object entering the trigger has the "Key" tag, open the door
        if (other.CompareTag(keyTag))
        {
            isOpen = true;
        }
    }

    private void Update()
    {
        // Lerp the door between closed and open positions
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (isOpen)
        {
            if (!loadingScene)
            {
                SoundManager.Instance.PlaySFX("open_door");
                SoundManager.Instance.PlaySFX("congratulations");

                loadingScene = true;
                sceneTransitionManager.GoToSceneAsync(0);
            }
        }
    }
}
