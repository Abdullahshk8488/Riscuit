using UnityEngine;

public class RunDungeonGenerator : MonoBehaviour
{
    [SerializeField] private AbstractDungeonGenerator abstractDungeonGenerator;

    private void Start()
    {
        abstractDungeonGenerator.GenerateDungeon();
        Player player = PlayerManager.Instance.MainPlayer;
        RoomManager roomManager = RoomManager.Instance;
        Transform startPoint = roomManager.GetPlayerSpawnPoint();
        player.transform.position = startPoint.position;
    }
}
