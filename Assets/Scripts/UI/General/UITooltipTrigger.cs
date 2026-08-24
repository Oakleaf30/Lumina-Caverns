using UnityEngine;
using UnityEngine.EventSystems;

public class UITooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string header;
    [TextArea][SerializeField] private string body;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.Show(header, body, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.Hide();
    }
}