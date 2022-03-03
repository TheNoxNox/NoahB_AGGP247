using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabSwitcher : MonoBehaviour
{
    public Spaceship spacePart;

    public TankBall ballPart;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            ballPart.gameObject.SetActive(false);
            spacePart.gameObject.SetActive(true);          
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            spacePart.gameObject.SetActive(false);
            ballPart.gameObject.SetActive(true);
        }
    }
}
