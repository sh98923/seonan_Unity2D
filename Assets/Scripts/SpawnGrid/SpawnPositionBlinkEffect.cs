using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionBlinkEffect : MonoBehaviour
{
    [SerializeField] private float _blinkTime;

    private void Update()
    { 
        if (_blinkTime < 0.5f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1,1,1,1-_blinkTime);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, _blinkTime);
            if (_blinkTime > 1)
            {
                _blinkTime = 0;
            }
        }

        _blinkTime += Time.deltaTime;
    }
}
