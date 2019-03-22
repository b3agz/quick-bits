using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleDragAndDropHandler : MonoBehaviour {

    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;
    private Transform dragTransform;

    void Start() {
        
        // Cache our GraphicRaycaster and EventSystem components. We're going to need them.
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();

    }


    void Update() {
        
        if (Input.GetMouseButtonDown(0)) {

            // Create a new pointer event and set the pointer event position to the current mouse position.
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;

            // Create a list to store raycast results in and then cast ray and pass the list over to recieve the results
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            // Loop through the results and check each one for anything we might want take action on.
            foreach (RaycastResult result in results) {

                // If current result is a dragable object, set it as our current drag transform.
                if (result.gameObject.tag == "Draggable") {

                    dragTransform = result.gameObject.transform;
                    Debug.Log(string.Format("<b>{0}</b> has been selected.", dragTransform.name));

                }

            }

        // If we currently have a drag transform set and the mouse button is down, set the transform position to the mouse position.
        } else if (Input.GetMouseButton(0) && dragTransform != null) {

            dragTransform.position = Input.mousePosition;

        // On mouse button up, drop any icon we're drag by setting the drag transform to null.
        }  else if (Input.GetMouseButtonUp(0) && dragTransform != null) {

            Debug.Log(string.Format("<b>{0}</b> has been deselected.", dragTransform.name));
            dragTransform = null;

        }

    }

}
