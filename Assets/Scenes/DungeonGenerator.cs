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

    public GameObject player; // Referin?? la juc?tor
    public int maxVisibleRooms = 2; // Num?rul de camere active simultan

    private Queue<GameObject> activeRooms = new Queue<GameObject>(); // Camerele active
    private Transform lastExitPoint; // Punctul de ie?ire al ultimei camere

    public GameObject playerPrefab;
    private int roomsGenerated = 0; // Num?r?m câte camere au fost generate
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
            Debug.LogError("Player-ul nu a fost g?sit! Asigur?-te c? ai setat player-ul în Inspector.");
            return;
        }

        // Gener?m camera de start
        GameObject startRoom = Instantiate(startRoomPrefab, Vector3.zero, Quaternion.identity);
        activeRooms.Enqueue(startRoom);

        //PlacePlayerInStartRoom(startRoom);

        // Set?m punctul de ie?ire al primei camere
        lastExitPoint = GetExitPoint(startRoom);

        // Gener?m prima camer? adiacent?
        GenerateNextRoom();
    }

    void Update()
    {
        // Verific?m dac? juc?torul a trecut de punctul de ie?ire
        if (Vector3.Distance(player.transform.position, lastExitPoint.position) < 5f)
        {
            GenerateNextRoom();
        }
    }

    void GenerateNextRoom()
    {
        // Verific?m dac? am ajuns la num?rul maxim de camere
        if (roomsGenerated >= maxRooms - 1)
        {
            // Genereaz? camera final?
            GameObject finalRoom = Instantiate(endRoomPrefab);
            AlignRoom(finalRoom, lastExitPoint);
            activeRooms.Enqueue(finalRoom);

            Debug.Log("FinalRoom a fost generat?!");

            // Dezactiveaz? generarea altor camere
            enabled = false; // Opre?te scriptul dup? generarea FinalRoom
            return;
        }

        // Ob?ine o camer? aleatorie
        GameObject nextRoomPrefab = GetRandomRoomPrefab();
        GameObject nextRoom = Instantiate(nextRoomPrefab);

        // Aliniem camera nou?
        AlignRoom(nextRoom, lastExitPoint);

        // Actualiz?m punctul de ie?ire
        lastExitPoint = GetExitPoint(nextRoom);

        // Ad?ug?m camera la coada camerelor active
        activeRooms.Enqueue(nextRoom);

        // ?tergem camerele vechi dac? sunt prea multe
        if (activeRooms.Count > maxVisibleRooms)
        {
            GameObject oldRoom = activeRooms.Dequeue();
            Destroy(oldRoom);
        }

        // Increment?m contorul de camere
        roomsGenerated++;
    }


    GameObject GetRandomRoomPrefab()
    {
        GameObject nextPrefab = null;
        int randomIndex = Random.Range(0, 13);

        if (randomIndex == 0)
        {
            nextPrefab = hallPrefab;

        }
        else if (randomIndex == 1)
        {
            nextPrefab = curvedHallLeftPrefab;

        }
        else if (randomIndex == 2)
        {
            nextPrefab = curvedHallRightPrefab;

        }
        else if (randomIndex == 3)
        {
            nextPrefab = largeHallPrefab;

        }
        else if (randomIndex == 4)
        {
            nextPrefab = roomType1;

        }
        else if (randomIndex == 5)
        {
            nextPrefab = roomType2;

        }
        else if (randomIndex == 6)
        {
            nextPrefab = roomType3;

        }
        else if (randomIndex == 7)
        {
            nextPrefab = roomType4;

        }
        else if (randomIndex == 8)
        {
            nextPrefab = LargecurvedHallLeftPrefab;

        }
        else if (randomIndex == 9)
        {
            nextPrefab = LargecurvedHallRightPrefab;

        }
        else if (randomIndex == 10)
        {
            nextPrefab = roomType5;

        }
        else if (randomIndex == 11)
        {
            nextPrefab = roomType6;

        }
        else if (randomIndex == 12)
        {
            nextPrefab = roomType7;

        }
        return nextPrefab;
    }

    Transform GetExitPoint(GameObject room)
    {
        // G?se?te punctul de ie?ire al unei camere
        return room.transform.Find("ExitPoint");
    }

    void AlignRoom(GameObject room, Transform previousExitPoint)
    {
        // Aliniaz? camera nou? cu punctul de ie?ire al celei anterioare
        Transform entryPoint = room.transform.Find("EntryPoint");

        if (entryPoint == null || previousExitPoint == null)
        {
            Debug.LogError("EntryPoint sau ExitPoint lips?!");
            return;
        }

        // Calculeaz? pozi?ia ?i rotirea pentru aliniere
        Vector3 offset = previousExitPoint.position - entryPoint.position;
        room.transform.position += offset;

        float angleDifference = previousExitPoint.eulerAngles.y - entryPoint.eulerAngles.y;
        room.transform.RotateAround(entryPoint.position, Vector3.up, angleDifference);
    }
}
