using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;//키보드,마우스,터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원

public class JoyStick : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField,Range(10,200)]
    private float leverRange;

    private Vector2 inputDirection;
    private bool isInput;

    [SerializeField]
    private Player controller;

    bool isBorder;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    //터치한 위치를 찾아내 레버 이미지가 그쪽으로 이동하게 해줘야함
    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
        isInput = true;
        controller.isInput = isInput;
    }

    // 오브젝트를 클릭해서 드래그하는 도중에 들어오는 이벤트
    // 클릭을 유지한 상태로 마우스를 멈추면 이벤트가 들어오지 않음
    public void OnDrag(PointerEventData eventData)
    {
       
            ControlJoystickLever(eventData);
            controller.isInput = isInput;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        controller.Move(Vector2.zero, isBorder, isInput);
        controller.isInput = isInput;
    }

    void ControlJoystickLever(PointerEventData eventData)
    {
        // eventData.position : 터치한 위치
        var inputPos    = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        // inputVector를 그냥 쓰지 않고 굳이 leverRange로 나누는 이유는 
        // inputVector가 해상도를 기반으로 한 값이기에 캐릭터의 이동값으로 쓰기에는 너무 큰 값이다
        // 그래서 입력값의 범위를 0~1사이로 만들어 정규화된 값으로 캐릭터에 전달하기 위함
        // 해상도가 바뀌면 입력 방향값의 크기가 바꿔져 캐릭터의 이동속도가 바뀌는 문제도 있음
        inputDirection = inputVector / leverRange;

    }

    private void InputControlVector()
    {
        //캐릭터에게 입력 벡터를 전달

            controller.Move(inputDirection,isBorder, isInput);

        //Debug.Log(inputDirection.x + "/" + inputDirection.y);
    }

    private void Update()
    {
        StopToObstacle();
        InputControlVector();


    }

    public void StopToObstacle()
    {
        Vector3 tempVec = new Vector3(controller.transform.position.x,
                                      controller.transform.position.y + 0.5f,
                                      controller.transform.position.z);

        Debug.DrawRay(tempVec, controller.transform.forward, Color.red);
        isBorder = Physics.Raycast(tempVec,
                                   controller.transform.forward,
                                   1,
                                   LayerMask.GetMask("Obstacle"));
    }

}
