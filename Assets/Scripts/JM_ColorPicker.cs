using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JM_ColorPicker : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public Image circlePalette;
    public Image picker;
    public Image paletteUI;
    public Image pickerUI;
    public Color selectColor;

    Vector2 paletteSize;
    CircleCollider2D paletteCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        paletteCollider = circlePalette.GetComponent<CircleCollider2D>();
        paletteSize = new Vector2(
            circlePalette.GetComponent<RectTransform>().rect.width,
            circlePalette.GetComponent<RectTransform>().rect.height);
        selectColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SelectColor();
    }

    public void OnDrag(PointerEventData eventData)
    {
        SelectColor();
    }

    void SelectColor()
    {
        Vector3 offset = Input.mousePosition - transform.position;
        Vector3 diff = Vector3.ClampMagnitude(offset, paletteCollider.radius - 1);
        Vector3 uiDiff = Vector3.ClampMagnitude(offset, 55);
        picker.transform.position = transform.position + diff;
        pickerUI.transform.position = transform.position + uiDiff;
        selectColor = GetColor();
    }

    Color GetColor()
    {
        Vector2 circlePalettePos = circlePalette.transform.position;
        Vector2 pickerPos = picker.transform.position;
        Vector2 pos = pickerPos - circlePalettePos + paletteSize * 0.5f;
        Vector2 normalized = new Vector2(
            (pos.x / (circlePalette.GetComponent<RectTransform>().rect.width)),
            (pos.y / (circlePalette.GetComponent<RectTransform>().rect.height)));
         
        Texture2D texture = circlePalette.mainTexture as Texture2D;
        Color color = texture.GetPixelBilinear(normalized.x, normalized.y);
        return color;
    }
    
}
