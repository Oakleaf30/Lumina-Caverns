using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private RectTransform canvasRect; // the root Canvas's RectTransform
    [SerializeField] private RectTransform panelRect;  // the tooltip panel's own RectTransform

    private void Awake() => Instance = this;

    public void Show(string header, string body, Vector2 screenPos)
    {
        panel.SetActive(true);
        headerText.text = header;
        bodyText.text = body;
        PositionNearCursor(screenPos);
    }

    public void Hide() => panel.SetActive(false);

    private void PositionNearCursor(Vector2 screenPos)
    {
        Vector2 offset = new Vector2(20, -20);
        Vector2 targetPos = screenPos + offset;

        // Half-width/height, since RectTransform positions are measured from center
        float halfWidth = panelRect.rect.width / 2f;
        float halfHeight = panelRect.rect.height / 2f;

        float canvasHalfWidth = canvasRect.rect.width / 2f;
        float canvasHalfHeight = canvasRect.rect.height / 2f;

        // Clamp X so the panel's left/right edges stay inside the canvas
        targetPos.x = Mathf.Clamp(targetPos.x, -canvasHalfWidth + halfWidth, canvasHalfWidth - halfWidth);

        // Clamp Y so the panel's top/bottom edges stay inside the canvas
        targetPos.y = Mathf.Clamp(targetPos.y, -canvasHalfHeight + halfHeight, canvasHalfHeight - halfHeight);

        panel.transform.position = targetPos;
    }
}