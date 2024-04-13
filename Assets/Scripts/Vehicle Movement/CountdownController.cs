using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public CarController carController;

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return StartCoroutine(ScaleTextOverTime(countdownText.rectTransform, Vector3.one * 1.5f, 0.5f));
            yield return StartCoroutine(ChangeTextColorOverTime(countdownText, new Color(0.2f, 0.2f, 0.2f), 0.3f)); 
            yield return new WaitForSeconds(0.3f);
            countdownText.color = Color.cyan; 
        }

        
        countdownText.text = "Go!";
        yield return StartCoroutine(ScaleTextOverTime(countdownText.rectTransform, Vector3.one * 2f, 0.5f));

        
        countdownText.gameObject.SetActive(false);

        // Allows car movement
        carController.canMove = true;
    }

    private IEnumerator ScaleTextOverTime(RectTransform rectTransform, Vector3 targetScale, float duration)
    {
        float timer = 0f;
        Vector3 startScale = rectTransform.localScale;

        while (timer < duration)
        {
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = targetScale;
    }

    private IEnumerator ChangeTextColorOverTime(TextMeshProUGUI text, Color targetColor, float duration)
    {
        float timer = 0f;
        Color startColor = text.color;

        while (timer < duration)
        {
            text.color = Color.Lerp(startColor, targetColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        text.color = targetColor;
    }
}