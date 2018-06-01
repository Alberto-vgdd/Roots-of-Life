using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralStatistics : MonoBehaviour
{
    public Image progressImage;
    public Text percentageText;
    public GameObject timeObject;
    public GameObject seedsObject;
    public float percentage = 1.0f;
    private bool d = true;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update()
    {
        setProgressbar(percentage);
        int n = (int)Mathf.Round(percentage * 100);
        setHours((int)Mathf.Round(n * 0.5f));
        setSeeds(n * 5);

        calc();
    }

    public void setProgressbar(float p)
    {
        progressImage.fillAmount = p;
        percentageText.text = Mathf.Round(p * 100) + "%";
    }

    public void setHours(int hours)
    {
        timeObject.GetComponentInChildren<Text>().text = hours + "\nhours";
    }

    public void setSeeds(int seeds)
    {
        seedsObject.GetComponentInChildren<Text>().text = seeds + "\nseeds";
    }

    private void calc()
    {
        if (d)
            percentage -= Time.deltaTime * 1f;
        else
            percentage += Time.deltaTime * 0.1f;
        if (percentage >= 1.0f)
        {
            d = true;
            percentage = 1.0f;
        }
        if (percentage <= 0.0f)
        {
            d = false;
            percentage = 0.0f;
        }
    }
}
