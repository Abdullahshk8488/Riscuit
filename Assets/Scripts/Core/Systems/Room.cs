using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Node> RoomNodes { get; set; }
    public Transform CenterOfTheRoom { get; set; }
    public GameObject TriggerObject;
    private bool _hasBeenTriggered = false;
    public bool isStartingRoom = false;
    public bool IsRoomCleared = false;
    private int _enemySpawnedInRoom = 0;

    public void SetDoorTrigger(DoorTrigger doorTrigger)
    {
        TriggerObject = doorTrigger.gameObject;
        doorTrigger.room = this;
    }

    public void RoomTrigger()
    {
        RoomManager.Instance.SetCurrentRoom(this);

        if (!_hasBeenTriggered && !isStartingRoom)
        {
            _hasBeenTriggered = true;
            SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        int rand = Random.Range(1, 6);
        _enemySpawnedInRoom += rand;
        for (int i = 0; i < rand; ++i)
        {
            Enemy_Controller enemyController = Instantiate(RoomManager.Instance.GetRandomEnemy);
            enemyController.transform.position = CenterOfTheRoom.position;
            enemyController.currentNode = NearestNode(CenterOfTheRoom.position);
            enemyController.EnemyDeath += EnemyDied;
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

    private void EnemyDied()
    {
        _enemySpawnedInRoom--;
        if (_enemySpawnedInRoom <= 0)
        {
            IsRoomCleared = true;
            GameManager.Instance.CheckIfAllRoomsCompleted();
        }
    }
}
