using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite moleHardHat;
    [SerializeField] private Sprite moleHatBroken;
    [SerializeField] private Sprite moleHit;
    [SerializeField] private Sprite moleHatHit;

    private SpriteRenderer spriteRenderer;
    private Vector2 startPos = new Vector2(0f, -2.56f);
    private Vector2 endPos = Vector2.zero;
    private float showDuration = 0.5f;
    private float duration = 1f;

    //mole paramenters
    private bool hittable = true;
    public enum MoleType
    {
        Standard,
        HardHat,
        Bomb
    };
    private MoleType moleType;
    private float hardRate = 0.25f;
    private int lives;

    private IEnumerator ShowHide(Vector2 start, Vector2 end)
    {
        transform.localPosition = start;
        // show the mole
        float elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(start, end, elapsed / showDuration);
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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        CreateNext();
        StartCoroutine(ShowHide(startPos, endPos));
    }

    private void OnMouseDown()
    {
        if (hittable)
        {
            switch (moleType)
            {
                case MoleType.Standard:
                    spriteRenderer.sprite = moleHit;

                    //stop animation
                    StopAllCoroutines();
                    StartCoroutine(QuickHide());

                    hittable = false;
                    break;
                case MoleType.HardHat:
                    // if lives == 2 => reduce and change sprite.
                    if (lives == 2)
                    {
                        spriteRenderer.sprite = moleHatBroken;
                        lives--;
                    }
                    else
                    {
                        spriteRenderer.sprite = moleHatHit;
                        StopAllCoroutines();
                        StartCoroutine(QuickHide());
                        hittable = false;
                    }
                    break;
                case MoleType.Bomb:
                    //game over
                    break;
                default:
                    break;

            }
        }

    }

    private IEnumerator QuickHide()
    {
        yield return new WaitForSeconds(0.25f);

        if (!hittable)
        {
            Hide();
        }
    }

    public void Hide()
    {
        transform.localPosition = startPos;
    }

    public void CreateNext()
    {
        float random = Random.Range(0f, 1f);
        if (random < hardRate)
        {
            //create a hard hat mole
            moleType = MoleType.HardHat;
            spriteRenderer.sprite = moleHardHat;
            lives = 2;
        }
        else
        {
            //create a standard mole
            moleType = MoleType.Standard;
            spriteRenderer.sprite = mole;
            lives = 1;
        }

        hittable = true;
    }
}
