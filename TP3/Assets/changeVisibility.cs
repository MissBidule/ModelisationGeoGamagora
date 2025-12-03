using UnityEngine;

public class changeVisibility : MonoBehaviour
{
    public bool initVisibility;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        GetComponent<Renderer>().enabled = !initVisibility;
    }

    /*void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().enabled = initVisibility;
    }*/
}
