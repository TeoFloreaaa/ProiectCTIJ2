using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    public string keyID;
    public KeyCode interactKey = KeyCode.E;
    public float detectionRadius = 2f; // Raza pentru detectarea player-ului
    public LayerMask playerLayer; // Layer-ul pentru player

    private bool playerNearby = false;

    private void Update()
    {
        // Verificăm dacă player-ul este în raza de detectare
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        playerNearby = colliders.Length > 0;

        if (playerNearby && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Jucătorul a apăsat E pentru a colecta cheia.");
            PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.CollectKey(keyID);
                Debug.Log("Cheia " + keyID + " a fost colectată!");
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
