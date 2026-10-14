using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool isGrounded = false;
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
