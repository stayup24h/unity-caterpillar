using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class ShadowEffect : MonoBehaviour
{
    [System.Serializable]
    public class Property
    {
        public Vector3 positionOffset = new Vector3(0.1f, -0.1f);
        [Min(0)] public float scaleOffset = 1;
        public Color color = new Color(0, 0, 0, 0.3f);
    }

    [SerializeField] private Property property;
    private GameObject shadow;
    private SpriteRenderer shadowSprite;

    void Start()
    {
        shadow = new GameObject("Shadow");
        shadow.transform.parent = transform;

        shadow.transform.localPosition = property.positionOffset;
        shadow.transform.localRotation = Quaternion.identity;
        shadow.transform.localScale = new Vector3 (property.scaleOffset, property.scaleOffset, 1);

        Material material = new Material(Shader.Find("UI/Unlit/Transparent"));
        material.color = property.color;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        shadowSprite = shadow.AddComponent<SpriteRenderer>();
        shadowSprite.sprite = renderer.sprite;
        shadowSprite.material = material;

        shadowSprite.sortingLayerName = renderer.sortingLayerName;
        shadowSprite.sortingOrder = renderer.sortingOrder - 1;
    }

    void LateUpdate()
    {
        shadow.transform.position = transform.position + property.positionOffset;
        shadow.transform.localScale = new Vector3(property.scaleOffset, property.scaleOffset, 1);
        shadowSprite.material.color = property.color;
    }
}
