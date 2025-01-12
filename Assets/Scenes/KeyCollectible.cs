using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    public string keyID = "Key1"; // Identificator pentru cheia specific?

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Juc?torul a intrat în zona");

        if (other.CompareTag("Player")) // Verific? dac? juc?torul intr? în trigger
        {
            Debug.Log("Juc?torul a intrat în zona cheii!");

            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.CollectKey(keyID);
                Debug.Log("Cheia " + keyID + " colectat?!");

                // Distruge cheia dup? colectare
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("PlayerInventory nu este ata?at juc?torului!");
            }
        }
    }
}
