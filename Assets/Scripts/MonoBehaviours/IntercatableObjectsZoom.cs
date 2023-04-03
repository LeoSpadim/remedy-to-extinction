using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntercatableObjectsZoom : MonoBehaviour
{
    [SerializeField] private float floor = -4f;
    public UnityEvent OnZoomEvent;

    Vector3 startPos;

    private DragTarget drag;

    private Collider2D col;

    void OnEnable()
    {
        transform.position = startPos;
    }

    void Awake()
    {
        drag = FindObjectOfType<DragTarget>();
        col = GetComponent<Collider2D>();
        startPos = transform.position;
    }

    void Update()
    {
        if(gameObject.transform.position.y < floor && !PauseMenu.PAUSED && drag.collider == col && Input.GetMouseButtonUp(0))
        {
            OnZoomEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }


    /*private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer == 6 && !PauseMenu.PAUSED)
        {
            other.gameObject.SetActive(false);
            OnZoomEvent?.Invoke(other.gameObject.name);
        }
    }*/
}
