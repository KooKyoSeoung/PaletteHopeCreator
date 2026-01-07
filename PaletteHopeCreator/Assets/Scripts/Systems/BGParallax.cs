using UnityEngine;

public class BGParallax : MonoBehaviour
{
    public float animationSpeed = 0.05f;
    private MeshRenderer meshRenderer;
    private Material materialInstance;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        materialInstance = meshRenderer.material;
    }

    void Update()
    {
        materialInstance.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
    }
}
