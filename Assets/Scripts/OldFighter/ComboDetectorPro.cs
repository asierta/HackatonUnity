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
    bool next = true;

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
            "Attack1",
            "Attack4",
            "Attack3"
        };

        BuildNodes();
    }

    //Actualiza los bits correspondientes a cada una de las teclas, poniendo a 1 las presionadas y a 0 las que no
    void UpdateButtons()
    {
        previousButtons = buttons; //Save buttons previous state
        buttons = 0;

        buttons |= Input.GetKeyDown(KeyCode.UpArrow) == true ? (uint)(BUTTONS.UP) : 0;
        buttons |= Input.GetKeyDown(KeyCode.DownArrow) == true ? (uint)(BUTTONS.DOWN) : 0;
        buttons |= Input.GetKeyDown(KeyCode.LeftArrow) == true ? (uint)(BUTTONS.LEFT) : 0;
        buttons |= Input.GetKeyDown(KeyCode.RightArrow) == true ? (uint)(BUTTONS.RIGHT) : 0;

        buttons |= Input.GetKeyDown(KeyCode.Z) == true ? (uint)(BUTTONS.Z) : 0;
        buttons |= Input.GetKeyDown(KeyCode.X) == true ? (uint)(BUTTONS.X) : 0;

        if (previousButtons != buttons)
        {


        }
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
        if (DidButtonChange() && next)
        {
            if (nodes[(int)comboIndex].ContainsKey(buttons))
            {
                //Pasar al siguiente nodo
                comboIndex = nodes[(int)comboIndex][buttons];
                if (buttons != 0)
                {
                    if (animationIndex == 2)
                    {
                        if (buttons == (uint)BUTTONS.RIGHT)
                        {
                            myAnimator.SetBool("Attack4", true);
                        }
                        else if (buttons == (uint)BUTTONS.UP)
                        {
                            myAnimator.SetBool("Attack2", true);
                        }
                    }
                    else 
                    { 
                        myAnimator.SetBool(animationVariables[animationIndex], true);
                    }
                    ++animationIndex;
                    next = false;
                }

                if (comboIndex >= (uint)BUTTONS.COMBO_END)
                {
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
                animationIndex = 0;
                next = true;
                ResetButtons();
            }
            comboTime = 0;
        }
        else
        {
            //if (comboTime > maxTimeBetweenInputs)
            //{
            //    Debug.Log("Out of Time!!");
            //    comboIndex = 0;
            //    animationIndex = 0;
            //    next = true;
            //}
        }
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

    public void HabilitarNext(float time)
    {
        next = true;
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

    public void return1()
    {
        if (animationIndex >= 2)
        {
            if (buttons == (uint)BUTTONS.RIGHT)
            {
                myAnimator.SetBool("Attack4", true);
            }
            else if (buttons == (uint)BUTTONS.UP)
            {
                myAnimator.SetBool("Attack2", true);
            }
        }
        else
        {
            myAnimator.SetBool("Attack1", false);
            animationIndex = 0;
        }
    }

    public void return2()
    {
        if (animationIndex >= 3)
        {
            myAnimator.SetBool("Attack3", true);
        }
        else
        {
            myAnimator.SetBool("Attack4", false);
            myAnimator.SetBool("Attack2", false);
            myAnimator.SetBool("Attack1", false);
            animationIndex = 0;
        }
    }

    public void return3()
    {
        myAnimator.SetBool("Attack1", false);
        myAnimator.SetBool("Attack2", false);
        myAnimator.SetBool("Attack3", false);
        myAnimator.SetBool("Attack4", false);
        animationIndex = 0;
    }

    public void return4()
    {
        if (animationIndex >= 3)
        {
            myAnimator.SetBool("Attack4", true);
        }
        else
        {
            myAnimator.SetBool("Attack4", false);
            myAnimator.SetBool("Attack2", false);
            myAnimator.SetBool("Attack1", false);
            animationIndex = 0;
        }
    }
}
