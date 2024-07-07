using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public float disappearDelay = 2f;
    public float disappearSpeed = 3f;
    public float movementSlowDownSpeed = 0.1f;
    public float moveYSpeed = 10f;
    public float finalMoveYSpeed = 0.25f;
    public float enlargeSize = 64f;

    private TextMeshPro textMesh;
    private float lerpTime = 0f;
    private float enlargeLerpTime = 0f;
    private float movementLerpTime = 0f;

    void Start()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update() {
        // Start moving upwards
        movementLerpTime += Time.deltaTime * movementSlowDownSpeed;

        float startSpeed = moveYSpeed;
        float endSpeed = finalMoveYSpeed;

        moveYSpeed = Mathf.Lerp(startSpeed, endSpeed, movementLerpTime);

        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearDelay -= Time.deltaTime;

        // Change size of text
        if (disappearDelay < 0)
        {
            // Make text disappear if the delay has been completed
            lerpTime += Time.deltaTime * disappearSpeed;

            float start = textMesh.fontSize;
            float end = 0f;

            textMesh.fontSize = Mathf.Lerp(start, end, lerpTime);

            if (textMesh.fontSize <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Otherwise enlarge the text
            enlargeLerpTime += Time.deltaTime * disappearSpeed;

            float start = textMesh.fontSize;
            float end = enlargeSize;

            textMesh.fontSize = Mathf.Lerp(start, end, enlargeLerpTime);
        }
    }
}
