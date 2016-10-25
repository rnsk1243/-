using UnityEngine;
using System.Collections;

public class ObjectMath : MonoBehaviour {

    // 시작 높이
    public float height = 0.0f;
    // 힘의 크기
    public float power = 0.0f;
    // 각도
    public float angle = 0.0f;
    // 질량
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

    float realityTimy;
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
        tr.Translate(testPosition);

        // Debug.Log("cos = " + Mathf.Cos((angle * Mathf.PI) / 180.0f));
        // Debug.Log("sin = " + Mathf.Sin((angle * Mathf.PI) / 180.0f));
        // 출발
        rig.AddForce(new Vector3(vectorX, vectorY));
        realityTimy = Time.realtimeSinceStartup;
        //Debug.Log("Time.deltaTime = " + Time.deltaTime);
        //rig.vel

        // 예상 최고 높이
        if(isGravity)
        {
            StartCoroutine(GetVelocity());
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

    IEnumerator GetVelocity()
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
        //Debug.Log("중력 = " + Physics.gravity);

        // 힘으로만 올라간 높이
        float powerH = (-Mathf.Pow(velocity.y, 2.0f) / (2.0f * Physics.gravity.y));
        float h = height + powerH;


        // 예상 최고높이까지 걸린 시간
        float time = Mathf.Sqrt((2 * powerH) / -Physics.gravity.y);
        Debug.Log("예상 최고높이까지 걸린 시간 = " + time + "초");
        print("=========예상============");
        Debug.Log("예상최고 높이 = " + h);
        // 예상 최고높이에서 땅에 닿기까지 걸린 시간
        float time2 = Mathf.Sqrt((2 * h) / -Physics.gravity.y);

        float resultTime = time + time2;
        Debug.Log("예상 시작부터 땅에 닿기까지 걸린 시간 = " + resultTime + "초");

        Debug.Log("예상 이동 거리 = " + resultTime * velocity.x);

    } 

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
        // 예상 시간
        float resultTime = height / -velocity.y;
        print("=========예상============");
        Debug.Log("예상 시작부터 땅에 닿기까지 걸린 시간 = " + resultTime + "초");

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
            print("실제 걸린 시간 = " + (Time.realtimeSinceStartup - realityTimy));
           print("실제 이동 거리 = "+ (creshPosition.x - testPosition.x));
        }

    }

}
