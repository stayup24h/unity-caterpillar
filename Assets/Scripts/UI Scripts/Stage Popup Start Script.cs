using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StagePopupStartScript : MonoBehaviour
, IPointerClickHandler

{
    public GameObject stagePanel;
    public void OnPointerClick(PointerEventData eventData)
    {
        stagePanel.SetActive(true);
    }
}
