using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float openAngle = 90f; // Unghiul la care se deschide u?a
    public float openSpeed = 2f; // Viteza deschiderii
    public KeyCode interactKey = KeyCode.E; // Tasta pentru interac?iune

    private bool isOpen = false; // U?a e deschis??
    private bool playerNearby = false; // Juc?torul este aproape?

    private Quaternion closedRotation; // Pozi?ia ini?ial? a u?ii (închis?)
    private Quaternion openRotation; // Pozi?ia deschis? a u?ii

    void Start()
    {
        // Salveaz? rota?ia curent? a pivotului ca fiind "închis?"
        closedRotation = transform.rotation;

        // Calculeaz? rota?ia pentru pozi?ia "deschis?"
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        // Dac? juc?torul apas? tasta de interac?iune ?i este aproape
        if (playerNearby && Input.GetKeyDown(interactKey))
        {
            isOpen = !isOpen; // Comut? între deschis/închis
        }

        // Anima?ia de deschidere/închidere
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
            Debug.Log("Juc?torul este aproape de u??!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            Debug.Log("Juc?torul a p?r?sit zona u?ii.");
        }
    }

}
