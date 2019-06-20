using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class CameraZoomOutWithShipSize : MonoBehaviour
{
    [SerializeField]
    private ShipStructure structure;
    private CameraFollowMovement follow;

    [SerializeField]
    private float smoothing = 5.0f;

    private Camera camera;
    private float startOrthoSize;
    private float followSmoothing = 0.0f;

    void Start()
    {
        camera = GetComponent<Camera>();
        follow = GetComponent<CameraFollowMovement>();
        startOrthoSize = camera.orthographicSize;
    }

    void Update()
    {
        float targetOrthoSize = startOrthoSize + (startOrthoSize * structure.Planks.Count / 100.0f);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetOrthoSize, smoothing * Time.deltaTime);

        if(followSmoothing == 0.0f)
        {
            followSmoothing = follow.Smoothing;
        }
        float smoothingTarget = followSmoothing + (followSmoothing * (structure.Oars.Count + (structure.Stern ? 0 : 10) + (structure.Bow ? 0 : 10)) / 100.0f);
        follow.Smoothing = Mathf.Lerp(follow.Smoothing, smoothingTarget, smoothing * Time.deltaTime);
    }
}
