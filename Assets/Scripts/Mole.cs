using System.Collections;
using UnityEngine;

public class Mole : MonoBehaviour
{
    private Vector2 startPos = new Vector2(0f, -2.56f);
    private Vector2 endPos = Vector2.zero;
    private float showDuration = 0.5f;
    private float duration = 1f;

    private IEnumerator ShowHide(Vector2 start, Vector2 end)
    {
        transform.localPosition = start;
        // show the mole
        float elapsed = 0f;
        while (elapsed<showDuration)
        {
            transform.localPosition= Vector2.Lerp(start, end, elapsed/showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = end;
        
        // waiting for duration to pass
        yield return new WaitForSeconds(duration);

        // hide the mole
        elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(end, start, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = start;
    }
}
