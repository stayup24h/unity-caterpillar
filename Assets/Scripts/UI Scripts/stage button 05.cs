using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageButton05Script : MonoBehaviour
, IPointerClickHandler

{
    public void OnPointerClick(PointerEventData eventData)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage 05");
    }
}