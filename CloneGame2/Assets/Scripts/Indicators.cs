using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicators : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform indicatorParent;
    public GameObject rockIndicatorPrefab;
    public GameObject birdIndicatorPrefab;

    [Header("Settings")]
    public float edgePadding = 50f;
    public float indicatorLifetime = 4f;
    [Range(0.1f, 1f)] public float edgeBuffer = 0.9f; // 90% of screen

    private Camera mainCamera;
    private Transform player;

    void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnEnable()
    {
        Level_Obstacles.OnObstacleSpawned += HandleObstacleSpawned;
        
    }

    void OnDisable()
    {
        Level_Obstacles.OnObstacleSpawned -= HandleObstacleSpawned;
    }



    void HandleObstacleSpawned(Vector3 spawnPosition, Level_Obstacles.ObstacleType type)
    {
        // Create appropriate indicator
        GameObject indicatorPrefab = type == Level_Obstacles.ObstacleType.Rock
            ? rockIndicatorPrefab
            : birdIndicatorPrefab;

        GameObject indicator = Instantiate(indicatorPrefab, indicatorParent);
        StartCoroutine(UpdateIndicatorPosition(indicator.GetComponent<RectTransform>(), spawnPosition));
        Destroy(indicator, indicatorLifetime);
    }

    IEnumerator UpdateIndicatorPosition(RectTransform indicator, Vector3 worldPosition)
    {
        while (indicator != null)
        {
            // Convert world position to screen position
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPosition);

            // Check if obstacle is on screen
            bool onScreen = screenPos.z > 0 &&
                          screenPos.x > 0 && screenPos.x < Screen.width &&
                          screenPos.y > 0 && screenPos.y < Screen.height;

            if (onScreen)
            {
                // If on screen, show indicator at obstacle position
                indicator.gameObject.SetActive(true);
                indicator.anchoredPosition = screenPos - new Vector3(Screen.width / 2, Screen.height / 2);
            }
            else
            {
                // If off screen, show at screen edge pointing to obstacle
                indicator.gameObject.SetActive(true);
                Vector3 dir = (worldPosition - player.position).normalized;
                dir.z = 0;
                Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
                Vector3 screenEdge = GetScreenEdgePosition(dir, screenCenter);
                indicator.anchoredPosition = screenEdge - screenCenter;

                // Rotate arrow to point toward obstacle
                if (indicator.TryGetComponent<RectTransform>(out var rt))
                {
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    rt.localEulerAngles = new Vector3(0, 0, angle);
                }
            }

            yield return null;
        }
    }

    Vector3 GetScreenEdgePosition(Vector3 direction, Vector3 screenCenter)
    {
        direction.Normalize();
        float ratio = (float)Screen.width / Screen.height;
        Vector3 multipliedDir = new Vector3(direction.x * ratio, direction.y, 0).normalized;

        float screenRadius = Mathf.Min(Screen.width, Screen.height) / 2 - edgePadding;
        return screenCenter + multipliedDir * screenRadius;
    }

}
