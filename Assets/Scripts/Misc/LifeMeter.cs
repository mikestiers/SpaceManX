using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeMeter : MonoBehaviour
{
    public Image lifeImage;
    public int numLives;

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < lives; i++)
        //{
        //    Image newImage = Instantiate(lifeImage, transform);
        //    newImage.transform.localScale = Vector3.one;
        //}
    }

    // Update is called once per frame
    void Update()
    {
//        GameManager.instance.onLifeValueChanged.AddListener(UpdateLifeText);

    }
}
