using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }
    [SerializeField] private Node nodePrefab;
    [SerializeField] private GameObject doorTrigger;
    private List<Room> rooms = new List<Room>();
    public Room CurrentRoom { get; private set; }
    private int _roomCount = 0;
    private GameObject _doorTriggers = null;

    private GameObject _roomsParent = null;
    public List<Node> GetCurrentRoomNodes
    {
        get
        {
            if (CurrentRoom == null) return null;
            return CurrentRoom.RoomNodes;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetRooms()
    {
        foreach (Room room in rooms)
        {
            if (room == null)
            {
                continue;
            }

            foreach (Node node in room.RoomNodes)
            {
                if (node == null)
                {
                    continue;
                }
                Destroy(node.gameObject);
            }
            Destroy(room.gameObject);
        }

        DestroyChildren(_roomsParent);
        DestroyChildren(_doorTriggers);
        rooms.Clear();
        CurrentRoom = null;
        _roomCount = 0;
    }

    private void DestroyChildren(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return;
        }

        for (int i = 0; i < _roomsParent.transform.childCount; i++)
        {
            Destroy(_roomsParent.transform.GetChild(i).gameObject);
        }
    }

    public Transform GetPlayerSpawnPoint()
    {
        if (rooms.Count == 0)
        {
            return null;
        }

        return rooms[0].CenterOfTheRoom;
    }

    public void CreateRoom(HashSet<Vector2Int> nodes, Vector2 centerPoint)
    {
        if (_roomsParent == null)
        {
            _roomsParent = new GameObject("Rooms");
        }

        GameObject roomGo = new GameObject("Room " + _roomCount++);
        Room room = roomGo.AddComponent<Room>();
        room.transform.position = centerPoint;
        roomGo.transform.SetParent(_roomsParent.transform);
        rooms.Add(room);

        GameObject centerOfTheRoom = new GameObject("Center Point");
        centerOfTheRoom.transform.position = centerPoint;
        centerOfTheRoom.transform.SetParent(room.transform);
        room.CenterOfTheRoom = centerOfTheRoom.transform;

        List<Node> nodeList = new List<Node>();
        foreach (Vector2Int node in nodes)
        {
            Node newNode = Instantiate(nodePrefab, new Vector2(node.x + 0.5f, node.y + 0.5f), Quaternion.identity);
            nodeList.Add(newNode);
            newNode.transform.SetParent(room.transform);
        }

        CreateConnections(nodeList);
        room.RoomNodes = nodeList;
    }

    private void CreateConnections(List<Node> nodeList)
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            for (int j = 0; j < nodeList.Count; j++)
            {
                if (Vector2.Distance(nodeList[i].transform.position, nodeList[j].transform.position) <= 1.0f)
                {
                    ConnectNodes(nodeList[i], nodeList[j]);
                    ConnectNodes(nodeList[j], nodeList[i]);
                }
            }
        }
    }

    private void ConnectNodes(Node from, Node to)
    {
        if (from == to)
        {
            return;
        }

        from.connections.Add(to);
    }

    public void SetCurrentRoom(Room room)
    {
        CurrentRoom = room;
    }

    public void SetFirstRoom()
    {
        if (rooms == null)
        {
            return;
        }
        if (rooms.Count == 0)
        {
            return;
        }

        CurrentRoom = rooms[0];
    }

    public void PlaceTriggers(List<CorridorStartEnd> corridors)
    {
        if (_doorTriggers == null)
        {
            _doorTriggers = new GameObject("Door Triggers");
        }

        for (int i = 0; i < corridors.Count; i++)
        {
            GameObject go = Instantiate(doorTrigger);
            go.transform.position = (Vector2)corridors[i].start;
            go.transform.SetParent(_doorTriggers.transform);
        }
        GameObject goEnd = Instantiate(doorTrigger);
        goEnd.transform.position = (Vector2)corridors[corridors.Count - 1].end;
        goEnd.transform.SetParent(_doorTriggers.transform);
    }

    public void SetTriggerForRooms()
    {
        List<GameObject> triggers = new List<GameObject>();
        for (int i = 0; i < _doorTriggers.transform.childCount; i++)
        {
            triggers.Add(_doorTriggers.transform.GetChild(i).gameObject);
        }

        // Check if each trigger is inside a room

        for (int i = triggers.Count - 1; i >= 0; i--)
        {
            bool deleteTrigger = true;
            foreach (Room room in rooms)
            {
                if (IsTriggerInRoom(triggers[i].transform, room))
                {
                    room.TriggerObject = triggers[i];
                    deleteTrigger = false;
                    continue;
                }
            }

            if (!deleteTrigger)
            {
                continue;
            }

            GameObject triggerNotInRoom = triggers[i];
            triggers.RemoveAt(i);
            Destroy(triggerNotInRoom);
        }
    }

    private bool IsTriggerInRoom(Transform triggerTransform, Room room)
    {
        Vector2 min = room.RoomNodes[0].transform.position;
        Vector2 max = room.RoomNodes[0].transform.position;

        for (int i = 1; i < room.RoomNodes.Count; i++)
        {
            min.x = Mathf.Min(min.x, room.RoomNodes[i].transform.position.x);
            min.y = Mathf.Min(min.y, room.RoomNodes[i].transform.position.y);

            max.x = Mathf.Max(max.x, room.RoomNodes[i].transform.position.x);
            max.y = Mathf.Max(max.y, room.RoomNodes[i].transform.position.y);
        }

        return triggerTransform.position.x >= min.x
            && triggerTransform.position.y >= min.y
            && triggerTransform.position.x <= max.x
            && triggerTransform.position.y <= max.y;
    }
}
