using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RaycastDebugger : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var results = new List<RaycastResult>();
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count == 0)
            {
                Debug.Log("Raycast hit nothing");
                return;
            }

            foreach (var result in results)
                Debug.Log($"Hit: {result.gameObject.name} | Layer: {LayerMask.LayerToName(result.gameObject.layer)}");
            
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector3 screenPos = Mouse.current.position.ReadValue();
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
                Debug.Log($"Screen: {screenPos}, World: {worldPos}");

                Collider2D hit = Physics2D.OverlapPoint(worldPos);
                Debug.Log($"Physics2D hit: {(hit != null ? hit.gameObject.name : "nothing")}");
            }
        }
    }
}