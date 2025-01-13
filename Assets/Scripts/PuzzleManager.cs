using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Platforms to Monitor")]
    public PlatformController platformWhite;
    public PlatformController platformGreen;
    public PlatformController platformBrown;

    [Header("Object to Move Up")]
    public Transform objectToMove;  // assign the pillar/gate
    public Vector3 moveOffset = new Vector3(0, 3, 0);
    public float moveSpeed = 2f;

    // Internal references
    private Vector3 _originalPos;
    private bool _hasMoved = false;

    private void Start()
    {
        if (objectToMove != null)
        {
            _originalPos = objectToMove.position;
        }
    }

    private void Update()
    {
        if (!_hasMoved && AllPlatformsSatisfied())
        {
            SoundManager.Instance.PlaySFX("glass_box");
            // Start moving the object up
            _hasMoved = true;
        }

        if (_hasMoved && objectToMove != null)
        {
            // Lerp from original position to original + moveOffset
            Vector3 targetPos = _originalPos + moveOffset;
            objectToMove.position = Vector3.Lerp(
                objectToMove.position,
                targetPos,
                Time.deltaTime * moveSpeed
            );
        }
    }

    private bool AllPlatformsSatisfied()
    {
        return platformWhite.isCorrectlyPlaced &&
               platformGreen.isCorrectlyPlaced &&
               platformBrown.isCorrectlyPlaced;
    }
}
