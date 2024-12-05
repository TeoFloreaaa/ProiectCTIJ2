using UnityEngine;

public class DungeonGenerator : MonoBehaviour
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

    public GameObject playerPrefab; 
    private GameObject playerInstance; 

    private int maxRooms = 10;

    private string lastRoomType = "";

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
        GameObject nextPrefab = null;

        do
        {
            int randomIndex = Random.Range(0, 8);
            if (randomIndex == 0)
            {
                nextPrefab = hallPrefab;
                lastRoomType = "hall";
            }
            else if (randomIndex == 1 && lastRoomType != "curvedHallLeft")
            {
                nextPrefab = curvedHallLeftPrefab;
                lastRoomType = "curvedHallLeft";
            }
            else if (randomIndex == 2 && lastRoomType != "curvedHallRight")
            {
                nextPrefab = curvedHallRightPrefab;
                lastRoomType = "curvedHallRight";
            }
            else if(randomIndex == 3)
            {
                nextPrefab = largeHallPrefab;
                lastRoomType = "largeHall";
            }
            else if(randomIndex == 4)
            {
                nextPrefab = roomType1;
                lastRoomType = "roomType1";
            }
            else if (randomIndex == 5)
            {
                nextPrefab = roomType2;
                lastRoomType = "roomType2";
            }
            else if (randomIndex == 6)
            {
                nextPrefab = roomType3;
                lastRoomType = "roomType4";
            }
            else if (randomIndex == 7)
            {
                nextPrefab = roomType4;
                lastRoomType = "roomType4";
            }
        } while (nextPrefab == null);

        return nextPrefab;
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
            Debug.LogError("Player-ul lipseste sau nu a fost adaugat in scena!");
        }
    }
}
