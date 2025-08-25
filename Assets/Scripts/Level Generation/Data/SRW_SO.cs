using UnityEngine;

[CreateAssetMenu(fileName = "SRW_Params_", menuName = "PCG/SRW Data")]
public class SRW_SO : ScriptableObject
{
    public int iterations = 10, walkLength = 10;
    public bool startRandomlyEachIteration = true;
}
