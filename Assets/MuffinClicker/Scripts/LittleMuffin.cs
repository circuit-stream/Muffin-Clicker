using UnityEngine;
using UnityEngine.UI;

public class LittleMuffin : MonoBehaviour
{
    public Image MuffinImage;
    public Sprite[] MuffinSprites;

    public float animationDuration = 1.5f;
    public float maxRotationVelocity = 160;
    public float maxHorizontalVelocity = 100;
    public float maxInitialVerticalVelocity = 40;
    public float gravity = 400;

    private float elapsedTime;

    private float TimePerSprite
    {
        get { return animationDuration / MuffinSprites.Length; }
    }

    private RectTransform myTransform;
    private uint spriteIndex = 0;
    private Vector2 currentVelocity;
    private Vector3 rotationVelocity;

    public void Setup(Vector2 position)
    {
        myTransform = GetComponent<RectTransform>();

        myTransform.localPosition = position;
        myTransform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));

        rotationVelocity = new Vector3(0, 0, Random.Range(-maxRotationVelocity, maxRotationVelocity));
        currentVelocity = new Vector2(Random.Range(-maxHorizontalVelocity, maxHorizontalVelocity), Random.Range(0, maxInitialVerticalVelocity));
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= animationDuration)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        UpdateTransform();
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (Mathf.Floor(elapsedTime / TimePerSprite) <= spriteIndex)
            return;

        spriteIndex++;
        MuffinImage.sprite = MuffinSprites[spriteIndex];
    }

    private void UpdateTransform()
    {
        myTransform.anchoredPosition += currentVelocity * Time.deltaTime;
        myTransform.Rotate(rotationVelocity * Time.deltaTime);

        currentVelocity.y -= gravity * Time.deltaTime;
    }
}