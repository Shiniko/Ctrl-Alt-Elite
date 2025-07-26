using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WindowPointer : MonoBehaviour
{
    [SerializeField] private Camera uiCamera;

    public Transform target;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 targetPosScreenPoint;
    [SerializeField] private Transform pointer;
    [SerializeField] private Sprite arrow;
    [SerializeField] private Sprite noArrow;
    [SerializeField] private Sprite gateArrow;
    [SerializeField] private Image pointerImage;

    private Vector3 toPosition;
    private Vector3 fromPosition;
    private Vector3 direction;
    [SerializeField] private float angle;
    private Vector3 pointerWorldPosition;
    [SerializeField] private float borderSize;

    [SerializeField] private bool isOffScreen;
    [SerializeField] private bool isOnScreen;
    [SerializeField] private Vector3 cappedTargetScreenPosition;


    void Awake()
    {
        if (pointer == null)
        {
            if (GameObject.FindGameObjectWithTag("Pointer") != null)
            {
                pointer = GameObject.FindGameObjectWithTag("Pointer").transform;

                if (pointerImage == null)
                {
                    pointerImage = GameObject.FindGameObjectWithTag("Pointer").GetComponentInChildren<Image>();
                }
            }
        }
    }

    private void Update()
    {
        if (target != null)
        {

                targetPosition = target.transform.position;
                toPosition = targetPosition;

            fromPosition = Camera.main.transform.position;
            fromPosition.z = 0f;

            direction = (toPosition - fromPosition).normalized;

            //Debug.DrawRay(pointer.position, direction, Color.blue);

            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            pointer.localEulerAngles = new Vector3(0f, 0f, angle);

            targetPosScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);

            if (targetPosScreenPoint.x <= 0f || targetPosScreenPoint.x >= Screen.width || targetPosScreenPoint.y <= 0f || targetPosScreenPoint.y >= Screen.height)
            {
                isOffScreen = true;


                    pointerImage.sprite = arrow;

            }
            else
            {
                isOffScreen = false;
                pointerImage.sprite = noArrow;


                    target = null;

            }

            if (isOffScreen)
            {
                cappedTargetScreenPosition = targetPosScreenPoint;

                if (cappedTargetScreenPosition.x <= borderSize)
                {
                    cappedTargetScreenPosition.x = borderSize;
                }

                if (cappedTargetScreenPosition.x >= (Screen.width - borderSize))
                {
                    cappedTargetScreenPosition.x = (Screen.width - borderSize);
                }

                if (cappedTargetScreenPosition.y <= borderSize)
                {
                    cappedTargetScreenPosition.y = borderSize;
                }

                if (cappedTargetScreenPosition.y >= (Screen.height - borderSize))
                {
                    cappedTargetScreenPosition.y = (Screen.height - borderSize);
                }

                pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
                pointer.position = pointerWorldPosition;
                pointer.localPosition = new Vector3(pointer.localPosition.x, pointer.localPosition.y, 0f);

            }
        }
        else
        {
            isOffScreen = false;
            pointerImage.sprite = noArrow;
        }

    }

    public void SetOff()
    {
        pointerImage.sprite = noArrow;
        target = null;
    }


}
