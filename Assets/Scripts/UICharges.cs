using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UICharges : MonoBehaviour
{
    [SerializeField, Tooltip("ref a la boule pour lire ses charges")]
    private Boule boule;

    [SerializeField, Tooltip("les 3 image ui pour les charges")]
    private Image[] imagesCharges;

    private void Update()
    {
        if (boule == null) return;

        for (int i = 0; i < imagesCharges.Length; i++)
        {
            if (imagesCharges[i] != null)
            {
                imagesCharges[i].enabled = (i < boule.ChargerAcceleration);
            }
        }
    }
}
