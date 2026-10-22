using UnityEngine;
public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.1f;

    private Material mat;
    private float offset;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Increasing the Y offset makes the image appear to move down.
        // Repeat keeps the number between 0 and 1 so it never grows forever.
        offset = Mathf.Repeat(offset + scrollSpeed * Time.deltaTime, 1f);
        mat.mainTextureOffset = new Vector2(0f, offset);
    }
}