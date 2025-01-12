using UnityEngine;
using System.Collections.Generic;

public class ProceduralDungeon : MonoBehaviour
{
    public GameObject startRoomPrefab;
    public GameObject hallPrefab;
    public GameObject largeHallPrefab;
    public GameObject curvedHallLeftPrefab;
    public GameObject curvedHallRightPrefab;
    public GameObject endRoomPrefab;
    public GameObject roomType1;
    public GameObject roomType2;
    public GameObject roomType3;
    public GameObject roomType4;
    public GameObject roomType5;
    public GameObject roomType6;
    public GameObject roomType7;
    public GameObject LargecurvedHallLeftPrefab;
    public GameObject LargecurvedHallRightPrefab;
    public GameObject KeyRoom;

    public GameObject lockedWallPrefab; // Prefab-ul pentru zidul blocant
    private GameObject lastLockedWall; // Referință la ultimul zid generat

    public GameObject player; // Referință la jucător
    public int maxVisibleRooms = 2; // Număr maxim de camere vizibile simultan

    private Queue<GameObject> activeRooms = new Queue<GameObject>(); // Coada camerelor active
    private Transform lastExitPoint; // Punctul de ieșire al ultimei camere

    public GameObject playerPrefab;
    private int roomsGenerated = 0; // Număr de camere generate
    public int maxRooms = 10;

    void Start()
    {
        if (player == null && playerPrefab != null)
        {
            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Player-ul a fost generat automat.");
        }

        if (player == null)
        {
            Debug.LogError("Player-ul nu a fost găsit! Asigură-te că ai setat player-ul în Inspector.");
            return;
        }

        // Generăm camera de start
        GameObject startRoom = Instantiate(startRoomPrefab, Vector3.zero, Quaternion.identity);
        activeRooms.Enqueue(startRoom);

        // Setăm punctul de ieșire al primei camere
        lastExitPoint = GetExitPoint(startRoom);

        // Generăm prima cameră adiacentă
        GenerateNextRoom();
    }

    void Update()
    {
        // Verificăm dacă jucătorul a trecut de punctul de ieșire
        if (Vector3.Distance(player.transform.position, lastExitPoint.position) < 5f)
        {
            GenerateNextRoom();
        }
    }

    void GenerateNextRoom()
    {
        // Verificăm dacă am ajuns la numărul maxim de camere
        if (roomsGenerated >= maxRooms - 1)
        {
            // Generează camera finală
            GameObject finalRoom = Instantiate(endRoomPrefab);
            AlignRoom(finalRoom, lastExitPoint);
            activeRooms.Enqueue(finalRoom);

            Debug.Log("FinalRoom a fost generată!");

            // Dezactivează generarea altor camere
            enabled = false; // Oprește scriptul după generarea FinalRoom
            return;
        }

        // Obținem o cameră aleatorie
        GameObject nextRoomPrefab = GetRandomRoomPrefab();
        GameObject nextRoom = Instantiate(nextRoomPrefab);

        // Aliniem camera nouă
        AlignRoom(nextRoom, lastExitPoint);

        // Actualizăm punctul de ieșire
        lastExitPoint = GetExitPoint(nextRoom);

        // Adăugăm camera la coada camerelor active
        activeRooms.Enqueue(nextRoom);

        // Adăugăm zidul la camera anterioară
        if (activeRooms.Count > 1) // Dacă există cel puțin o cameră anterioară
        {
            GameObject currentRoom = nextRoom; // Camera generată acum
            AddLockedWallToCurrentRoom(currentRoom); // Transmitem camera generată funcției
        }

        // Ștergem camerele vechi dacă sunt prea multe
        if (activeRooms.Count > maxVisibleRooms)
        {
            GameObject oldRoom = activeRooms.Dequeue();
            Destroy(oldRoom);
        }

        // Incrementăm contorul de camere
        roomsGenerated++;
    }


    void AddLockedWallToCurrentRoom(GameObject currentRoom)
    {
        // Adaugă zidul blocant la EntryPoint-ul camerei actuale
        Transform entryPoint = currentRoom.transform.Find("EntryPoint");
        if (entryPoint != null)
        {
            // Șterge zidul blocant anterior, dacă există
            if (lastLockedWall != null)
            {
                Destroy(lastLockedWall);
            }

            // Creează un nou zid blocant
            lastLockedWall = Instantiate(lockedWallPrefab, entryPoint.position, entryPoint.rotation);
            Debug.Log("Peretele blocant a fost adăugat la EntryPoint-ul camerei actuale.");
        }
        else
        {
            Debug.LogWarning("EntryPoint-ul nu a fost găsit în camera actuală.");
        }
    }


    GameObject GetRandomRoomPrefab()
    {
        GameObject nextPrefab = null;
        bool visitedRoom = false;
        do
        {
            int randomIndex = Random.Range(0, 14);
            if (randomIndex == 0) nextPrefab = hallPrefab;
            else if (randomIndex == 1) nextPrefab = curvedHallLeftPrefab;
            else if (randomIndex == 2) nextPrefab = curvedHallRightPrefab;
            else if (randomIndex == 3) nextPrefab = largeHallPrefab;
            else if (randomIndex == 4) nextPrefab = roomType1;
            else if (randomIndex == 5) nextPrefab = roomType2;
            else if (randomIndex == 6) nextPrefab = roomType3;
            else if (randomIndex == 7) nextPrefab = roomType4;
            else if (randomIndex == 8) nextPrefab = LargecurvedHallLeftPrefab;
            else if (randomIndex == 9) nextPrefab = LargecurvedHallRightPrefab;
            else if (randomIndex == 10) nextPrefab = roomType5;
            else if (randomIndex == 11) nextPrefab = roomType6;
            else if (randomIndex == 12) nextPrefab = roomType7;
            else if (randomIndex == 13 && visitedRoom == false)
            {
                nextPrefab = KeyRoom;
                if (visitedRoom == true) nextPrefab = null;
                visitedRoom = true;
            }
        } while (nextPrefab == null);

        return nextPrefab;
    }

    Transform GetExitPoint(GameObject room)
    {
        // Găsește punctul de ieșire al unei camere
        return room.transform.Find("ExitPoint");
    }

    void AlignRoom(GameObject room, Transform previousExitPoint)
    {
        // Aliniază camera nouă cu punctul de ieșire al celei anterioare
        Transform entryPoint = room.transform.Find("EntryPoint");

        if (entryPoint == null || previousExitPoint == null)
        {
            Debug.LogError("EntryPoint sau ExitPoint lipsește!");
            return;
        }

        // Calculează poziția și rotirea pentru aliniere
        Vector3 offset = previousExitPoint.position - entryPoint.position;
        room.transform.position += offset;

        float angleDifference = previousExitPoint.eulerAngles.y - entryPoint.eulerAngles.y;
        room.transform.RotateAround(entryPoint.position, Vector3.up, angleDifference);
    }
}
