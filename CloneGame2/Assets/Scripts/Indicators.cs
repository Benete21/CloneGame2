using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicators : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 40f;
    public float sphereCastRadius = 4f;
    public float maxDetectionDistance = 70f;
    public LayerMask obstacleLayer;
    public Vector3[] detectionDirections = {
        Vector3.up,        // Top
        Vector3.right,     // Right
        Vector3.left       // Left
    };

    [Header("Indicator Settings")]
    public RectTransform boundaryPanel;
    public GameObject rockIndicatorPrefab;
    public GameObject birdIndicatorPrefab;
    public float indicatorLifetime = 5f;
    public float edgePadding = 30f;
    public float flashStartDistance = 10f;
    public float flashInterval = 0.3f;

    private Camera mainCamera;
    private Transform player;
    private Vector2 panelHalfSize;
    private Dictionary<Transform, GameObject> activeIndicators = new Dictionary<Transform, GameObject>();

    void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        panelHalfSize = boundaryPanel.rect.size * 0.5f;
    }

    void Update()
    {
        CheckForObstacles();
        CleanUpDestroyedIndicators();
    }

    void CleanUpDestroyedIndicators()
    {
        List<Transform> toRemove = new List<Transform>();

        foreach (var pair in activeIndicators)
        {
            if (pair.Key == null || pair.Value == null) // Obstacle or indicator was destroyed
            {
                toRemove.Add(pair.Key);
            }
            else if (Vector3.Distance(player.position, pair.Key.position) > detectionRadius * 1.2f)
            {
                Destroy(pair.Value);
                toRemove.Add(pair.Key);
            }
        }

        foreach (Transform key in toRemove)
        {
            activeIndicators.Remove(key);
        }
    }


    void CheckForObstacles()
    {
        foreach (Vector3 direction in detectionDirections)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                player.position + direction * 2f, // Offset from player
                sphereCastRadius,
                direction,
                maxDetectionDistance,
                obstacleLayer);

            foreach (RaycastHit hit in hits)
            {
                if (Vector3.Distance(player.position, hit.point) <= detectionRadius)
                {
                    if (!activeIndicators.ContainsKey(hit.transform))
                    {
                        CreateIndicator(hit.transform);
                    }
                }
            }
        }

        void CreateIndicator(Transform obstacle)
        {
            Level_Obstacles.ObstacleType type = obstacle.CompareTag("Rocks")
                ? Level_Obstacles.ObstacleType.Rock
                : Level_Obstacles.ObstacleType.Bird;

            GameObject indicator = Instantiate(GetPrefab(type), boundaryPanel);
            activeIndicators.Add(obstacle, indicator);
            StartCoroutine(UpdateIndicator(indicator.GetComponent<RectTransform>(), obstacle));
        }

        IEnumerator UpdateIndicator(RectTransform indicator, Transform obstacle)
        {
            Image indicatorImage = indicator.GetComponent<Image>();
            Color originalColor = indicatorImage.color;
            bool shouldFlash = false;

            while (obstacle != null && Vector3.Distance(player.position, obstacle.position) <= detectionRadius * 1.2f)
            {
                // Update position
                UpdateIndicatorPosition(indicator, obstacle.position);

                // Check for flashing
                float distance = Vector3.Distance(player.position, obstacle.position);
                if (distance <= flashStartDistance && !shouldFlash)
                {
                    shouldFlash = true;
                    StartCoroutine(FlashIndicator(indicatorImage));
                }
                else if (distance > flashStartDistance && shouldFlash)
                {
                    shouldFlash = false;
                    indicatorImage.color = originalColor;
                }

                yield return null;
            }

            // Clean up
            if (indicator != null)
            {
                Destroy(indicator.gameObject);
            }
            activeIndicators.Remove(obstacle);
        }

        IEnumerator FlashIndicator(Image image)
        {
            Color originalColor = image.color;
            while (image != null)
            {
                image.gameObject.SetActive(false);
                yield return new WaitForSeconds(flashInterval);
                image.gameObject.SetActive(true);
                yield return new WaitForSeconds(flashInterval);
            }
        }

        void UpdateIndicatorPosition(RectTransform indicator, Vector3 worldPos)
        {
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(worldPos);
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                boundaryPanel,
                screenPoint,
                null,
                out localPos);

            localPos.x = Mathf.Clamp(localPos.x, -panelHalfSize.x + edgePadding, panelHalfSize.x - edgePadding);
            localPos.y = Mathf.Clamp(localPos.y, -panelHalfSize.y + edgePadding, panelHalfSize.y - edgePadding);

            indicator.anchoredPosition = localPos;

            if (screenPoint.z < 0 ||
                screenPoint.x < 0 || screenPoint.x > Screen.width ||
                screenPoint.y < 0 || screenPoint.y > Screen.height)
            {
                Vector3 dir = (worldPos - player.position).normalized;
                indicator.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            }
            else
            {
                indicator.localEulerAngles = Vector3.zero;
            }
        }

        GameObject GetPrefab(Level_Obstacles.ObstacleType type) =>
            type == Level_Obstacles.ObstacleType.Rock ? rockIndicatorPrefab : birdIndicatorPrefab;

        void OnDrawGizmosSelected()
        {
            if (player != null)
            {

                foreach (Vector3 direction in detectionDirections)
                {
                    Vector3 origin = player.position + direction * 2f;
                    Gizmos.DrawWireSphere(origin, sphereCastRadius);
                    Gizmos.DrawLine(origin, origin + direction * maxDetectionDistance);

                    // Visualize the actual detection radius
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(player.position, detectionRadius);
                    Gizmos.color = Color.yellow;
                }
            }
        }

    }
}
