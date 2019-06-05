using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveTest : MonoBehaviour
{
    public float speed = 5;
    public float distance = 20;

    public AnimationCurve curve;

    public bool useCurve = true;

    private float t;

    private float remainDistance;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        remainDistance = distance;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.F))
        {
            t += Time.deltaTime / 5;
            t = Mathf.Clamp01(t);
        }

        if (Input.GetKey(KeyCode.B))
        {
            t -= Time.deltaTime / 5;
            t = Mathf.Clamp01(t);
        }


        if (useCurve)
        {
            Vector3 currentDistance = Vector3.right * distance * curve.Evaluate(t);
            transform.position = startPos + currentDistance;
        }
        else
        {
            Vector3 currentDistance = Vector3.right * distance * t;
            transform.position = startPos + currentDistance;
        }

        //if(remainDistance > 0)
        //{
        //    speed = curve.Evaluate()

        //    transform.position += Vector3.right * speed * Time.deltaTime;
        //    remainDistance -= speed * Time.deltaTime;
        //}
    }
}
