using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float backgroundScrollSpeed = 0.5f;
    private Material _myMaterial;
    private Vector2 _offSet;

    private void Start()
    {
        _myMaterial = GetComponent<Renderer>().material;
        _offSet = new Vector2(0f, backgroundScrollSpeed);
    }

    private void Update()
    {
        _myMaterial.mainTextureOffset += _offSet * Time.deltaTime;
    }
}
