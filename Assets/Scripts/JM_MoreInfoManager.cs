using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_MoreInfoManager : MonoBehaviour
{
    public GameObject moreInfo1;
    public GameObject moreInfo2;
    public GameObject moreInfo3;
    public GameObject moreInfo4;
    public GameObject moreInfo5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInfoClick1()
    {
        moreInfo1.SetActive(true);
        moreInfo2.SetActive(false);
        moreInfo3.SetActive(false);
        moreInfo4.SetActive(false);
        moreInfo5.SetActive(false);
    }

    public void OnInfoClick2()
    {
        moreInfo1.SetActive(false);
        moreInfo2.SetActive(true);
        moreInfo3.SetActive(false);
        moreInfo4.SetActive(false);
        moreInfo5.SetActive(false);
    }

    public void OnInfoClick3()
    {
        moreInfo1.SetActive(false);
        moreInfo2.SetActive(false);
        moreInfo3.SetActive(true);
        moreInfo4.SetActive(false);
        moreInfo5.SetActive(false);
    }

    public void OnInfoClick4()
    {
        moreInfo1.SetActive(false);
        moreInfo2.SetActive(false);
        moreInfo3.SetActive(false);
        moreInfo4.SetActive(true);
        moreInfo5.SetActive(false);
    }

    public void OnInfoClick5()
    {
        moreInfo1.SetActive(false);
        moreInfo2.SetActive(false);
        moreInfo3.SetActive(false);
        moreInfo4.SetActive(false);
        moreInfo5.SetActive(true);
    }

    public void OnClickCheckOut()
    {
        if (moreInfo1.activeSelf)
            Application.OpenURL("https://woodwerk.com/en/product/bergen-bed-200x200-cm/");
        if (moreInfo2.activeSelf)       
            Application.OpenURL("https://www.ikea.com/kr/ko/p/vittsjoe-shelving-unit-black-brown-glass-80213314/");
        if (moreInfo3.activeSelf)
            Application.OpenURL("https://www.trdst.com/goods/goods_view.php?goodsNo=1000007935");
        if (moreInfo4.activeSelf)
            Application.OpenURL("https://www.ikea.com/kr/ko/p/gladom-tray-table-black-00411997/");
        if (moreInfo5.activeSelf)
            Application.OpenURL("https://www.smegkorea.com/product/product_view.php?pseq=3024");
    }
}
