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
    public float edgePadding = 10f;
    public float flashStartDistance = 10f;
    public float flashInterval = 0.3f;

    [Header("Position Locking")]
    public bool lockRockYAxis = true;
    public bool lockBirdXAxis = true;
    public float lockedPositionOffset = 50f;

    [Header("Screen Entry Detection")]
    public float screenEntryThreshold = 0.05f;
    public float destroyDelayAfterScreenEntry = 0.05f; // Destroy 1 second after entering screen
    private Dictionary<Transform, bool> obstacleOnScreen = new Dictionary<Transform, bool>();
    private Dictionary<Transform, Coroutine> destroyCoroutines = new Dictionary<Transform, Coroutine>();

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
        CleanUpDestroyedData();

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
                // Update position and check screen entry
                UpdateIndicatorPosition(indicator, obstacle);

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
            CleanupObstacleData(obstacle);
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

        void UpdateIndicatorPosition(RectTransform indicator, Transform obstacle)
        {
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(obstacle.position);
            bool isOnScreen = IsOnScreen(viewportPoint);

            // Initialize tracking if new obstacle
            if (!obstacleOnScreen.ContainsKey(obstacle))
            {
                obstacleOnScreen[obstacle] = false;
            }

            // Check if obstacle just entered screen
            if (isOnScreen && !obstacleOnScreen[obstacle])
            {
                // Start destroy countdown
                if (destroyCoroutines.ContainsKey(obstacle))
                {
                    StopCoroutine(destroyCoroutines[obstacle]);
                }
                destroyCoroutines[obstacle] = StartCoroutine(DestroyIndicatorAfterDelay(indicator, obstacle, destroyDelayAfterScreenEntry));
            }
            obstacleOnScreen[obstacle] = isOnScreen;

            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                boundaryPanel,
                mainCamera.WorldToScreenPoint(obstacle.position),
                null,
                out localPos);

            // Apply boundary clamping
            localPos.x = Mathf.Clamp(localPos.x, -panelHalfSize.x + edgePadding, panelHalfSize.x - edgePadding);
            localPos.y = Mathf.Clamp(localPos.y, -panelHalfSize.y + edgePadding, panelHalfSize.y - edgePadding);

            indicator.anchoredPosition = localPos;
            
        }


        IEnumerator DestroyIndicatorAfterDelay(RectTransform indicator, Transform obstacle, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (indicator != null)
            {
                Destroy(indicator.gameObject);
            }
            CleanupObstacleData(obstacle);
        }

        bool IsOnScreen(Vector3 viewportPoint)
        {
            return viewportPoint.z > 0 &&
                   viewportPoint.x > screenEntryThreshold &&
                   viewportPoint.x < 1 - screenEntryThreshold &&
                   viewportPoint.y > screenEntryThreshold &&
                   viewportPoint.y < 1 - screenEntryThreshold;
        }


        void CleanUpDestroyedData()
        {
            // Clean up obstacles that were destroyed
            List<Transform> destroyedObstacles = new List<Transform>();
            foreach (var obstacle in activeIndicators.Keys)
            {
                if (obstacle == null) // Obstacle was destroyed
                {
                    destroyedObstacles.Add(obstacle);
                }
            }

            // Clean up indicators that were destroyed
            List<Transform> destroyedIndicators = new List<Transform>();
            foreach (var pair in activeIndicators)
            {
                if (pair.Value == null) // Indicator was destroyed
                {
                    destroyedIndicators.Add(pair.Key);
                }
            }

            // Remove all destroyed entries
            foreach (Transform obstacle in destroyedObstacles)
            {
                CleanupObstacleData(obstacle);
            }
            foreach (Transform obstacle in destroyedIndicators)
            {
                CleanupObstacleData(obstacle);
            }
        }

        void CleanupObstacleData(Transform obstacle)
        {
            if (obstacle != null)
            {
                // Stop any running destroy coroutine
                if (destroyCoroutines.TryGetValue(obstacle, out Coroutine coroutine))
                {
                    StopCoroutine(coroutine);
                    destroyCoroutines.Remove(obstacle);
                }
            }

            // Remove from all dictionaries
            obstacleOnScreen.Remove(obstacle);
            activeIndicators.Remove(obstacle);
            destroyCoroutines.Remove(obstacle);
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
