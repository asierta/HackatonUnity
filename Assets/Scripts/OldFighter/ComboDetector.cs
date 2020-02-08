using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboDetector : MonoBehaviour
{
    enum BUTTONS
    { 
        UP = 0x01,
        DOWN = 0x02,
        LEFT = 0x04,
        RIGHT = 0x08,

        Z = 0x10,
        X = 0x20,

        COMBO_END = 0xFFFFFFF
    };

    public TextMesh textMesh;
    public float maxTimeBetweenInputs; //Deberia ser el tiempo que dura la animación de cada ataque

    uint buttons = 0;
    uint previousButtons = 0;
    uint comboIndex = 0;
    uint[] combo;
    float comboTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        combo = new uint[] {
            (uint)BUTTONS.RIGHT, 0,
            (uint)BUTTONS.RIGHT, 0,
            (uint)BUTTONS.RIGHT, 0,
            (uint)BUTTONS.COMBO_END
        };
        textMesh.text = "Introduce un combo -> -> ->";    
    }

    //Actualiza los bits correspondientes a cada una de las teclas, poniendo a 1 las presionadas y a 0 las que no
    void UpdateButtons()
    {
        previousButtons = buttons; //Save buttons previous state
        buttons = 0;

        buttons |= Input.GetKey(KeyCode.UpArrow) == true ? (uint)(BUTTONS.UP) : 0;
        buttons |= Input.GetKey(KeyCode.DownArrow) == true ? (uint)(BUTTONS.DOWN) : 0;
        buttons |= Input.GetKey(KeyCode.LeftArrow) == true ? (uint)(BUTTONS.LEFT) : 0;
        buttons |= Input.GetKey(KeyCode.RightArrow) == true ? (uint)(BUTTONS.RIGHT) : 0;

        buttons |= Input.GetKey(KeyCode.Z) == true ? (uint)(BUTTONS.Z) : 0;
        buttons |= Input.GetKey(KeyCode.X) == true ? (uint)(BUTTONS.X) : 0;

       // textMesh.text = System.Convert.ToString(buttons, 2).PadLeft(6, '0');
    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateButtons();
        DetectCombo();
        comboTime += Time.deltaTime;
    }

    bool DidButtonChange()
    {
        return (previousButtons != buttons);
    }

    void DetectCombo()
    {
        if (DidButtonChange())
        {
            if (buttons == combo[comboIndex])
            {
                ++comboIndex;
                if ((uint)BUTTONS.COMBO_END == combo[comboIndex])
                {
                    Debug.Log("Combo ended");
                    comboIndex = 0;
                    textMesh.text = "HA DO KEN!!";
                    Invoke("ClearComboMessage", 1);
                    //Animacion final
                }
                else
                {
                    //Animacion intermedia
                }
            }
            else
            {
                Debug.Log("Error!!");
                textMesh.text = "ERROOOR";
                Invoke("ClearComboMessage", 1);
                comboIndex = 0;
            }
            comboTime = 0;
        }
        else 
        {
            if (comboTime > maxTimeBetweenInputs)
            {
                Debug.Log("Out of Time!!");

                comboIndex = 0;
            }
        }
    }

    void ClearComboMessage()
    {
        textMesh.text = "Introduce un combo -> -> ->";
    }
}
