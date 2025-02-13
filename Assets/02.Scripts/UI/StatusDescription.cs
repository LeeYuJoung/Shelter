using UnityEngine;
using UnityEngine.EventSystems;

namespace yjlee.UI
{
    public class StatusDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject descriptionImage;

        public void OnPointerEnter(PointerEventData eventData)
        {
            descriptionImage.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            descriptionImage.SetActive(false);
        }
    }
}