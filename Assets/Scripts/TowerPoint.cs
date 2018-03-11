using UnityEngine;
//using UnityEngine.EventSystems;

public class TowerPoint : MonoBehaviour
{
    public Color hoverColor = Color.green;
    //private Color hoverColor = new Color(1f, 0f, 0f, 1f);

    private bool isSelected = false;

    private SpriteRenderer rend;
    private Color defaultColor;
    private Color selectedColor = Color.red;

    private Tower tower;
    
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        // make sure the opacity of the hover color isn't 0
        if (hoverColor.a < 0.1f) hoverColor.a = 0.1f;
        if (selectedColor.a < 0.1f) selectedColor.a = 0.1f;

        defaultColor = rend.color;
    }

    private void Update()
    {
        if (isSelected)
        {
            // if escape key is pressed, deselect this TowerPoint
            if (Input.GetKeyDown("escape"))
            {
                BuildManager buildManager = BuildManager.instance;
                buildManager.SelectTowerPoint(null);
            }
        }
    }

    private void OnMouseEnter()
    {
        //if (EventSystem.current.IsPointerOverGameObject()) return;

        //if (!buildManager.CanBuild) return;

        // set hover color regardless of its selected status
        SetHoverColor();
    }

    private void OnMouseExit()
    {
        // when mouse is leaving, if it's selected, set selected color; otherwise, set default color
        if (isSelected)
        {
            SetSelectedColor();
        }
        else
        {
            SetDefaultColor();
        }
    }

    private void OnMouseDown()
    {
        BuildManager buildManager = BuildManager.instance;

        // invert this object's being selected;
        // SelectTowerPoint takes care of the bool and the color
        if (isSelected)
        {
            buildManager.SelectTowerPoint(null);
        }
        else
        {
            buildManager.SelectTowerPoint(this);
        }
    }

    private void SetDefaultColor()
    {
        rend.color = defaultColor;
    }

    private void SetHoverColor()
    {
        rend.color = hoverColor;
    }

    private void SetSelectedColor()
    {
        rend.color = selectedColor;
    }

    public Vector3 GetBuildPosition()
    {
        //return transform.position + positionOffset;
        return transform.position;
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            // set selected color
            SetSelectedColor();
        }
        else
        {
            // set default color
            SetDefaultColor();
        }
        isSelected = selected;
    }

    public void SetTower(Tower _tower)
    {
        //Debug.Log("TowerPoint.SetTower");
        this.tower = _tower;
        tower.SetTowerPoint(this);
    }

    public bool IsVacant()
    {
        return this.tower == null;
    }
}
