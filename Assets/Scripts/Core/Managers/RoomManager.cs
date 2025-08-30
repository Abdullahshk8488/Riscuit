using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }
    [SerializeField] private Node nodePrefab;
    private List<Room> rooms = new List<Room>();
    public Room CurrentRoom { get; private set; }
    private int _roomCount = 0;

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
        Debug.Log("Destroying rooms");
        foreach (Room room in rooms)
        {
            foreach (Node node in room.RoomNodes)
            {
                Destroy(node.gameObject);
            }
            Destroy(room.gameObject);
        }
        rooms.Clear();
        CurrentRoom = null;
    }

    public void CreateRoom(HashSet<Vector2Int> nodes, Vector2 centerPoint)
    {
        if (_roomsParent == null)
        {
            _roomsParent = new GameObject("Rooms");
        }

        GameObject roomGo = new GameObject("Room " + _roomCount++);
        Room room = roomGo.AddComponent<Room>();
        roomGo.transform.SetParent(_roomsParent.transform);
        rooms.Add(room);

        List<Node> nodeList = new List<Node>();
        foreach (Vector2Int node in nodes)
        {
            Node newNode = Instantiate(nodePrefab, new Vector2(node.x + 0.5f, node.y + 0.5f), Quaternion.identity);
            nodeList.Add(newNode);
            newNode.transform.SetParent(room.transform);
        }

        CreateConnections(nodeList);
        room.RoomNodes = nodeList;

        GameObject centerOfTheRoom = new GameObject("Center Point");
        centerOfTheRoom.transform.position = centerPoint;
        centerOfTheRoom.transform.SetParent(room.transform);
        room.CenterOfTheRoom = centerOfTheRoom.transform;
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
}
