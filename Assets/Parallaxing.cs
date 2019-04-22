using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{

    public Transform[] layers;
    private float[] parallaxScales;
    public float smoothing;

    private Transform camera;
    private Vector3 previousCamera;
    // Start is called before the first frame update
    void Awake()
    {
        camera = Camera.main.transform;
    }
    void Start()
    {
        previousCamera = camera.position;
        parallaxScales = new float[layers.Length];
        for(int i=0; i < layers.Length; i++)
        {
            parallaxScales[i] = layers[i].position.z * -1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        for(int i=0; i < layers.Length; i++)
        {
            float parallax = (previousCamera.x - camera.position.x) * parallaxScales[i];
            float layerPositionX = layers[i].position.x + parallax;
            Vector3 layerPosition = new Vector3(layerPositionX, layers[i].position.y, layers[i].position.z);
            layers[i].position = Vector3.Lerp(layers[i].position, layerPosition, smoothing * Time.deltaTime);

            previousCamera = camera.position;
        }
    }
}
