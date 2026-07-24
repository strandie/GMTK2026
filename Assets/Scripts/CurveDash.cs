using System.Collections.Generic;
using UnityEngine;

public class CurveDash : MonoBehaviour
{
    private Rigidbody2D rb;

    private List<Vector2> rawPath =
        new List<Vector2>();

    private List<Vector2> dashPath =
        new List<Vector2>();


    [Header("Drawing")]
    public float maxInk = 8f;
    public float maxDrawDistance = 5f;
    public float minimumPointDistance = 0.15f;


    [Header("Dash")]
    public float dashSpeed = 50f;


    [Header("Visual")]
    public LineRenderer line;


    private bool drawing;
    private bool dashing;

    private int currentPoint;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (line)
            line.positionCount = 0;
    }



    private Vector2 MouseWorld()
    {
        return Camera.main.ScreenToWorldPoint(
            Input.mousePosition
        );
    }



    private void Update()
    {
        Vector2 mouse = MouseWorld();



        if (Input.GetMouseButtonDown(1))
        {
            rawPath.Clear();

            dashPath.Clear();


            drawing = true;


            AddPoint(rb.position);
        }



        if (drawing && Input.GetMouseButton(1))
        {
            Vector2 point =
                ClampFromPlayer(mouse);


            AddPoint(point);

            UpdateLine();
        }



        if (drawing && Input.GetMouseButtonUp(1))
        {
            drawing = false;

            PreparePath();

            dashing = true;

            currentPoint = 0;
        }



        if (Input.GetMouseButtonDown(0))
        {
            drawing = false;
            dashing = false;

            rawPath.Clear();
            dashPath.Clear();

            rb.linearVelocity = Vector2.zero;

            if (line)
                line.positionCount = 0;
        }
    }





    private Vector2 ClampFromPlayer(Vector2 point)
    {
        Vector2 offset =
            point - rb.position;


        if (offset.magnitude > maxDrawDistance)
        {
            offset =
                offset.normalized *
                maxDrawDistance;
        }


        return rb.position + offset;
    }






    private void AddPoint(Vector2 point)
    {
        if (rawPath.Count > 0)
        {
            if (Vector2.Distance(
                rawPath[^1],
                point)
                < minimumPointDistance)
                return;
        }


        rawPath.Add(point);
    }






    private void PreparePath()
    {
        dashPath.Clear();


        float length = 0;


        for (int i = 1; i < rawPath.Count; i++)
        {
            float dist =
                Vector2.Distance(
                    rawPath[i - 1],
                    rawPath[i]
                );


            if (length + dist > maxInk)
                break;


            length += dist;


            dashPath.Add(rawPath[i]);
        }
    }






    private void UpdateLine()
    {
        if (!line)
            return;


        line.positionCount =
            rawPath.Count;


        for (int i = 0; i < rawPath.Count; i++)
        {
            line.SetPosition(
                i,
                rawPath[i]
            );
        }
    }






    private void FixedUpdate()
    {
        if (!dashing)
            return;


        if (currentPoint >= dashPath.Count)
        {
            EndDash();
            return;
        }


        Vector2 target =
            dashPath[currentPoint];


        Vector2 dir =
            (target - rb.position)
            .normalized;



        rb.linearVelocity =
            dir * dashSpeed;



        if (Vector2.Distance(
            rb.position,
            target)
            < 0.1f)
        {
            currentPoint++;
        }
    }






    private void EndDash()
    {
        dashing = false;

        rb.linearVelocity =
            Vector2.zero;


        if (line)
            line.positionCount = 0;
    }
}