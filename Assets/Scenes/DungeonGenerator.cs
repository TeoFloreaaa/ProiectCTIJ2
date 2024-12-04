using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject startRoomPrefab;
    public GameObject hallPrefab;
    public GameObject curvedHallLeftPrefab;
    public GameObject curvedHallRightPrefab;
    public GameObject endRoomPrefab;

    public GameObject playerPrefab; 
    private GameObject playerInstance; 

    private int maxRooms = 8;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Transform lastExitPoint = null;

        
        GameObject startRoom = Instantiate(startRoomPrefab, Vector3.zero, Quaternion.identity);
        lastExitPoint = GetExitPoint(startRoom);

        
        PlacePlayerInStartRoom(startRoom);

        
        for (int i = 1; i < maxRooms - 1; i++)
        {
            GameObject nextRoomPrefab = GetRandomRoomPrefab();
            GameObject nextRoom = Instantiate(nextRoomPrefab);

            AlignRoom(nextRoom, lastExitPoint);
            lastExitPoint = GetExitPoint(nextRoom);
        }

        
        GameObject endRoom = Instantiate(endRoomPrefab);
        AlignRoom(endRoom, lastExitPoint);
    }

    GameObject GetRandomRoomPrefab()
    {
        int randomIndex = Random.Range(0, 3);
        if (randomIndex == 0) return hallPrefab;
        else if (randomIndex == 1) return curvedHallLeftPrefab;
        else return curvedHallRightPrefab;
    }

    Transform GetExitPoint(GameObject room)
    {
        return room.transform.Find("ExitPoint");
    }

    void AlignRoom(GameObject room, Transform previousExitPoint)
    {
        Transform entryPoint = room.transform.Find("EntryPoint");

        if (entryPoint == null || previousExitPoint == null)
        {
            Debug.LogError("EntryPoint sau ExitPoint lips?!");
            return;
        }

        
        Vector3 offset = previousExitPoint.position - entryPoint.position;
        room.transform.position += offset;

        
        float angleDifference = previousExitPoint.eulerAngles.y - entryPoint.eulerAngles.y;
        room.transform.RotateAround(entryPoint.position, Vector3.up, angleDifference);
    }

    void PlacePlayerInStartRoom(GameObject startRoom)
    {
        
        if (playerInstance == null && playerPrefab != null)
        {
            
            Transform entryPoint = startRoom.transform.Find("EntryPoint");
            Vector3 playerPosition = entryPoint != null ? entryPoint.position : startRoom.transform.position;
            playerPosition.y += 2f;
            playerPosition.z += 2f;
            playerInstance = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("PlayerPrefab este lips? sau juc?torul a fost deja plasat!");
        }
    }
}
