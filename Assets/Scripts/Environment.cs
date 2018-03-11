using UnityEngine;
using UnityEngine.EventSystems;

public class Environment : MonoBehaviour
{
    public GameObject rallyPointSprite;

    public void SetRallyPoint()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        GameObject rallyPointGO = (GameObject)Instantiate(rallyPointSprite, mousePosition, Quaternion.identity);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("OVER OTHER OBJECT Environment.OnMouseDown");
        }
        else
        {
            //Debug.Log("Environment.OnMouseDown");
            //BuildManager buildManager = BuildManager.instance;
            //buildManager.ClearSelection();
        }
    }
}
