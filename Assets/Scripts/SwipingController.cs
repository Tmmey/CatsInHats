using System.Collections.Generic;
using UnityEngine;


public class SwipingController : MonoBehaviour
{
    /// <summary>
    /// Reference to gesture input manager.
    /// </summary>
    [SerializeField]
    private GestureController gestureController;

    /// <summary>
    /// All active bins.
    /// </summary>
    [SerializeField]
    private Bin[] bins;

    /// <summary>
    /// Minimum cosine of angle between swipe and bin in order to target the given bin.
    /// </summary>
    [SerializeField]
    private float minimumCosineForBinTargetting = 0.75f;

    /// <summary>
    /// Mapping of input IDs to 'grabbed' cat.
    /// </summary>
    private readonly Dictionary<int, Cat> swipeMapping = new Dictionary<int, Cat>();

    private Camera cachedCamera;
    private int castLayerMask;
    private float screenUnitsToWorldUnits;

    protected virtual void Awake()
    {
        castLayerMask = LayerMask.GetMask("Default");
        cachedCamera = Camera.main;

        // Calculate transformation factor from screen units to world units
        if (!cachedCamera.orthographic)
        {
            Debug.LogError("Swiping controller only supports orthographic camera");
        }
        else
        {
            screenUnitsToWorldUnits = (cachedCamera.orthographicSize * 2) / Screen.height;
        }
    }

    private void OnEnable()
    {
        gestureController.PotentiallySwiped += OnDragged;
        gestureController.Swiped += OnSwiped;
        gestureController.Pressed += OnPressed;
    }

    protected virtual void OnDisable()
    {
        gestureController.PotentiallySwiped -= OnDragged;
        gestureController.Swiped -= OnSwiped;
        gestureController.Pressed -= OnPressed;
    }

    private void OnSwiped(SwipeInput input)
    {
        // Try find a grabbed cat for this swipe
        Cat swipedCat;

        if (!swipeMapping.TryGetValue(input.InputId, out swipedCat))
        {
            return;
        }

        swipeMapping.Remove(input.InputId);

        // Launch cat at target if it hasn't been launched yet
        if (swipedCat == null ||
            swipedCat.HasFlung)
        {
            return;
        }

        // Find bin in swipe direction
        Bin targetBin = FindTargetBinForSwipe(input);
        Vector2 targetPosition = targetBin != null
            ? targetBin.transform.position
            : swipedCat.transform.position + (Vector3)input.SwipeDirection;

        swipedCat.LaunchAt(targetPosition, input.SwipeVelocity * screenUnitsToWorldUnits);
    }

    private void OnPressed(SwipeInput input)
    {
        // Make sure that the swipe mapping doesn't contain this swipe
        swipeMapping.Remove(input.InputId);

        // Try also find grabbed cat on first press
        Vector2 worldCurrent = cachedCamera.ScreenToWorldPoint(input.EndPosition);

        // Try find cat to grab for this touch
        Collider2D collider = Physics2D.OverlapPoint(worldCurrent, castLayerMask);
        if (collider != null)
        {
            // Try find a Cat object on this hit component
            var cat = collider.GetComponent<Cat>();
            if (cat != null &&
                !cat.HasFlung)
            {
                // Remember that this swipe went over this object
                swipeMapping[input.InputId] = cat;
            }
        }
    }

    private void OnDragged(SwipeInput input)
    {
        // If this input's already picked up some cat, ignore this
        if (swipeMapping.ContainsKey(input.InputId))
        {
            return;
        }

        Vector2 worldPrevious = cachedCamera.ScreenToWorldPoint(input.PreviousPosition);
        Vector2 worldCurrent = cachedCamera.ScreenToWorldPoint(input.EndPosition);

        //Try find cat to grab for this swipe
        RaycastHit2D hit = Physics2D.Linecast(worldPrevious, worldCurrent, castLayerMask);
        if (hit.collider != null)
        {
            // Try find a Cat object on this hit component
            var cat = hit.collider.GetComponent<Cat>();
            if (cat != null &&
                !cat.HasFlung)
            {
                // Remember that this swipe went over this object
                swipeMapping[input.InputId] = cat;
            }
        }
    }

    /// <summary>
    /// Given a swipe, find the nearest bin (within reason) of the swipe direction.
    /// </summary>
    private Bin FindTargetBinForSwipe(SwipeInput input)
    {
        Vector2 swipeEndPosition = cachedCamera.ScreenToWorldPoint(input.EndPosition);
        var biggestDot = 0.0f;
        Bin targetBin = null;

        foreach (Bin bin in bins)
        {
            Vector2 toTargetBin = (Vector2)bin.transform.position - swipeEndPosition;
            float toTargetDot = Vector2.Dot(input.SwipeDirection, toTargetBin.normalized);

            if (toTargetDot > biggestDot &&
                toTargetDot >= minimumCosineForBinTargetting)
            {
                biggestDot = toTargetDot;
                targetBin = bin;
            }
        }
        return targetBin;
    }
}

