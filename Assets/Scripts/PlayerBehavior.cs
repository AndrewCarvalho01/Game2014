using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float horizontalForce;
    [Range(0.0f, 1.0f)] public float decay;

    private Rigidbody2D rb;
    private Camera cam;
    private float halfWidth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        // Half the ship's width, so the whole ship stays on screen, not just its centre.
        halfWidth = GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
    }
    private void FixedUpdate()
    {
        rb.linearVelocity *= (1.0f - decay);
        Move();
        CheckBounds();
    }
    private float GetTouchDirection()
    {
        if (Touchscreen.current == null)
        {
            return 0.0f;
        }

        var touch = Touchscreen.current.primaryTouch;

        if (!touch.press.isPressed)
        {
            return 0.0f;
        }

        Vector2 pos = touch.position.ReadValue();

        // Screen.width updates when the phone rotates, so this still splits
        // the screen into left and right halves in landscape.
        if (pos.x < Screen.width / 2)
        {
            return -1.0f;
        }

        return 1.0f;
    }

    private void Move()
    {
        float x = GetTouchDirection();
        rb.AddForce(new Vector2(x * horizontalForce, 0.0f));
    }

    private void CheckBounds()
    {
        // Work out the screen edges from the camera every step, so the limits
        // grow in landscape and shrink back in portrait.
        float min = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x + halfWidth;
        float max = cam.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x - halfWidth;

        Vector2 pos = rb.position;

        if (pos.x < min || pos.x > max)
        {
            pos.x = Mathf.Clamp(pos.x, min, max);
            rb.position = pos;
            rb.linearVelocity = new Vector2(0.0f, rb.linearVelocity.y);
        }
    }
}