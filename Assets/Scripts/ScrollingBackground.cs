using UnityEngine;
public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.1f;
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
        // Scroll the texture downwards. The offset is wrapped around to keep it between 0 and 1.
        offset = Mathf.Repeat(offset + scrollSpeed * Time.deltaTime, 1f);
        mat.mainTextureOffset = new Vector2(0f, offset);
    }
    private void FitToScreen()
    {
        // Size of the area the camera sees, in world units.
        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;
        // Cover the whole view, centred on the camera. Keep the quad's own Z, so it stays behind the ships.
        transform.localScale = new Vector3(width, height, 1f);
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
        // Repeat the texture across the width so the stars keep their shape
        // instead of being stretched when the screen gets wider.
        Texture tex = mat.mainTexture;
        float textureAspect = (float)tex.width / tex.height;
        float tilesAcross = (width / height) / textureAspect;
        mat.mainTextureScale = new Vector2(tilesAcross, 1f);
    }
}