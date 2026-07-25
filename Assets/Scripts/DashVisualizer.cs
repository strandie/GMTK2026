using UnityEngine;

public class DashVisualizer : MonoBehaviour
{
    public LineRenderer line;
    public Transform lineTip;

    public void Draw(Vector2 direction, float length)
    {
        line.enabled = true;

        line.SetPosition(0, transform.position);

        line.SetPosition(1, transform.position + (Vector3)(direction * length));

        lineTip.gameObject.SetActive(true);
        lineTip.position = transform.position + (Vector3)(direction * length);
        lineTip.eulerAngles = Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
    }


    public void Hide()
    {
        line.enabled = false;
        lineTip.gameObject.SetActive(false);
    }

}
