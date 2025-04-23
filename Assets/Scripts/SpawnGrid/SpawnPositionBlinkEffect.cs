using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPositionBlinkEffect : MonoBehaviour
{
    [SerializeField] private Image _targetImage;
    [SerializeField] private float _blinkTime;

    private void Update()
    { 
        if (_blinkTime < 0.5f)
        {
            _targetImage = GetComponent<Image>();
            _targetImage.color = new Color(1,1,1,1-_blinkTime);
        }
        else
        {
            _targetImage.color = new Color(1, 1, 1, _blinkTime);
            if (_blinkTime > 1)
            {
                _blinkTime = 0;
            }
        }

        _blinkTime += Time.deltaTime;
    }
}
