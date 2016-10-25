using UnityEngine;
using System.Collections;

public class ObjectMath : MonoBehaviour {

    // 시작 높이
    public float height = 0.0f;
    // 힘의 크기
    public float power = 0.0f;
    // 각도
    public float angle = 0.0f;
    // 질량 (클수록 적게 이동)
    public float mass = 0.0f;

    // 중력 사용할래 말래?
    public bool isGravity = true;

    Rigidbody rig;
    Transform tr;
    float maxY;

    // 바닥에 충돌했을때 위치
    Vector3 creshPosition;
    // 시작 위치
    Vector3 testPosition;
    // 실제 시간 기록을위한 변수
    float realityTime;
    // 힘의 x성분
    float vectorX;
    float vectorY;
    // Use this for initialization
    void Start () {

        rig = GetComponent<Rigidbody>();
        rig.mass = mass;

        rig.useGravity = isGravity;

        tr = GetComponent<Transform>();

        if(isGravity)
        {
            // 포물선만 가능하도록 각도 조절
            if (angle > 180)
            {
                angle = 180;
            }
            else if (angle < 0)
            {
                angle = 0;
            }
        }else
        {
            // 자유낙하만 가능하도록 각도 조절
            if (angle <= 180)
            {
                angle = 181;
            }
            else if (angle >= 360)
            {
                angle = 359;
            }
        }

         vectorX = power * Mathf.Cos((angle * Mathf.PI) / 180.0f);
         vectorY = power * Mathf.Sin((angle * Mathf.PI) / 180.0f);
        //Debug.Log("Mathf.Sin((angle * Mathf.PI) / 180.0f) = " + Mathf.Sin((angle * Mathf.PI) / 180.0f));
        testPosition = new Vector3(0.0f, height, 0.0f);
        Debug.Log("출발 위치 = " + testPosition);
        // 테스트 위치로 이동
        tr.Translate(testPosition);

        // Debug.Log("cos = " + Mathf.Cos((angle * Mathf.PI) / 180.0f));
        // Debug.Log("sin = " + Mathf.Sin((angle * Mathf.PI) / 180.0f));
        // 출발
        rig.AddForce(new Vector3(vectorX, vectorY));
        // 출발 시간 기록
        realityTime = Time.realtimeSinceStartup;
        //Debug.Log("Time.deltaTime = " + Time.deltaTime);
        //rig.vel

        // 예상 수치 기록 코루틴 실행
        if(isGravity)
        {
            StartCoroutine(useGravity());
        }
        else
        {
            StartCoroutine(zeroGravity());
        }
       

    }

    void Update()
    {
        // 현재높이 나중높이 비교 하여 큰값 저장
        maxY = Mathf.Max(tr.position.y, maxY);
        
    }
    // 중력사용 포물선
    IEnumerator useGravity()
    {
        // 이전 위치
        Vector3 oldPosition = tr.position;
        yield return null; // 한 프레임
        // 한 프레임 후 위치
        Vector3 newPosition = tr.position;
        // 위치변화량
        Vector3 delPosition = newPosition - oldPosition;
        // 속도 = 위치변화량 / 시간
        Debug.Log("속도 = " + delPosition / Time.deltaTime);
        // 속도 
        Vector3 velocity = (delPosition / Time.deltaTime) / 2;

        Debug.Log("y성분 속도 = " + velocity.y);
        //Debug.Log("중력 = " + Physics.gravity);

        // 힘으로만 올라간 높이 // 최고점은 나중속도가 0이므로 다음 공식이 성립함 -> 처음속도^2 = -(2 * 중력 * 거리)
        // 따라서 거리 = -(처음속도^2 / (2 * 중력))
        float powerH = (-Mathf.Pow(velocity.y, 2.0f) / (2.0f * Physics.gravity.y));
        // 최고 높이 = 기본시작높이 + 힘으로만 올라간 높이
        float h = height + powerH;


        // 예상 최고높이까지 걸린 시간 // 1/2 * 중력 * 시간^2 = 거리 이므로 시간을 구한다.
        float time = Mathf.Sqrt((2 * powerH) / -Physics.gravity.y);
        Debug.Log("예상 최고높이까지 걸린 시간 = " + time + "초");
        print("=========예상============");
        Debug.Log("예상최고 높이 = " + h);
        // 예상 최고높이에서 땅에 닿기까지 걸린 시간
        float time2 = Mathf.Sqrt((2 * h) / -Physics.gravity.y);
        // 최고높이까지 올라갈때 걸린 시간과 최고높이에서 땅에 닿기까지 걸린시간을 더하여 전체 걸린시간을 구함.
        float resultTime = time + time2;
        Debug.Log("예상 시작부터 땅에 닿기까지 걸린 시간 = " + resultTime + "초");

        Debug.Log("예상 이동 거리 = " + resultTime * velocity.x);

    } 
    // 무중력 자유낙하
    IEnumerator zeroGravity()
    {
        // 이전 위치
        Vector3 oldPosition = tr.position;
        yield return null; // 한 프레임
        // 한 프레임 후 위치
        Vector3 newPosition = tr.position;
        // 위치변화량
        Vector3 delPosition = newPosition - oldPosition;
        Debug.Log("속도 = " + delPosition / Time.deltaTime);
        // 속도 
        Vector3 velocity = (delPosition / Time.deltaTime) / 2;

        Debug.Log("y성분 속도 = " + velocity.y);
        // 예상 시간 // 중력이 없으므로 -> 처음속도 * 시간 = 시작높이 // 따라서 시간 = 시작높이 / 처음속도 이다.
        // 이때 속도 성분은 -을 곱하여 양수로 만든다. 
        float resultTime = height / -velocity.y;
        print("=========예상============");
        Debug.Log("예상 시작부터 땅에 닿기까지 걸린 시간 = " + resultTime + "초");
        // 거리 = 시간 * 속도
        Debug.Log("예상 이동 거리 = " + resultTime * velocity.x);

    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Floor")
        {
            creshPosition = tr.position;
             print("=========결과============");
            if(isGravity)
            {
                Debug.Log("실제 최고높이 = " + maxY);
            }
            print("실제 걸린 시간 = " + (Time.realtimeSinceStartup - realityTime));
           print("실제 이동 거리 = "+ (creshPosition.x - testPosition.x));
        }

    }

}
