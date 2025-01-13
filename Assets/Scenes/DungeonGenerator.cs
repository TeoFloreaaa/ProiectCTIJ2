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
    public GameObject KeyRoom1;

    public GameObject lockedWallPrefab; 
    private GameObject lastLockedWall; 

    public GameObject player; 
    public int maxVisibleRooms = 2; 

    private Queue<GameObject> activeRooms = new Queue<GameObject>(); 
    private Transform lastExitPoint; 

    public GameObject playerPrefab;
    private int roomsGenerated = 0; 
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

        
        GameObject startRoom = Instantiate(startRoomPrefab, Vector3.zero, Quaternion.identity);
        activeRooms.Enqueue(startRoom);

        
        lastExitPoint = GetExitPoint(startRoom);

        
        GenerateNextRoom();
    }

    void Update()
    {
        
        if (Vector3.Distance(player.transform.position, lastExitPoint.position) < 5f)
        {
            GenerateNextRoom();
        }
    }

    void GenerateNextRoom()
    {
        
        if (roomsGenerated >= maxRooms - 1)
        {
            
            GameObject finalRoom = Instantiate(endRoomPrefab);
            AlignRoom(finalRoom, lastExitPoint);
            activeRooms.Enqueue(finalRoom);

            Debug.Log("FinalRoom a fost generată!");

            
            enabled = false; 
            return;
        }

        
        GameObject nextRoomPrefab = GetRandomRoomPrefab();
        GameObject nextRoom = Instantiate(nextRoomPrefab);

        
        AlignRoom(nextRoom, lastExitPoint);

        
        lastExitPoint = GetExitPoint(nextRoom);

        
        activeRooms.Enqueue(nextRoom);

        
        if (activeRooms.Count > 1) 
        {
            GameObject currentRoom = nextRoom; 
            AddLockedWallToCurrentRoom(currentRoom); 
        }

        
        if (activeRooms.Count > maxVisibleRooms)
        {
            GameObject oldRoom = activeRooms.Dequeue();
            Destroy(oldRoom);
        }

        
        roomsGenerated++;
    }


    void AddLockedWallToCurrentRoom(GameObject currentRoom)
    {
        
        Transform entryPoint = currentRoom.transform.Find("EntryPoint");
        if (entryPoint != null)
        {
           
            if (lastLockedWall != null)
            {
                Destroy(lastLockedWall);
            }

            
            lastLockedWall = Instantiate(lockedWallPrefab, entryPoint.position, entryPoint.rotation);
            //Debug.Log("Peretele blocant a fost adăugat la EntryPoint-ul camerei actuale.");
        }
        else
        {
            Debug.LogWarning("EntryPoint-ul nu a fost găsit în camera actuală.");
        }
    }


    GameObject GetRandomRoomPrefab()
    {
        GameObject nextPrefab = null;
        do
        {
            int randomIndex = Random.Range(0, 15);
            randomIndex = 14;
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
            else if (randomIndex == 13) nextPrefab = KeyRoom;
            else if (randomIndex == 14) nextPrefab = KeyRoom1;
        } while (nextPrefab == null);

        return nextPrefab;
    }

    Transform GetExitPoint(GameObject room)
    {
        return room.transform.Find("ExitPoint");
    }

    void AlignRoom(GameObject room, Transform previousExitPoint)
    {
        // Aliniaza camera noua cu punctul de iesire al celei anterioare
        Transform entryPoint = room.transform.Find("EntryPoint");

        if (entryPoint == null || previousExitPoint == null)
        {
            Debug.LogError("EntryPoint sau ExitPoint lipsește!");
            return;
        }

        
        Vector3 offset = previousExitPoint.position - entryPoint.position;
        room.transform.position += offset;

        float angleDifference = previousExitPoint.eulerAngles.y - entryPoint.eulerAngles.y;
        room.transform.RotateAround(entryPoint.position, Vector3.up, angleDifference);
    }
}
