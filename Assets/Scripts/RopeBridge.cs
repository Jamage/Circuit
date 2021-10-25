using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeBridge : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    [SerializeField] private float ropeSegLength = 0.25f;
    [SerializeField] private int segmentCount = 35;
    [SerializeField] float lineWidth = 0.1f;
    [SerializeField] Vector2 forceGravity = new Vector2(0, -1);
    [SerializeField] bool useGravity = true;
    [SerializeField] int precision = 40;
    [SerializeField, Tooltip("Ensure amount divides segment count equally")] private List<Vector3> anchorPoints;
    private Transform parentTransform;

    private void Awake()
    {
        if (parentTransform == null)
            parentTransform = GetComponentInParent<Transform>();
    }


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = transform.position;

        for (int i = 0; i < segmentCount; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLength;
        }
    }

    private void Update()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentCount];
        for (int i = 0; i < this.segmentCount; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow - (Vector2)parentTransform.position;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    private void Simulate()
    {
        //SIMULATION
        for (int i = 0; i < this.segmentCount; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            if (useGravity)
                firstSegment.posNow += forceGravity * Time.deltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < precision; i++)
        {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        SetAnchors();

        for (int i = 0; i < this.segmentCount - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = dist - ropeSegLength;
            Vector2 changeDir = Vector2.zero;

            changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * .5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * .5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else //only move second point, not first
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void SetAnchors()
    {
        if (anchorPoints.Count == 0)
            return;

        RopeSegment[] anchorSegments = new RopeSegment[anchorPoints.Count];
        for (int index = 0; index < anchorPoints.Count - 1; index++)
        {
            int segment = (int)(segmentCount * ((float)index / (float)(anchorPoints.Count - 1)));
            anchorSegments[index] = this.ropeSegments[segment];
            anchorSegments[index].posNow = anchorPoints[index] + parentTransform.position;
            ropeSegments[segment] = anchorSegments[index];
        }

        anchorSegments[anchorPoints.Count - 1] = ropeSegments[segmentCount - 1];
        anchorSegments[anchorPoints.Count - 1].posNow = anchorPoints[anchorPoints.Count - 1] + parentTransform.position;
        ropeSegments[segmentCount - 1] = anchorSegments[anchorPoints.Count - 1];
    }

    private void OnValidate()
    {
        lineWidth = Mathf.Clamp(lineWidth, .01f, 10);
        segmentCount = Mathf.Clamp(segmentCount, 1, int.MaxValue);
        ropeSegLength = Mathf.Clamp(ropeSegLength, .001f, float.MaxValue);
        precision = Mathf.Clamp(precision, 1, 100);
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }

    internal void SetupFrom(LinePanelType panelType)
    {
        anchorPoints.Clear();
        if (panelType.HasFlag(LinePanelType.One))
            anchorPoints.Add(new Vector3(0, 0, 0));
        if (panelType.HasFlag(LinePanelType.Two))
            anchorPoints.Add(new Vector3(1, 0, 0));
        if (panelType.HasFlag(LinePanelType.Three))
            anchorPoints.Add(new Vector3(2, 0, 0));
        if (panelType.HasFlag(LinePanelType.Four))
            anchorPoints.Add(new Vector3(0, -0.5f, 0));
        if (panelType.HasFlag(LinePanelType.Six))
            anchorPoints.Add(new Vector3(2, -0.5f, 0));
        if (panelType.HasFlag(LinePanelType.Seven))
            anchorPoints.Add(new Vector3(0, -1, 0));
        if (panelType.HasFlag(LinePanelType.Eight))
            anchorPoints.Add(new Vector3(1, -1, 0));
        if (panelType.HasFlag(LinePanelType.Nine))
            anchorPoints.Add(new Vector3(2, -1, 0));

        for(int i = 1; i < anchorPoints.Count; i += 2)
            anchorPoints.Insert(i, new Vector3(1, -0.5f, 0));
    }
}
