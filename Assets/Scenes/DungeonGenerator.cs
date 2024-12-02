using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject startRoomPrefab;
    public GameObject hallPrefab;
    public GameObject curvedHallLeftPrefab;
    public GameObject curvedHallRightPrefab;
    public GameObject endRoomPrefab;

    public GameObject playerPrefab; // Prefab-ul pentru juc?tor
    private GameObject playerInstance; // Referin?? la instan?a juc?torului

    private int maxRooms = 5;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Transform lastExitPoint = null;

        // Plas?m camera de start
        GameObject startRoom = Instantiate(startRoomPrefab, Vector3.zero, Quaternion.identity);
        lastExitPoint = GetExitPoint(startRoom);

        // Plas?m juc?torul în camera de start
        PlacePlayerInStartRoom(startRoom);

        // Gener?m camerele intermediare
        for (int i = 1; i < maxRooms - 1; i++)
        {
            GameObject nextRoomPrefab = GetRandomRoomPrefab();
            GameObject nextRoom = Instantiate(nextRoomPrefab);

            AlignRoom(nextRoom, lastExitPoint);
            lastExitPoint = GetExitPoint(nextRoom);
        }

        // Plas?m camera de final
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

        // Aliniem pozi?ia camerei noi astfel încât EntryPoint s? se suprapun? peste ExitPoint-ul anterior
        Vector3 offset = previousExitPoint.position - entryPoint.position;
        room.transform.position += offset;

        // Rote?te camera pentru a se potrivi direc?iei punctului anterior
        float angleDifference = previousExitPoint.eulerAngles.y - entryPoint.eulerAngles.y;
        room.transform.RotateAround(entryPoint.position, Vector3.up, angleDifference);
    }

    void PlacePlayerInStartRoom(GameObject startRoom)
    {
        // Verific?m dac? player-ul este deja plasat
        if (playerInstance == null && playerPrefab != null)
        {
            // Pozi?ion?m juc?torul în centrul camerei de start sau la un punct specific (ex. EntryPoint)
            Transform entryPoint = startRoom.transform.Find("EntryPoint");
            Vector3 playerPosition = entryPoint != null ? entryPoint.position : startRoom.transform.position;
            playerPosition.y += 2f;
            // Instan?iem juc?torul la pozi?ia calculat?
            playerInstance = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("PlayerPrefab este lips? sau juc?torul a fost deja plasat!");
        }
    }
}
