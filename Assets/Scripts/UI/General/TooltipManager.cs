using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private RectTransform canvasRect; // the root Canvas's RectTransform
    [SerializeField] private RectTransform panelRect;  // the tooltip panel's own RectTransform
    [SerializeField] private float maxWidth = 550f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Show(string header, string body, Vector2 screenPos)
    {
        panel.SetActive(true);
        headerText.text = header;
        bodyText.text = body;
        PositionNearCursor(screenPos);

        float preferredWidth = bodyText.GetPreferredValues(body, maxWidth, 0).x;
        bodyText.textWrappingMode = preferredWidth > maxWidth
            ? TextWrappingModes.Normal
            : TextWrappingModes.NoWrap;

        LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);
    }

    public void Hide() => panel.SetActive(false);

    private void PositionNearCursor(Vector2 screenPos)
    {
        // Convert screen point -> local point inside the canvas RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenPos, null, out Vector2 localPoint);

        Vector2 offset = new Vector2(20, -20);

        // Flip offset if the tooltip would overflow that side of the canvas
        float halfWidth = panelRect.rect.width / 2f;
        float halfHeight = panelRect.rect.height / 2f;
        float canvasHalfWidth = canvasRect.rect.width / 2f;
        float canvasHalfHeight = canvasRect.rect.height / 2f;

        if (localPoint.x + offset.x + panelRect.rect.width > canvasHalfWidth)
            offset.x = -20 - panelRect.rect.width; // flip to the left of cursor

        if (localPoint.y + offset.y - panelRect.rect.height < -canvasHalfHeight)
            offset.y = 20 + panelRect.rect.height; // flip above cursor

        Vector2 targetPos = localPoint + offset;

        targetPos.x = Mathf.Clamp(targetPos.x, -canvasHalfWidth + halfWidth, canvasHalfWidth - halfWidth);
        targetPos.y = Mathf.Clamp(targetPos.y, -canvasHalfHeight + halfHeight, canvasHalfHeight - halfHeight);

        panelRect.anchoredPosition = targetPos;
    }
}