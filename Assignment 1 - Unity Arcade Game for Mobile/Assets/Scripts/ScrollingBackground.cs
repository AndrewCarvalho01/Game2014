using UnityEngine;

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
        FitToScreen();
        // Increasing the Y offset makes the image appear to move down.
        offset = Mathf.Repeat(offset + scrollSpeed * Time.deltaTime, 1f);
        mat.mainTextureOffset = new Vector2(0f, offset);
    }

    private void FitToScreen()
    {
        // Size of the area the camera sees, in world units.
        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;

        // Cover the whole view and centers the camera
        transform.localScale = new Vector3(width, height, 1f);
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);

        // Repeat the texture across the width so it keeps its shape instead of being stretched. 
        Texture tex = mat.mainTexture;
        float textureAspect = (float)tex.width / tex.height;
        float tilesAcross = (width / height) / textureAspect;
        mat.mainTextureScale = new Vector2(tilesAcross, 1f);
    }
}