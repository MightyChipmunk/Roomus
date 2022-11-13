using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI_CropItem : MonoBehaviour
{
    public Texture texture;
    public string category;

    // Start is called before the first frame update
    void Start()
    {
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        GetComponent<Image>().sprite = Sprite.Create((Texture2D)texture, rect, new Vector2(0.3f, 0.3f));

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Deco_UIManager.Instance.OnClickCategory(category);
    }
}
