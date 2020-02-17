using UnityEngine;

public class PlayerDangler : MonoBehaviour
{
    public float rotationSpeed = 1;
    private Vector2 mousePos;
    private bool IsDangling;

    void Start()
    {
        Cursor.visible = true;
        IsDangling = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Get the angle of the mouse regarding the player
        float angle = Mathf.Atan2(transform.position.y - mousePos.y, transform.position.x - mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, (angle+90)), rotationSpeed * Time.deltaTime);
    }
}
