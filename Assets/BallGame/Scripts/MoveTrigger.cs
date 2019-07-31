using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveTrigger : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (BallController.Self)
        {
            BallController.Self.Move(GameSetting.CurMouseWorldPos(eventData.position));
        }
    }
}
