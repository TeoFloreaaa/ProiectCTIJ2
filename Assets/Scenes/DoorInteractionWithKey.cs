using UnityEngine;

public class DoorInteractionWithKey : MonoBehaviour
{
    public string requiredKeyID; // ID-ul cheii necesare
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public KeyCode interactKey = KeyCode.E;

    private bool isOpen = false;
    private bool isPlayerNear = false;
    private Transform doorTransform;

    void Start()
    {
        doorTransform = transform;
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(interactKey))
        {
            PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
            if (playerInventory != null && playerInventory.HasKey(requiredKeyID))
            {
                // Deschide u?a
                if (!isOpen)
                {
                    StartCoroutine(OpenDoor());
                }
            }
            else
            {
                Debug.Log("Ai nevoie de cheia " + requiredKeyID + " pentru a deschide aceast? u??!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("E?ti aproape de u??.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    private System.Collections.IEnumerator OpenDoor()
    {
        isOpen = true;

        Quaternion startRotation = doorTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(doorTransform.eulerAngles + new Vector3(0, openAngle, 0));

        float elapsed = 0f;
        while (elapsed < openSpeed)
        {
            doorTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / openSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        doorTransform.rotation = targetRotation;
    }
}
