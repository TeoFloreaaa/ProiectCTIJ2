using UnityEngine;

public class DoorInteractionWithKey : MonoBehaviour
{
    public float openAngle = 90f; // Unghiul la care se deschide ușa
    public float openSpeed = 2f; // Viteza deschiderii
    public KeyCode interactKey = KeyCode.E; // Tasta pentru interacțiune
    public string requiredKeyID; // ID-ul cheii necesare pentru a deschide ușa
    public AudioClip openDoorSound; // Sunet pentru deschiderea ușii
    public AudioClip lockedDoorSound; // Sunet pentru ușa încuiată

    private bool isOpen = false; // Ușa e deschisă?
    private bool playerNearby = false; // Jucătorul este aproape?
    private Quaternion closedRotation; // Poziția inițială a ușii (închisă)
    private Quaternion openRotation; // Poziția deschisă a ușii
    private AudioSource audioSource;

    void Start()
    {
        // Salvează rotația curentă a pivotului ca fiind "închisă"
        closedRotation = transform.rotation;

        // Calculează rotația pentru poziția "deschisă"
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        // Adaugă sau obține componenta AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Previne redarea automată a sunetelor
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Dacă jucătorul apasă tasta de interacțiune și este aproape
        if (playerNearby && Input.GetKeyDown(interactKey))
        {
            PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();

            if (playerInventory != null && playerInventory.HasKey(requiredKeyID))
            {
                // Deschide ușa dacă cheia este prezentă în inventar
                if (!isOpen)
                {
                    isOpen = true;
                    PlaySound(openDoorSound);
                }
            }
            else
            {
                // Redă sunetul de ușă încuiată
                PlaySound(lockedDoorSound);
                Debug.Log("Ai nevoie de cheia " + requiredKeyID + " pentru a deschide această ușă!");
            }
        }

        // Animația de deschidere/închidere
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRotation, Time.deltaTime * openSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play(); // Redă sunetul
        }
    }
}
