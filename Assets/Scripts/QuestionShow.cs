using System.Collections;
using UnityEngine;

public class QuestionShow : MonoBehaviour
{
    public RectTransform panel;
    public float moveSpeed = 800f;
    public float showTime = 3f;
    public float waitTime = 2f;

    private float hiddenY = 720f;
    private float visibleY = 620f;

    void Start()
    {
        panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, hiddenY);
        StartCoroutine(PanelLoop());
    }

    IEnumerator PanelLoop()
    {
        while (true)
        {
            // Aþaðý in
            yield return MovePanel(visibleY);
            Debug.Log("Soru gosteriliyor");

            yield return new WaitForSeconds(showTime);

            //Yukarý çýk
            yield return MovePanel(hiddenY);

            yield return new WaitForSeconds(waitTime);

            Debug.Log("Yeni soru hazýrlanýyor");
        }
    }

    IEnumerator MovePanel(float targetY)
    {
        while (Mathf.Abs(panel.anchoredPosition.y - targetY) > 1f)
        {
            Vector2 pos = panel.anchoredPosition;
            pos.y = Mathf.MoveTowards(pos.y, targetY, moveSpeed * Time.deltaTime);
            panel.anchoredPosition = pos;
            yield return null;
        }
    }
}
