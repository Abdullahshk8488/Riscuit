using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Node> RoomNodes { get; set; }
    public Transform CenterOfTheRoom { get; set; }
    public GameObject TriggerObject;
    private bool _isFirstTimeTriggered = false;
    public bool isStartingRoom = false;

    public void SetDoorTrigger(DoorTrigger doorTrigger)
    {
        TriggerObject = doorTrigger.gameObject;
        doorTrigger.room = this;
    }

    public void RoomTrigger()
    {
        RoomManager.Instance.SetCurrentRoom(this);

        if (!_isFirstTimeTriggered && !isStartingRoom)
        {
            SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < 5; ++i)
        {
            Enemy_Controller enemyController = Instantiate(RoomManager.Instance.GetRandomEnemy);
            enemyController.transform.position = CenterOfTheRoom.position;
            enemyController.currentNode = NearestNode(CenterOfTheRoom.position);
        }
    }

    private Node NearestNode(Vector2 position)
    {
        Node closestNode = RoomNodes[0];
        float closestDistance = Vector2.Distance(position, closestNode.transform.position);
        for (int i = 1; i < RoomNodes.Count; ++i)
        {
            float currentDistance = Vector2.Distance(position, closestNode.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestNode = RoomNodes[i];
            }
        }
        return closestNode;
    }
}
