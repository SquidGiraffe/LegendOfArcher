using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;//Ű����,���콺,��ġ�� �̺�Ʈ�� ������Ʈ�� ���� �� �ִ� ����� ����

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
    //��ġ�� ��ġ�� ã�Ƴ� ���� �̹����� �������� �̵��ϰ� �������
    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
        isInput = true;
        controller.isInput = isInput;
    }

    // ������Ʈ�� Ŭ���ؼ� �巡���ϴ� ���߿� ������ �̺�Ʈ
    // Ŭ���� ������ ���·� ���콺�� ���߸� �̺�Ʈ�� ������ ����
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
        // eventData.position : ��ġ�� ��ġ
        var inputPos    = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        // inputVector�� �׳� ���� �ʰ� ���� leverRange�� ������ ������ 
        // inputVector�� �ػ󵵸� ������� �� ���̱⿡ ĳ������ �̵������� ���⿡�� �ʹ� ū ���̴�
        // �׷��� �Է°��� ������ 0~1���̷� ����� ����ȭ�� ������ ĳ���Ϳ� �����ϱ� ����
        // �ػ󵵰� �ٲ�� �Է� ���Ⱚ�� ũ�Ⱑ �ٲ��� ĳ������ �̵��ӵ��� �ٲ�� ������ ����
        inputDirection = inputVector / leverRange;

    }

    private void InputControlVector()
    {
        //ĳ���Ϳ��� �Է� ���͸� ����

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
