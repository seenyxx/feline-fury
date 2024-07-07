using UnityEngine;

public class ScaleWorldCanvas : MonoBehaviour
{
    // Set scale on start up
    private void Awake() {
        SetScale();
    }

    // Scales the canvas of UI to match the screen proportions and different aspect ratios
    public void SetScale()
    {
        RectTransform rt = GetComponent<RectTransform>();
        float canvasHeight = rt.rect.height;
        float desiredCanvasWidth = canvasHeight * Camera.main.aspect;

        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, desiredCanvasWidth);
    }
}
