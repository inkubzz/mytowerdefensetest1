using UnityEngine;

public class Shop : MonoBehaviour
{
    public TowerBlueprint tower1;
    //public TowerBlueprint tower2;

    private BuildManager buildManager;
    
    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void BuildTower1()
    {
        Debug.Log("clicked build tower 1");
        buildManager.BuildTower(tower1);
    }

    public void SellTower()
    {
        buildManager.SellTower();
    }

    public void SetRallyPoint()
    {
        buildManager.SetRallyPoint();
    }

    //public void BuildTower2()
    //{
    //    Debug.Log("clicked build tower 2");
    //    buildManager.SelectTurretToBuild(tower2);
    //}
}
