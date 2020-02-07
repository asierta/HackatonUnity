using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ComboNode = System.Collections.Generic.Dictionary<uint, uint>;
public class ComboDetectorPro : MonoBehaviour
{

    Animator myAnimator;

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
    public float maxTimeBetweenInputs = 2; //Deberia ser el tiempo que dura la animación de cada ataque

    uint buttons = 0;
    uint previousButtons = 0;

    string[] messages;
    string[] animationVariables;

    uint comboIndex = 0;
    int animationIndex = 0;
    uint[] combo;
    List<uint[]> combos = new List<uint[]>();
    List<ComboNode> nodes = new List<ComboNode>();

    float comboTime = 0; //Count time between inputs

    // Start is called before the first frame update
    void Start()
    {

        myAnimator = this.GetComponent<Animator>();

        combo = new uint[] {
            (uint)BUTTONS.RIGHT, 0,
            (uint)BUTTONS.RIGHT, 0,
            (uint)BUTTONS.RIGHT, 0,
            (uint)BUTTONS.COMBO_END
        };
        textMesh.text = "Introduce un combo -> -> ->";

        combos.Add(new uint[] {
            (uint)BUTTONS.RIGHT, 0,
            (uint)BUTTONS.RIGHT, 0,
            (uint)BUTTONS.RIGHT, 0,
            (uint)BUTTONS.COMBO_END});

        combos.Add(new uint[] {
            (uint)BUTTONS.RIGHT, 0,
            (uint)BUTTONS.UP, 0,
            (uint)BUTTONS.RIGHT, 0,
            (uint)BUTTONS.COMBO_END + 1});

        combos.Add(new uint[] {
            (uint)BUTTONS.LEFT, 0,
            (uint)BUTTONS.LEFT, 0,
            (uint)BUTTONS.DOWN, 0,
            (uint)BUTTONS.COMBO_END + 2});

        combos.Add(new uint[] {
            (uint)BUTTONS.LEFT, 0,
            (uint)BUTTONS.UP, 0,
            (uint)BUTTONS.DOWN, 0,
            (uint)BUTTONS.DOWN, 0,
            (uint)BUTTONS.COMBO_END + 3});

        messages = new string[] {
            "Combo1 derecha-derecha-derecha",
            "Combo2 derecha-arriba-derecha",
            "Combo3 izquierda-izquierda-abajo",
            "Combo4 izquierda-arriba-abajo-abajo"
        };

        animationVariables = new string[] {
            "punch",
            "hook",      
        };

        BuildNodes();
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

        if (previousButtons != buttons)
        {
    
        
        }
        // textMesh.text = System.Convert.ToString(buttons, 2).PadLeft(6, '0');
    }



    void FinishRecovery()
    {

        myAnimator.SetBool(animationVariables[animationIndex], false);
        print("VOlvi al idle");
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

    void BuildNodes()
    {
        uint nodeCounter = 0;

        nodes.Add(new ComboNode()); //Empty root node

        for (int row = 0; row < combos.Count; ++row)
        {
            nodeCounter = 0;

            uint[] currentCombo = combos[row];

            for (int i = 0; i < currentCombo.Length - 1; ++i)
            {
                //Si el elemento del combo actual no existe en el diccionario
                if (!nodes[(int)nodeCounter].ContainsKey(currentCombo[i]))
                {
                    //Si no es el final
                    if (i < currentCombo.Length - 2)
                    {
                        //Agregarlo
                        nodes[(int)nodeCounter].Add(currentCombo[i], (uint)nodes.Count);

                        nodeCounter = (uint)nodes.Count;

                        //Agregar nuevo elemento a la lista (diccionario vacio)
                        nodes.Add(new ComboNode());
                    }
                    else //Final del combo
                    {
                        nodes[(int)nodeCounter].Add(currentCombo[i], currentCombo[i + 1]);
                    }
                }
                else //El elemento ya esta incluido, pasamos al estado al que conduce la llave actual ?
                {
                    nodeCounter = nodes[(int)nodeCounter][currentCombo[i]];
                }
            }
        }
    }

    void DetectCombo()
    {
        if (DidButtonChange())
        {

            if (buttons == (uint)BUTTONS.Z)
            {
                print("boton z");
            }
            


            if (nodes[(int)comboIndex].ContainsKey(buttons))
            {
                //Pasar al siguiente nodo
                comboIndex = nodes[(int)comboIndex][buttons];


             


                if (comboIndex >= (uint)BUTTONS.COMBO_END)
                {
                    Debug.Log(comboIndex);
                    textMesh.text = messages[comboIndex - (uint)BUTTONS.COMBO_END];
                    Invoke("ClearComboMessage", 1);
                    comboIndex = 0;
                  
                    ResetButtons();
                    
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
                ResetButtons();
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

    void ResetButtons()
    {
        previousButtons = 0;
        buttons = 0;
    }

    void ClearComboMessage()
    {
        textMesh.text = "Introduce un combo -> -> ->";
    }
}
