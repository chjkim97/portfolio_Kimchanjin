using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraControllerCardView : MonoBehaviour
{
    private float speed = 200f;
    public float yRangeup = 579f;
    public float yRangedown = -900f;
    public Button Up;
    public Button Down;

    private bool isMovingUp = false;
    private bool isMovingDown = false;

    void Start()
    {
        // 버튼에 눌림/놓임 이벤트 추가
        Up.GetComponent<EventTrigger>().triggers.Add(CreateEventTriggerEntry(EventTriggerType.PointerDown, () => isMovingUp = true));
        Up.GetComponent<EventTrigger>().triggers.Add(CreateEventTriggerEntry(EventTriggerType.PointerUp, () => isMovingUp = false));
        Down.GetComponent<EventTrigger>().triggers.Add(CreateEventTriggerEntry(EventTriggerType.PointerDown, () => isMovingDown = true));
        Down.GetComponent<EventTrigger>().triggers.Add(CreateEventTriggerEntry(EventTriggerType.PointerUp, () => isMovingDown = false));
    }

    void Update()
    {
        if (isMovingUp)
        {
            CameraMove(1f);
        }
        else if (isMovingDown)
        {
            CameraMove(-1f);
        }
    }

    void CameraMove(float direction)
    {
        transform.Translate(Vector2.up * direction * Time.deltaTime * speed);

        // 카메라 위치 제한
        if (transform.position.y > yRangeup)
        {
            transform.position = new Vector3(transform.position.x, yRangeup, transform.position.z);
        }
        if (transform.position.y < yRangedown)
        {
            transform.position = new Vector3(transform.position.x, yRangedown, transform.position.z);
        }
    }

    EventTrigger.Entry CreateEventTriggerEntry(EventTriggerType type, System.Action action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener((_) => action());
        return entry;
    }
}

