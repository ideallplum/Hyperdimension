using UnityEngine;
using UnityEngine.UI;

public class FPSView : MonoBehaviour
{
    public Text text;

    float elapsed = 0f;
    int fps = 0;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        fps++;

        if (elapsed >= 1f)
        {
            text.text = fps.ToString();

            elapsed = 0f;
            fps = 0;
        }
    }
}
