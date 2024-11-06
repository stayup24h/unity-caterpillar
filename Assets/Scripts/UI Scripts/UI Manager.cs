using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
