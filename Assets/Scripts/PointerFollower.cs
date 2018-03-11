using UnityEngine;

public class PointerFollower : MonoBehaviour
{
	void Update ()
    {
        Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2 = new Vector2(mousePos3.x, mousePos3.y);
        transform.position = mousePos2;
    }
}
