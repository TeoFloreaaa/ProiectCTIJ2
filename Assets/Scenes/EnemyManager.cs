using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemies; // Array cu toți inamicii din cameră
    public DoorInteraction door; // Referință către scriptul ușii

    void Update()
    {
        if (AllEnemiesDefeated())
        {
            // Permite deschiderea ușii
            door.canOpen = true;
        }
    }

    bool AllEnemiesDefeated()
    {
        if(enemies.Length == 0)
            return true;
        // Verifică dacă toți inamicii au fost eliminați
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                return false; // Dacă un inamic încă există, returnează false
            }
        }
        return true;
    }
}
