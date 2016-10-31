using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
    //추적할 타겟 게임오브젝트의 트랜스폼 변수
    public Vector3 targetTr;
    //카메라와의 일정 거리
    public float dist = 8.0f;
    //카메라의 높이 설정
    public float height = 3.0f;
    //부드러운 추적을 위한 변수
    public float dampTrace = 20.0f;
    //카메라 자신의 트랜스폼 변수
    public Transform tr;

    Init InitPosition;

	// Use this for initialization
	void Start () {

        InitPosition = GameObject.Find("GameObject").GetComponent<Init>();

        tr = GetComponent<Transform>();
        
        if(InitPosition.power > 800)
        {
            GetComponent<Camera>().fieldOfView = 160;
        }

    }
	
    // 업데이트 함수 함수 호출 이후 한 번씩 호출되는 함수인 LateUpdate 사용
    // 추적할 타겟의 이동이 종료된 이후에 카메라가 추적하기 위해 LateUpdate 사용
	void LateUpdate () {

        targetTr = new Vector3(0, InitPosition.height, 0);

        //카메라의 위치를 추적대상의 dist 변수만큼 뒤쪽으로 배치하고
        //height 변수만큼 위로 올림
        tr.position = Vector3.Lerp(tr.position,
                                   new Vector3(targetTr.x, targetTr.y + height, targetTr.z - dist)
                                    , Time.deltaTime * dampTrace);

        //카메라가 타겟 게임오브젝트를 바라보게 설정
        //tr.LookAt(targetTr);
	}
}
