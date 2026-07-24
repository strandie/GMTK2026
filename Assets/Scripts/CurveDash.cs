using System.Collections.Generic;
using UnityEngine;

public class CurveDash : MonoBehaviour
{
    private Rigidbody2D rb;

    private List<Vector2> pathPoints =
        new List<Vector2>();

    [Header("Path")]
    public float maxInk = 8f;

    [Header("Dash")]
    public float dashSpeed = 20f;

    [Header("Visual")]
    public LineRenderer line;


    private bool drawing;

    private float currentLength;

    private int currentPoint;

    private bool dashing;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (line != null)
            line.positionCount = 0;
    }



    private void Update()
    {
        Vector2 mouse =
            Camera.main.ScreenToWorldPoint(
                Input.mousePosition
            );


        // Start drawing
        if (Input.GetMouseButtonDown(1))
        {
            pathPoints.Clear();

            currentLength = 0;

            currentPoint = 0;

            drawing = true;

            AddPoint(rb.position);
        }



        // Drawing
        if (drawing && Input.GetMouseButton(1))
        {
            AddPoint(mouse);

            UpdateLine();
        }



        // Release
        if (drawing && Input.GetMouseButtonUp(1))
        {
            drawing = false;

            StartDash();
        }



        // Cancel
        if (Input.GetMouseButtonDown(0))
        {
            drawing = false;

            dashing = false;

            pathPoints.Clear();

            if (line)
                line.positionCount = 0;
        }
    }



    private void AddPoint(Vector2 point)
    {
        if (pathPoints.Count > 0)
        {
            float distance =
                Vector2.Distance(
                    pathPoints[^1],
                    point
                );


            if (currentLength + distance > maxInk)
                return;


            currentLength += distance;
        }


        pathPoints.Add(point);
    }



    private void UpdateLine()
    {
        if (!line)
            return;


        line.positionCount =
            pathPoints.Count;


        for (int i = 0; i < pathPoints.Count; i++)
        {
            line.SetPosition(
                i,
                pathPoints[i]
            );
        }
    }



    private void StartDash()
    {
        if (pathPoints.Count < 2)
            return;


        currentPoint = 1;

        dashing = true;
    }



    private void FixedUpdate()
    {
        if (!dashing)
            return;


        if (currentPoint >= pathPoints.Count)
        {
            dashing = false;

            line.positionCount = 0;

            return;
        }



        Vector2 target =
            pathPoints[currentPoint];


        rb.MovePosition(
            Vector2.MoveTowards(
                rb.position,
                target,
                dashSpeed *
                Time.fixedDeltaTime
            )
        );



        if (Vector2.Distance(
            rb.position,
            target
        ) < 0.05f)
        {
            currentPoint++;
        }
    }
}