using System;
using UnityEngine;
using UnityEngine.UI;

public class ShadowEffect : MonoBehaviour
{
    [System.Serializable]
    public class Property
    {
        public bool isUI = false;
        public Vector3 positionOffset = new Vector3(0.1f, -0.1f);
        [Min(0)] public float scaleOffset = 1;
        public Color color = new Color(0, 0, 0, 0.3f);
    }

    [SerializeField] private Property property;
    private GameObject shadow;

    void Start()
    {
        shadow = new GameObject("Shadow");
        shadow.transform.parent = transform;

        if (property.isUI)
        {
            Image image = GetComponent<Image>();
            Image shadowImage = shadow.AddComponent<Image>();
            shadowImage.sprite = image.sprite;
            shadowImage.color = property.color;

            RectTransform rect = GetComponent<RectTransform>();
            SetRectTransform(shadow.GetComponent<RectTransform>(), rect, property.positionOffset, property.scaleOffset);

            GameObject replication = new GameObject(name);
            replication.transform.parent = transform;
            Image replImage = replication.AddComponent<Image>();
            replImage.sprite = image.sprite;

            RectTransform replRect = replImage.GetComponent<RectTransform>();
            SetRectTransform(replRect, rect);
            
            Color color = image.color;
            color.a = 0;
            image.color = color;

            shadow.GetComponent<RectTransform>().SetAsFirstSibling();
        }
        else
        {
            shadow.transform.localPosition = property.positionOffset;
            shadow.transform.localRotation = Quaternion.identity;
            shadow.transform.localScale = new Vector3(property.scaleOffset, property.scaleOffset, 1);

            Material material = new Material(Shader.Find("UI/Unlit/Transparent"));
            material.color = property.color;

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            SpriteRenderer shadowSprite = shadow.AddComponent<SpriteRenderer>();
            shadowSprite.sprite = renderer.sprite;
            shadowSprite.material = material;

            shadowSprite.sortingLayerName = renderer.sortingLayerName;
            shadowSprite.sortingOrder = renderer.sortingOrder - 1;
        }
    }

    void LateUpdate()
    {
        if (property.isUI)
        {
            SetRectTransform(shadow.GetComponent<RectTransform>(), GetComponent<RectTransform>(), property.positionOffset, property.scaleOffset);
            shadow.GetComponent<Image>().color = property.color;
        }
        else
        {
            shadow.transform.position = transform.position + property.positionOffset;
            shadow.transform.localScale = new Vector3(property.scaleOffset, property.scaleOffset, 1);
            shadow.GetComponent<SpriteRenderer>().material.color = property.color;
        }
    }

    private void SetRectTransform(RectTransform target, RectTransform source, Vector3 positionOffset = new Vector3(), float scaleOffset = 1)
    {
        if (positionOffset == new Vector3()) positionOffset = Vector3.zero;

        target.anchorMin = source.anchorMin;
        target.anchorMax = source.anchorMax;
        target.position = source.position + positionOffset;
        target.sizeDelta = source.sizeDelta;
        target.pivot = source.pivot;
        target.localRotation = source.localRotation;
        target.localScale = source.localScale * scaleOffset;
    }
}
