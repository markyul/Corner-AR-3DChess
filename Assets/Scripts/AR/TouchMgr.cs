using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TouchMgr : MonoBehaviour
{
    public GameObject placeObject;
    private ARRaycastManager raycastMgr;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Vector3 firstPose;
    void Start()
    {
        placeObject.transform.localScale = Vector3.one * 0.01f;
        raycastMgr = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);

        Vector2 pos = touch.position;
        Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f);
        Ray ray = Camera.main.ScreenPointToRay(theTouch);
        RaycastHit hit;

        if (touch.phase == TouchPhase.Began)
        {
            // 평면으로 인식한 곳만 레이로 검출
            if (raycastMgr.Raycast(touch.position
                , hits
                , TrackableType.PlaneWithinPolygon))
            {
                Pose pose = hits[0].pose;

                // 객체가 존재하지 않는다면 생성
                if (!placeObject.activeInHierarchy)
                {
                    placeObject.SetActive(true);
                    Instantiate(placeObject
                        , hits[0].pose.position
                        , hits[0].pose.rotation);
                    firstPose = pose.position;
                }
                // 존재한다면 
                else
                {
                    placeObject.transform.position = firstPose;

                }
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("hit 정보 : " + hit.collider.tag);
            }
            else
            {
                Debug.Log("hit 된 객체 없음");
            }
        }
    }
}
