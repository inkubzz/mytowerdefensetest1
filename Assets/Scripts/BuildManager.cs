using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public GameObject environmentGO;
    public GameObject shopGO;
    public GameObject walkablePath;

    //public GameObject standardTurretPrefab;
    //public GameObject missileLauncherPrefab;

    private static TowerPoint selectedTowerPoint;
    private static Tower selectedTower;
    //private TowerBlueprint towerToBuild;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one BuildManager in scene!");
            return;
        }
        instance = this;
    }

    public void SelectTowerPoint(TowerPoint towerPoint)
    {
        // if there was a previously selected TowerPoint, first reset it
        if (selectedTowerPoint != null)
        {
            selectedTowerPoint.SetSelected(false);
        }

        if (towerPoint == null)
        {
            selectedTowerPoint = null;
            shopGO.GetComponent<ShopButtonManager>().HideShop();
        }
        else
        {
            // first deselect a possible selected Tower
            if (selectedTower != null)
            {
                selectedTower.SetSelected(false);
                selectedTower = null;
            }

            towerPoint.SetSelected(true);
            selectedTowerPoint = towerPoint;
            shopGO.GetComponent<ShopButtonManager>().ShowShop(this);
        }
    }

    public void SelectTower(Tower tower)
    {
        // if there was a previously selected Tower, first reset it
        if (selectedTower != null)
        {
            selectedTower.SetSelected(false);
        }

        if (tower == null)
        {
            selectedTower = null;
            shopGO.GetComponent<ShopButtonManager>().HideShop();
        }
        else
        {
            // first deselect a possible selected TowerPoint
            if (selectedTowerPoint != null)
            {
                selectedTowerPoint.SetSelected(false);
                selectedTowerPoint = null;
            }

            tower.SetSelected(true);
            selectedTower = tower;
            shopGO.GetComponent<ShopButtonManager>().ShowShop(this);
        }
    }

    public void ClearSelection()
    {
        SelectTowerPoint(null);
        SelectTower(null);
    }

    public void BuildTower(TowerBlueprint towerBlueprint)
    {
        if (towerBlueprint == null) return;
        if (selectedTowerPoint == null) return;

        Debug.Log("BuildManager.BuildTower");
        // check if selectedTowerPoint is vacant
        if (!selectedTowerPoint.IsVacant())
        {
            Debug.Log("TowerPoint is taken!");
            return;
        }
        else Debug.Log("TowerPoint is vacant; building tower");

        // check money
        // subtract if enough

        GameObject towerObject = (GameObject)Instantiate(towerBlueprint.prefab, selectedTowerPoint.GetBuildPosition(), Quaternion.identity);
        selectedTowerPoint.SetTower(towerObject.GetComponent<Tower>());
        selectedTowerPoint.gameObject.SetActive(false);

        ClearSelection();
    }

    public void SellTower()
    {
        if (selectedTower == null) return;

        // reenable the TowerPoint below the tower's GameObject
        TowerPoint towerPoint = selectedTower.GetTowerPoint();
        towerPoint.gameObject.SetActive(true);

        // get currency back
        // TODO:

        // destroy the selected tower's game object
        Destroy(selectedTower.gameObject);

        ClearSelection();
    }

    public void SetRallyPoint()
    {
        if (selectedTower == null) return;

        Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2 = new Vector2(mousePos3.x, mousePos3.y);
        selectedTower.StartSettingRallyPoint(mousePos2);
        //selectedTower.SetRallyPoint(mousePos2);
    }

    public bool IsSelectedTowerPoint()
    {
        return (selectedTowerPoint != null);
    }

    public bool IsSelectedTower()
    {
        return (selectedTower != null);
    }
    

    //public void SelectTowerToBuild(TowerBlueprint towerBlueprint)
    //{
    //    towerToBuild = towerBlueprint;
    //}


    //public void BuildTowerOn(TowerPoint towerPoint)
    //{
    //    if (PlayerStats.Money < towerToBuild.cost)
    //    {
    //        Debug.Log("Not enough money");
    //        return;
    //    }

    //    PlayerStats.Money -= towerToBuild.cost;

    //    GameObject tower = (GameObject)Instantiate(towerToBuild.prefab, towerPoint.GetBuildPosition(), Quaternion.identity);
    //    towerPoint.tower = tower;

    //    Debug.Log("Tower built. Money left: " + PlayerStats.Money);
    //}

    //public bool CanBuild { get { return towerToBuild != null; } }
}
