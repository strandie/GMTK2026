using UnityEngine;

public class DashVisualizer : MonoBehaviour
{
    public LineRenderer line;

    public void Draw(Vector2 direction, float length)
    {
        line.enabled = true;

        line.SetPosition(0, transform.position);

        line.SetPosition(1, transform.position + (Vector3)(direction * length));
    }


    public void Hide()
    {
        line.enabled = false;
    }

}
