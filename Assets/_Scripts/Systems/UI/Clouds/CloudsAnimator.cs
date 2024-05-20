using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CloudsAnimator : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    public int maxNumberOfClouds;

    public float minIntervalBetweenClouds;
    public float maxIntervalBetweenClouds;

    public RectTransform leftLimit;
    public RectTransform rightLimit;

    private GameObject[] _clouds;
    private List<Tuple<GameObject, Vector2>> _cloudsInitialPos =
        new List<Tuple<GameObject, Vector2>>();

    private bool _nextCloud;
    private int _currentNumberOfClouds;
    private List<GameObject> _currentCloudsBeingAnimated = new List<GameObject>();

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name.Contains("Cloud"))
            {
                RectTransform rt = child.GetComponent<RectTransform>();
                _cloudsInitialPos.Add(
                    new Tuple<GameObject, Vector2>(
                        child,
                        new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y)
                    )
                );
            }
        }

        _clouds = new GameObject[_cloudsInitialPos.Count];

        for (int j = 0; j < _cloudsInitialPos.Count; j++)
            _clouds[j] = _cloudsInitialPos[j].Item1;
    }

    void Start()
    {
        _nextCloud = true;
        StartCoroutine(CloudsAnimatorHandler());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        for (int i = 0; i < _clouds.Length; i++)
        {
            _clouds[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
    }

    private IEnumerator CloudsAnimatorHandler()
    {
        float intervalBetweenClouds = 0.2f;

        while (true)
        {
            yield return new WaitForSeconds(intervalBetweenClouds);

            if (_nextCloud)
            {
                _currentNumberOfClouds++;

                if (_currentNumberOfClouds == maxNumberOfClouds)
                {
                    _nextCloud = false;
                    intervalBetweenClouds = 1f;
                }
                else
                {
                    intervalBetweenClouds = UnityEngine
                        .Random
                        .Range(minIntervalBetweenClouds, maxIntervalBetweenClouds);
                }

                StartCoroutine(CloudsAnimation());
            }
        }
    }

    private IEnumerator CloudsAnimation()
    {
        List<GameObject> validClouds = _clouds
            .Where(cloud => !_currentCloudsBeingAnimated.Contains(cloud))
            .ToList();

        GameObject cloud = validClouds[UnityEngine.Random.Range(0, validClouds.Count)];
        _currentCloudsBeingAnimated.Add(cloud);

        RectTransform cloudRT = cloud.GetComponent<RectTransform>();
        Image cloudImage = cloud.GetComponent<Image>();
        Vector2 nextInitialPos = _cloudsInitialPos[
            UnityEngine.Random.Range(0, _cloudsInitialPos.Count)
        ].Item2;

        float interval = 0.025f;
        float widthOffsetFactor = 0.5f;
        bool startedFadedOut = false;
        float speed = UnityEngine.Random.Range(minSpeed, maxSpeed);

        StartCoroutine(FadeCloud(cloudImage, true));

        if (cloudRT.anchoredPosition.x < 0)
        {
            while (cloudRT.anchoredPosition.x < rightLimit.anchoredPosition.x)
            {
                yield return new WaitForSeconds(interval);
                cloudRT.anchoredPosition = new Vector2(
                    cloudRT.anchoredPosition.x + speed,
                    cloudRT.anchoredPosition.y
                );

                if (
                    !startedFadedOut
                    && cloudRT.anchoredPosition.x
                        > rightLimit.anchoredPosition.x - cloudRT.sizeDelta.x * widthOffsetFactor
                )
                {
                    startedFadedOut = true;
                    StartCoroutine(FadeCloud(cloudImage, false));
                }
            }
        }
        else
        {
            while (cloudRT.anchoredPosition.x > leftLimit.anchoredPosition.x)
            {
                yield return new WaitForSeconds(interval);
                cloudRT.anchoredPosition = new Vector2(
                    cloudRT.anchoredPosition.x - speed,
                    cloudRT.anchoredPosition.y
                );

                if (
                    !startedFadedOut
                    && cloudRT.anchoredPosition.x
                        < leftLimit.anchoredPosition.x + cloudRT.sizeDelta.x * widthOffsetFactor
                )
                {
                    startedFadedOut = true;
                    StartCoroutine(FadeCloud(cloudImage, false));
                }
            }
        }

        cloudRT.anchoredPosition = new Vector2(nextInitialPos.x, nextInitialPos.y);
        _currentNumberOfClouds--;

        if (_currentNumberOfClouds < maxNumberOfClouds)
            _nextCloud = true;

        _currentCloudsBeingAnimated.Remove(cloud);
    }

    private IEnumerator FadeCloud(Image cloudImage, bool fadeIn)
    {
        float alphaFactor = fadeIn ? 0.1f : -0.1f;
        float target = fadeIn ? 1 : 0;

        while (cloudImage.color.a != target)
        {
            yield return new WaitForSeconds(0.08f);
            cloudImage.color = new Color(
                255,
                255,
                255,
                Mathf.Clamp(cloudImage.color.a + alphaFactor, 0, 1)
            );
        }
    }
}
