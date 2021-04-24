using Hyperdimension;
using UnityEngine;

public class ClickTest : MonoBehaviour
{
    public HyperBaseCollider barrelCollider;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            Vector3 targetPos = Math.WorldToSpace(mousePos);
            barrelCollider.HyperTransform.Position = targetPos;
        }
    }
}
