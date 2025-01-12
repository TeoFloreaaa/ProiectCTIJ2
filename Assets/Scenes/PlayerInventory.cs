using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<string> keys = new List<string>(); // Lista cheilor colectate

    public void CollectKey(string keyID)
    {
        if (!keys.Contains(keyID))
        {
            keys.Add(keyID);
            Debug.Log("Cheia " + keyID + " a fost colectată!");
        }
    }

    public bool HasKey(string keyID)
    {
        return keys.Contains(keyID);
    }
}
