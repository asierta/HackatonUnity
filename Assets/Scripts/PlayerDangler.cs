using UnityEngine;

public class PlayerDangler : MonoBehaviour
{
    private Vector2 mousePos;

    void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Get the angle of the mouse regarding the player
        float angle = Mathf.Atan2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        gameObject.transform.Rotate(Vector3.up, angle);
    }
}
