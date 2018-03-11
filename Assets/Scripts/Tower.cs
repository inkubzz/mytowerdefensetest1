using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject rallyPointSprite;

    private bool isSelected = false;

    private SpriteRenderer rend;
    private Color defaultColor;
    private Color hoverColor = new Color(0.52f, 0.52f, 0.52f, 1f);
    private Color selectedColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    private TowerPoint towerPoint; // a reference to the tower point below this tower
    private Vector2 rallyPoint;
    private GameObject rallyPointGO;
    private const float rallyPointMaxDistance = 2.5f;

    private bool isSettingRallyPoint = false;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        defaultColor = rend.color;

        SetDefaultRallyPoint();
    }

    private void Update()
    {
        if (isSettingRallyPoint)
        {
            if (Input.GetKeyDown("escape") || Input.GetMouseButtonDown(1))
            {
                Debug.Log("Escape or right mouse button pressed");
                StopSettingRallyPoint();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Left mouse button pressed");
                SetNewRallyPoint(Input.mousePosition);
            }
        }
        else if (isSelected)
        {
            // if escape key is pressed, deselect this tower
            if (Input.GetKeyDown("escape"))
            {
                BuildManager buildManager = BuildManager.instance;
                buildManager.SelectTower(null);
            }
            // maybe check here if the user has clicked somewhere outside of this tower and outside of the shop panel
            //else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            //{
            //    Debug.Log("mouse click");
            //    CircleCollider2D cc2 = GetComponent<CircleCollider2D>();
            //    if (cc2 != null)
            //    {
            //        Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //        Vector2 mousePos2 = new Vector2(mousePos3.x, mousePos3.y);
            //        //
            //        if (!cc2.bounds.Contains(mousePos3))
            //        {
            //            Debug.Log("not contained");
            //            //BuildManager buildManager = BuildManager.instance;
            //            //buildManager.SelectTower(null);
            //        }
            //        else Debug.Log("contained");
            //    }
            //}
        }
    }

    private void OnMouseEnter()
    {
        //if (EventSystem.current.IsPointerOverGameObject()) return;

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
        // SelectTower takes care of the bool and the color
        if (isSelected)
        {
            buildManager.SelectTower(null);
        }
        else
        {
            buildManager.SelectTower(this);
        }
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            // set selected color
            SetSelectedColor();

            // draw rally point sprite
            rallyPointGO = (GameObject)Instantiate(rallyPointSprite, rallyPoint, Quaternion.identity);
        }
        else
        {
            // set default color
            SetDefaultColor();

            Debug.Log("Tower.SetSelected: " + selected);

            // remove rally point sprite
            if (rallyPointGO != null)
            {
                Debug.Log("rallyPointGO is NOT null; destroying it ");
                Destroy(rallyPointGO);
            }
            else Debug.Log("rallyPointGO is null");
        }

        isSelected = selected;
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

    public void SetTowerPoint(TowerPoint _towerPoint)
    {
        this.towerPoint = _towerPoint;
    }

    public TowerPoint GetTowerPoint()
    {
        return this.towerPoint;
    }

    public void SetRallyPoint(Vector2 _rallyPoint)
    {
        this.rallyPoint = _rallyPoint;
    }

    private void SetDefaultRallyPoint()
    {
        // TODO: set it to the nearest point in the walkable path

        rallyPoint = new Vector2(transform.position.x - 1, transform.position.y - 1);
    }

    public void StartSettingRallyPoint(Vector2 mousePos)
    {
        isSettingRallyPoint = true;

        // destroy the currently showing (fixed) rally point sprite
        Destroy(rallyPointGO);

        // create a new rally point sprite that follows the pointer and set it to the global var
        rallyPointGO = Instantiate(rallyPointSprite, mousePos, Quaternion.identity);
        rallyPointGO.AddComponent<PointerFollower>();
    }

    private void StopSettingRallyPoint()
    {
        Debug.Log("Tower.StopSettingRallyPoint");
        isSettingRallyPoint = false;

        if (rallyPointGO != null)
        {
            // destroy the current instance of the rally point sprite that is following the pointer
            Destroy(rallyPointGO);

            if (this.isSelected)
            {
                // create a new rally point sprite at the rally point position
                rallyPointGO = Instantiate(rallyPointSprite, rallyPoint, Quaternion.identity);
            }
        }
    }

    private void SetNewRallyPoint(Vector3 mousePosition)
    {
        Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 mousePos2 = new Vector2(mousePos3.x, mousePos3.y);
        
        BuildManager buildManager = BuildManager.instance;

        bool validPos = false;

        if (Vector2.Distance(transform.position, mousePos2) > rallyPointMaxDistance)
        {
            validPos = false;
        }
        else // distance is fine
        {
            // STILL doesn't work as it should
            bool contained = false;
            GameObject walkablePath = buildManager.walkablePath;
            for (int i = 0; i < walkablePath.transform.childCount; i++)
            {
                Transform wp = walkablePath.transform.GetChild(i);
                BoxCollider2D bc2 = wp.GetComponent<BoxCollider2D>();

                mousePos3.z = bc2.transform.position.z;
                Vector3 center = bc2.bounds.center;

                if (bc2.bounds.Contains(mousePos3))
                {
                    contained = true;
                    break;
                }
            }

            if (contained)
            {
                validPos = true;
            }
            else
            {
                validPos = false;
            }
        }

        if (validPos)
        {
            rallyPoint = mousePos2;
            StopSettingRallyPoint();
        }
        else
        {
            // briefly change the sprite to an x and then change it back
            ChangeRallyPointSpriteX();
        }
        
    }
    
    private void ChangeRallyPointSpriteX()
    {
        //Debug.Log("ChangeRallyPointSpriteX");
        if (rallyPointGO == null) return;
        
        var xSprite = Resources.Load<Sprite>("Sprites/xSprite");
        rallyPointGO.GetComponent<SpriteRenderer>().sprite = xSprite;
        Invoke("RevertRallyPointSprite", 0.5f);
    }

    private void RevertRallyPointSprite()
    {
        //Debug.Log("RevertRallyPointSprite");
        if (rallyPointGO == null) return;

        var defaultSprite = Resources.Load<Sprite>("Sprites/RallyPoint");
        rallyPointGO.GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }

    // from https://codereview.stackexchange.com/questions/108857/point-inside-polygon-check
    // hasn't helped me so far
    public static bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        int polygonLength = polygon.Length, i = 0;
        bool inside = false;
        // x, y for tested point.
        float pointX = point.x, pointY = point.y;
        // start / end point for the current polygon segment.
        float startX, startY, endX, endY;
        Vector2 endPoint = polygon[polygonLength - 1];
        endX = endPoint.x;
        endY = endPoint.y;
        while (i < polygonLength)
        {
            startX = endX; startY = endY;
            endPoint = polygon[i++];
            endX = endPoint.x; endY = endPoint.y;
            //
            inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
                      && /* if so, test if it is under the segment */
                      ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
        }

        return inside;
    }
}
