using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lipsync
{

    public class ChangeIcon : MonoBehaviour
    {
        [SerializeField] private Image checkSprite;
        [SerializeField] private Sprite micIcon;
        [SerializeField] private Sprite stopIcon;
        [SerializeField] private Toggle btn;

        void Start()
        {
            btn.onValueChanged.AddListener((value) =>
            {
                if (!value)
                {
                    checkSprite.sprite = micIcon;
                    return;
                }

                checkSprite.sprite = stopIcon;
            });
        }
    }
}

