using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject startRoomPrefab;
    public GameObject hallPrefab;
    public GameObject curvedHallLeftPrefab;
    public GameObject curvedHallRightPrefab;
    public GameObject endRoomPrefab;

    private int maxRooms = 10;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Transform lastExitPoint = null;

        // Instan?iem camera de start la pozi?ia ini?ial? (0, 0, 0)
        GameObject startRoom = Instantiate(startRoomPrefab, Vector3.zero, Quaternion.identity);
        lastExitPoint = GetExitPoint(startRoom);

        // Gener?m camerele intermediare
        for (int i = 1; i < maxRooms - 1; i++)
        {
            GameObject nextRoomPrefab = GetRandomRoomPrefab();
            GameObject nextRoom = Instantiate(nextRoomPrefab);

            // Aliniem urm?toarea camer?
            AlignRoom(nextRoom, lastExitPoint);
            lastExitPoint = GetExitPoint(nextRoom);
        }

        // Instan?iem camera de final
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

        // Alineaz? pozi?ia camerei noi astfel încât EntryPoint s? se suprapun? peste ExitPoint-ul anterior
        Vector3 offset = previousExitPoint.position - entryPoint.position;
        room.transform.position += offset;

        // Debug pentru verificarea pozi?iilor
        Debug.Log($"Aliniem camera {room.name} la pozi?ia {room.transform.position}");

        // Alineaz? rota?ia pentru a p?stra direc?ia corect?
        float angleDifference = previousExitPoint.eulerAngles.y - entryPoint.eulerAngles.y;
        room.transform.RotateAround(entryPoint.position, Vector3.up, angleDifference);

        // Debug pentru rota?ie
        Debug.Log($"Rotim camera {room.name} cu diferen?a de unghi: {angleDifference}");
    }
}
