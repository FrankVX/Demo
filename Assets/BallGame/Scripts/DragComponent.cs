using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
public class DragComponent : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    bool draging;
    Vector3 offset, curPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        draging = true;
        curPos = GameSetting.CurMouseWorldPos(eventData.position);
        offset = transform.position - curPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        curPos = GameSetting.CurMouseWorldPos(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draging = false;
    }

    void Update()
    {
        if (GameSetting.Ins == null || !GameSetting.Ins.isEditor || !NetManager.Ins.IsServer) return;
        if (draging)
        {
            transform.position = curPos + offset;
        }
    }
}
