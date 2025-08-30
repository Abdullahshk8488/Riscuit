using UnityEngine;

public class RunDungeonGenerator : MonoBehaviour
{
    [SerializeField] private AbstractDungeonGenerator abstractDungeonGenerator;

    private void Start()
    {
        abstractDungeonGenerator.GenerateDungeon();
    }
}
