using UnityEngine;

// Scrolls the background downward and keeps it covering the whole screen in any orientation.
// It shows ONE copy of the image, scaled to fill the screen without stretching
// (the extra part is cropped off), so you never see the edges of the wallpaper.
public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.05f;

    private Material mat;
    private Camera cam;
    private float offset;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        cam = Camera.main;
    }

    void Update()
    {
        // Increasing the Y offset makes the image appear to move down.
        offset = Mathf.Repeat(offset + scrollSpeed * Time.deltaTime, 1f);

        FitToScreen();
    }

    private void FitToScreen()
    {
        // Size of the area the camera sees, in world units.
        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;

        // Make the quad cover the whole view, centred on the camera.
        transform.localScale = new Vector3(width, height, 1f);
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);

        // Compare the screen's shape to the image's shape.
        Texture tex = mat.mainTexture;
        float textureAspect = (float)tex.width / tex.height;
        float screenAspect = width / height;

        float scaleX = 1f;
        float scaleY = 1f;

        if (screenAspect > textureAspect)
        {
            // Screen is wider than the image (landscape): use the full width, crop top and bottom.
            scaleY = textureAspect / screenAspect;
        }
        else
        {
            // Screen is narrower than the image (portrait): use the full height, crop the sides.
            scaleX = screenAspect / textureAspect;
        }

        // Show only that part of the image, centred left-to-right, and scroll it vertically.
        mat.mainTextureScale = new Vector2(scaleX, scaleY);
        mat.mainTextureOffset = new Vector2((1f - scaleX) / 2f, offset);
    }
}