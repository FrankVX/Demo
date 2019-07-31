using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
public class LineDrawer : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public float pointLength;
    public LineRenderer prefab;
    public Transform line_root;

    Vector3 lastPos, curPos;

    EdgeCollider2D edgeCollider;
    LineRenderer lineRenderer;
    bool isPress;

    Vector3[] buffs = new Vector3[1000];

    public void OnBeginDrag(PointerEventData eventData)
    {
        isPress = true;
        var obj = Instantiate(prefab.gameObject, line_root);
        lineRenderer = obj.GetComponent<LineRenderer>();
        edgeCollider = obj.GetComponent<EdgeCollider2D>();
        lastPos = GameSetting.CurMouseWorldPos(eventData.position);

        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, lastPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        curPos = GameSetting.CurMouseWorldPos(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isPress = false;

        var count = lineRenderer.GetPositions(buffs);

        if (count < 2)
        {
            Destroy(lineRenderer.gameObject);
            lineRenderer = null;
            return;
        }

        var vs = new Vector2[count];
        for (int i = 0; i < count; i++)
        {
            var local_pos = lineRenderer.transform.InverseTransformPoint(buffs[i]);
            vs[i] = local_pos;
        }
        edgeCollider.points = vs;
        var line = lineRenderer.GetComponent<LineObj>();
        line.points.Clear();
        for (int i = 0; i < count; i++)
        {
            line.points.Add(new LineObj.Point() { point = buffs[i] });
        }
        NetworkServer.Spawn(lineRenderer.gameObject);
        lineRenderer = null;
    }


    private void Update()
    {
        if (!GameSetting.Ins || !GameSetting.Ins.isEditor) return;
        if (NetManager.Ins && NetManager.Ins.IsServer)
        {
            if (isPress && lineRenderer.positionCount < 1000)
            {
                var length = (curPos - lastPos).magnitude;
                if (length >= pointLength)
                {
                    lastPos = curPos;
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, lastPos);
                }
            }
        }

    }
}
