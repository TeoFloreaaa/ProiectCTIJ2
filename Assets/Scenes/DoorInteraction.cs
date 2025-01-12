using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float openAngle = 90f; // Unghiul la care se deschide ușa
    public float openSpeed = 2f; // Viteza deschiderii
    public KeyCode interactKey = KeyCode.E; // Tasta pentru interacțiune
    public AudioClip openDoorSound; // Sunetul pentru deschiderea ușii
    public AudioClip closeDoorSound; // Sunetul pentru închiderea ușii

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
            isOpen = !isOpen; // Comută între deschis/închis

            // Redă sunetul corespunzător
            if (isOpen)
            {
                PlaySound(openDoorSound);
            }
            else
            {
                PlaySound(closeDoorSound);
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
