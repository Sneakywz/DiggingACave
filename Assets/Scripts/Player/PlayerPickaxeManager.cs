using UnityEngine;

/// <summary>
/// Manages equipped pickaxe and mining power.
/// </summary>
public class PlayerPickaxeManager : MonoBehaviour
{
    [SerializeField] private PickaxeData woodPickaxe;
    [SerializeField] private PickaxeData stonePickaxe;
    [SerializeField] private PickaxeData ironPickaxe;

    private PickaxeData currentPickaxe;

    private void Awake()
    {
        EquipWoodPickaxe();
    }

    public void EquipWoodPickaxe()
    {
        SetActivePickaxe(woodPickaxe);
    }

    public void EquipStonePickaxe()
    {
        SetActivePickaxe(stonePickaxe);
    }

    public void EquipIronPickaxe()
    {
        SetActivePickaxe(ironPickaxe);
    }

    private void SetActivePickaxe(PickaxeData pickaxe)
    {
        woodPickaxe.gameObject.SetActive(false);
        stonePickaxe.gameObject.SetActive(false);
        ironPickaxe.gameObject.SetActive(false);

        pickaxe.gameObject.SetActive(true);
        currentPickaxe = pickaxe;
    }

    public int GetCurrentMiningPower()
    {
        return currentPickaxe != null ? currentPickaxe.MiningPower : 1;
    }
}