using UnityEngine;

public class ShopButtonManager : MonoBehaviour
{
    public GameObject buttonPurchaseTower1;
    public GameObject buttonSellTower;
    public GameObject buttonSetRallyPoint;

    //private BuildManager buildManager;

    private void Start()
    {
        //buildManager = BuildManager.instance;
    }

    public void HideShop()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowShop(BuildManager buildManager)
    {
        this.gameObject.SetActive(true);
        
        EnableButtonPurchaseTower1(buildManager.IsSelectedTowerPoint());
        EnableButtonSellTower(buildManager.IsSelectedTower());
        EnableButtonSetRallyPoint(buildManager.IsSelectedTower());
    }

    private void EnableButtonPurchaseTower1(bool active)
    {
        if (buttonPurchaseTower1 != null)
        {
            buttonPurchaseTower1.SetActive(active);
        }
    }

    private void EnableButtonSellTower(bool active)
    {
        if (buttonSellTower != null)
        {
            buttonSellTower.SetActive(active);
        }
    }

    private void EnableButtonSetRallyPoint(bool active)
    {
        if (buttonSetRallyPoint != null)
        {
            buttonSetRallyPoint.SetActive(active);
        }
    }
}
