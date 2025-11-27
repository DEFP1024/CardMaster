using UnityEngine;
using UnityEngine.EventSystems;

public class CardTargetManager : Singleton<CardTargetManager>
{
    [SerializeField] private RectTransform arrow;
    [SerializeField] private Canvas canvas;

    private CardObj currentCard;
    private bool isTargeting = false;

    private new void Awake()
    {
        if (arrow != null)
            arrow.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isTargeting == false || currentCard == null)
            return;

        UpdateArrow();

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            TrySelectEnemy();
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelTargeting();
        }
    }

    public void StartEnemyTargeting(CardObj card)
    {
        if (card == null)
            return;

        currentCard = card;
        isTargeting = true;

        if (arrow != null)
        {
            arrow.gameObject.SetActive(true);
            UpdateArrow();
        }
    }

    public void CancelTargeting()
    {
        isTargeting = false;
        currentCard = null;

        if (arrow != null)
            arrow.gameObject.SetActive(false);
    }

    private void UpdateArrow()
    {
        if (arrow == null || canvas == null || currentCard == null)
            return;

        RectTransform canvasRect = canvas.transform as RectTransform;

        Vector3 cardScreenPos = RectTransformUtility.WorldToScreenPoint(null, currentCard.transform.position);
        Vector3 mouseScreenPos = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, cardScreenPos, null, out Vector2 cardLocalPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mouseScreenPos, null, out Vector2 mouseLocalPos);

        arrow.anchoredPosition = cardLocalPos;

        Vector2 dir = mouseLocalPos - cardLocalPos;
        float distance = dir.magnitude;

        Vector2 size = arrow.sizeDelta;
        size.x = distance;
        arrow.sizeDelta = size;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrow.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void TrySelectEnemy()
    {
        Camera worldCam = Camera.main;
        if (worldCam == null)
            return;

        Vector3 worldPos = worldCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

        RaycastHit2D hit = Physics2D.Raycast(worldPos2D, Vector2.zero);
        if (hit.collider == null)
            return;

        EnemyUnit enemy = hit.collider.GetComponent<EnemyUnit>();
        if (enemy == null)
            return;

        GameManager.Instance.TryPlayCard(currentCard, enemy);
        CancelTargeting();
    }
}
