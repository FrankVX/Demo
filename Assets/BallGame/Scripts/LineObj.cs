using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class LineObj : NetworkBehaviour
{
    public struct Point
    {
        public Vector3 point;
    };
    public class Points : SyncListStruct<Point> { }

    public Points points = new Points();

    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isClient)
            Draw();
    }

    void Draw()
    {
        print("Draw line!");
        lineRenderer.positionCount = 0;
        foreach (var p in points)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, p.point);
        }
        edgeCollider.points = points.Select(a =>
        {
            var p = transform.InverseTransformPoint(a.point);
            return new Vector2(p.x, p.y);
        }).ToArray();
    }
}
