using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantfromHandler : MonoBehaviour
{
    public static PlantfromHandler instance;

    public Cubeplantform[] plantfroms;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        plantfroms = new Cubeplantform[7];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
