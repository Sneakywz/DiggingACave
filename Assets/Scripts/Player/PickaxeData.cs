using UnityEngine;

/// <summary>
/// Stores pickaxe mining power.
/// </summary>
public class PickaxeData : MonoBehaviour
{
    [SerializeField] private int miningPower = 1;

    public int MiningPower => miningPower;
}