using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Handler : MonoBehaviour
{
    // There are 15 levels each consisting of
    // 1 opening dialouge
    // 1 puzzle
    // 1 ending dialouge

    public GameObject lvl_select_screen;
    public GameObject canvas;
    public GameObject title;
    public GameObject speech;
    public GameObject submit_button;
    public GameObject goal_box;
    public Sprite[] goal_by_lvl;

    private enum States { PLAY, OPENING, CLOSING, LVL_SELECT, TITLE_SCREEN };

    private Level_Updater lvl_updater;
    private Dot_Handler dot_handler;
    private int current_level;
    private Vector3 center;
    private Vector3 offscreen;
    private int state;

    private const int NUM_LVLS = 10;
    private Vector2[][] dot_pos_by_lvl;
    private string[][] opening_dialouge_by_lvl;
    private int speech_pos;
    private string[][] closing_dialouge_by_lvl;
    private char[][][][] win_condition_by_lvl;
    private List<Vector2> used_dots;
    private List<Vector2> temp_used_dots;

    // Start is called before the first frame update
    void Start()
    {
        dot_handler = GetComponent<Dot_Handler>();
        current_level = -1;
        used_dots = new List<Vector2>();
        temp_used_dots = new List<Vector2>();
        center = new Vector3(9f, 5f, 0f);
        offscreen = new Vector3(9f, 20f, 0f);
        lvl_select_screen.transform.position = offscreen;
        lvl_updater = lvl_select_screen.GetComponent<Level_Updater>();
        lvl_updater.Unlock_lvl(0);

        canvas.SetActive(false);
        submit_button.SetActive(false);
        goal_box.SetActive(false);
        state = (int)States.TITLE_SCREEN;
        title.SetActive(true);
        speech_pos = 0;

        // initialize lvl data!
        Initialize_Opening_Dialouge();
        Initialize_Dot_Positions();
        Initialize_Win_Condition();
        Initialize_Closing_Dialouge();
    }

    void Initialize_Win_Condition()
    {
        win_condition_by_lvl = new char[NUM_LVLS][][][];
        win_condition_by_lvl[0] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','x','x','x' },
                new char[] { 'o','o','x','x' },
                new char[] { 'o','o','o','x' },
                new char[] { 'o','o','o','o' }
            }
        };
        win_condition_by_lvl[1] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','o','o','o' },
                new char[] { 'o','o','o','o' },
                new char[] { 'o','o','o','o' },
                new char[] { 'o','o','o','o' }
            }
        };
        win_condition_by_lvl[2] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','o','o' },
                new char[] { 'o','o','o' },
                new char[] { 'o','o','o' }
            }
        };
        win_condition_by_lvl[3] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','o','o','o','o' },
                new char[] { 'o','o','o','o','o' },
                new char[] { 'o','o','o','o','o' },
                new char[] { 'o','o','o','o','o' },
                new char[] { 'o','o','o','o','o' }
            }
        };
        win_condition_by_lvl[4] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','o' },
                new char[] { 'o','o' },
                new char[] { 'o','o' }
            }
        };
        win_condition_by_lvl[5] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','o','o' },
                new char[] { 'o','o','o' },
                new char[] { 'o','o','o' },
                new char[] { 'o','o','o' }
            }
        };
        win_condition_by_lvl[6] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','x','x'},
                new char[] { 'o','o','x'},
                new char[] { 'o','o','o'}
            },
            new char[][]
            {
                new char[] { 'o','x','x'},
                new char[] { 'o','o','x'},
                new char[] { 'o','o','o'}
            },
            new char[][]
            {
                new char[] { 'o','x' },
                new char[] { 'o','o' }
            }
        };
        win_condition_by_lvl[7] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','x','x','x','x','x' },
                new char[] { 'o','o','x','x','x','x' },
                new char[] { 'o','o','o','x','x','x' },
                new char[] { 'o','o','o','o','x','x' },
                new char[] { 'o','o','o','o','o','x' },
                new char[] { 'o','o','o','o','o','o' }
            },
            new char[][]
            {
                new char[] { 'o','x','x','x','x' },
                new char[] { 'o','o','x','x','x' },
                new char[] { 'o','o','o','x','x' },
                new char[] { 'o','o','o','o','x' },
                new char[] { 'o','o','o','o','o' }
            }
        };
        win_condition_by_lvl[8] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','x','x','x','x','x','x','x' },
                new char[] { 'o','o','x','x','x','x','x','x' },
                new char[] { 'o','o','o','x','x','x','x','x' },
                new char[] { 'o','o','o','o','x','x','x','x' },
                new char[] { 'o','o','o','o','o','x','x','x' },
                new char[] { 'o','o','o','o','o','o','x','x' },
                new char[] { 'o','o','o','o','o','o','o','x' },
                new char[] { 'o','o','o','o','o','o','o','o' }
            }
        };
        win_condition_by_lvl[9] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','x','x','x','x' },
                new char[] { 'o','o','x','x','x' },
                new char[] { 'o','o','o','x','x' },
                new char[] { 'o','o','o','o','x' },
                new char[] { 'o','o','o','o','o' }
            }
        };
    }

    void Initialize_Dot_Positions()
    {
        dot_pos_by_lvl = new Vector2[NUM_LVLS][];
        dot_pos_by_lvl[0] = new Vector2[]
        {
            // horizontal line of 10 dots
            new Vector2(1, 9),
            new Vector2(2, 9),
            new Vector2(3, 9),
            new Vector2(4, 9),
            new Vector2(5, 9),
            new Vector2(6, 9),
            new Vector2(7, 9),
            new Vector2(8, 9),
            new Vector2(9, 9),
            new Vector2(10, 9)
        };
        dot_pos_by_lvl[1] = new Vector2[]
        {
            // triangle of hieght 3
            new Vector2(1, 9),
            new Vector2(1, 8),
            new Vector2(1, 7),
            new Vector2(2, 8),
            new Vector2(2, 7),
            new Vector2(3, 7),
            // triangle of hieght 4
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(6, 8),
            new Vector2(6, 7),
            new Vector2(6, 6),
            new Vector2(7, 7),
            new Vector2(7, 6),
            new Vector2(8, 6),

        };
        dot_pos_by_lvl[2] = new Vector2[]
        {
            // 1
            new Vector2(1, 9),
            // 3
            new Vector2(3, 9),
            new Vector2(3, 8),
            new Vector2(3, 7),
            // 5
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(5, 5)

        };
        dot_pos_by_lvl[3] = new Vector2[]
        {
            // 3 square
            new Vector2(1, 9),
            new Vector2(2, 9),
            new Vector2(3, 9),
            new Vector2(1, 8),
            new Vector2(2, 8),
            new Vector2(3, 8),
            new Vector2(1, 7),
            new Vector2(2, 7),
            new Vector2(3, 7),

            // 4 square
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(6, 9),
            new Vector2(6, 8),
            new Vector2(6, 7),
            new Vector2(6, 6),
            new Vector2(7, 9),
            new Vector2(7, 8),
            new Vector2(7, 7),
            new Vector2(7, 6),
            new Vector2(8, 9),
            new Vector2(8, 8),
            new Vector2(8, 7),
            new Vector2(8, 6)
        };
        dot_pos_by_lvl[4] = new Vector2[]
        {
            // triangle 1
            new Vector2(1, 9),
            new Vector2(2, 8),
            new Vector2(1, 8),
            // triangle 2
            new Vector2(4, 9),
            new Vector2(5, 8),
            new Vector2(4, 8)
        };
        dot_pos_by_lvl[5] = new Vector2[]
        {
            // 2
            new Vector2(1, 9),
            new Vector2(1, 8),
            // 4
            new Vector2(3, 9),
            new Vector2(3, 8),
            new Vector2(3, 7),
            new Vector2(3, 6),
            // 6
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(5, 5),
            new Vector2(5, 4)
        };
        dot_pos_by_lvl[6] = new Vector2[]
        {
            // 15
            // row 1
            new Vector2(1, 9),
            new Vector2(1, 8),
            new Vector2(1, 7),
            new Vector2(1, 6),
            new Vector2(1, 5),
            // row 2
            new Vector2(2, 9),
            new Vector2(2, 8),
            new Vector2(2, 7),
            new Vector2(2, 6),
            new Vector2(2, 5),
            // row 3
            new Vector2(3, 9),
            new Vector2(3, 8),
            new Vector2(3, 7),
            new Vector2(3, 6),
            new Vector2(3, 5),
        };
        dot_pos_by_lvl[7] = new Vector2[]
        {
            // 6^2
            // row 1
            new Vector2(1, 9),
            new Vector2(1, 8),
            new Vector2(1, 7),
            new Vector2(1, 6),
            new Vector2(1, 5),
            new Vector2(1, 4),
            // row 2
            new Vector2(2, 9),
            new Vector2(2, 8),
            new Vector2(2, 7),
            new Vector2(2, 6),
            new Vector2(2, 5),
            new Vector2(2, 4),
            // row 3
            new Vector2(3, 9),
            new Vector2(3, 8),
            new Vector2(3, 7),
            new Vector2(3, 6),
            new Vector2(3, 5),
            new Vector2(3, 4),
            // row 4
            new Vector2(4, 9),
            new Vector2(4, 8),
            new Vector2(4, 7),
            new Vector2(4, 6),
            new Vector2(4, 5),
            new Vector2(4, 4),
            // row 5
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(5, 5),
            new Vector2(5, 4),
            // row 6
            new Vector2(6, 9),
            new Vector2(6, 8),
            new Vector2(6, 7),
            new Vector2(6, 6),
            new Vector2(6, 5),
            new Vector2(6, 4),
        };
        dot_pos_by_lvl[8] = new Vector2[]
        {
            // 6^2
            // row 1
            new Vector2(1, 9),
            new Vector2(1, 8),
            new Vector2(1, 7),
            new Vector2(1, 6),
            new Vector2(1, 5),
            new Vector2(1, 4),
            // row 2
            new Vector2(2, 9),
            new Vector2(2, 8),
            new Vector2(2, 7),
            new Vector2(2, 6),
            new Vector2(2, 5),
            new Vector2(2, 4),
            // row 3
            new Vector2(3, 9),
            new Vector2(3, 8),
            new Vector2(3, 7),
            new Vector2(3, 6),
            new Vector2(3, 5),
            new Vector2(3, 4),
            // row 4
            new Vector2(4, 9),
            new Vector2(4, 8),
            new Vector2(4, 7),
            new Vector2(4, 6),
            new Vector2(4, 5),
            new Vector2(4, 4),
            // row 5
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(5, 5),
            new Vector2(5, 4),
            // row 6
            new Vector2(6, 9),
            new Vector2(6, 8),
            new Vector2(6, 7),
            new Vector2(6, 6),
            new Vector2(6, 5),
            new Vector2(6, 4),
        };
        dot_pos_by_lvl[9] = new Vector2[]
        {
            // 3 long hexagon
            new Vector2(3, 9),
            new Vector2(4, 9),
            new Vector2(5, 9),

            new Vector2(2, 8),
            new Vector2(1, 7),
            new Vector2(2, 6),

            new Vector2(6, 8),
            new Vector2(7, 7),
            new Vector2(6, 6),

            new Vector2(3, 5),
            new Vector2(4, 5),
            new Vector2(5, 5),

            // inner hexagon
            new Vector2(3, 7),
            new Vector2(4, 7),
            new Vector2(5, 6),
        };
    }

    void Initialize_Closing_Dialouge()
    {
        closing_dialouge_by_lvl = new string[NUM_LVLS][];
        closing_dialouge_by_lvl[0] = new string[]
        {
            "Good Job!",
            "This triangle is called the tetractys.",
            "It represents the four elements: fire, water, air and earth."
        };
        closing_dialouge_by_lvl[1] = new string[]
        {
            "Good work.",
            "isn't it amazing how these shapes fit together!"
        };
        closing_dialouge_by_lvl[2] = new string[]
        {
            "Nice job.",
            "Did you notice why this will always work?"
        };
        closing_dialouge_by_lvl[3] = new string[]
        {
            "Perfect!",
            "Sadly, this method will not work for any two arbitraty squares.",
            "The ones that do work I call Pyhagorean Triples.",
            "The 3 square and 4 square is one of the simplest and most beautiful examples!"
        };
        closing_dialouge_by_lvl[4] = new string[]
        {
            "Looks good!",
            "Oblong numbers can also be thought of as the product of any two consecutive whole numbers."
        };
        closing_dialouge_by_lvl[5] = new string[]
        {
            "Excelent!",
            "There are often many differnt ways to construct the same number."
        };
        closing_dialouge_by_lvl[6] = new string[]
        {
            "Good work!"
        };
        closing_dialouge_by_lvl[7] = new string[]
        {
            "Nicely done!",
            "These types of constructions are always reversable."
        };
        closing_dialouge_by_lvl[8] = new string[]
        {
            "Good work!",
            "Numbers like these are called square trianglular numbers and they are somewhat rare.",
            "The next such number is 1,225 which is a 35 by 35 square and a 49 high triangle number."
        };
        closing_dialouge_by_lvl[9] = new string[]
        {
            "Congragulations!",
            "You've learned everything I have to teach you about polygonal numbers!"
        };
    }

    void Initialize_Opening_Dialouge()
    {
        opening_dialouge_by_lvl = new string[NUM_LVLS][];
        opening_dialouge_by_lvl[0] = new string[]
        {
            "Hello and wellcome to the temple of the pythagoreans.",
            "As a newcommer you will not be allowed to speak for 3 years.",
            "But don't fret, I have much to teach you about numbers.",
            "Let us start with a simple triangle number",
            "triangle numbers are constructed by arranging dots",
            "place one dot, then two dots, then three and so on on top of eachother to form a triangle.",
            "Start by arranging these 10 dots into a triangle number of height 4."
        };
        opening_dialouge_by_lvl[1] = new string[]
        {
            "Next we will take a look at a square number.",
            "Every square number can be constructed by rearranging two triangle numbers with consecutive hieghts.",
            "Constuct a 4 by 4 square number using a 3 high triangle and a 4 high triangle.",
        };
        opening_dialouge_by_lvl[2] = new string[]
        {
            "Another way to construct all of the square numbers is by adding up odd numbers.",
            "By taking the first n consecutive odd numbers you can construct an n by n square.",
            "Take the numbers 1, 3, and 5 and construct a 3 by 3 square."
        };
        opening_dialouge_by_lvl[3] = new string[]
        {
            "The most interesting way to construct square numbers I have found is by combining two smaller squares.",
            "Let's combine a 3 square and a 4 square into a 5 square.",
        };
        opening_dialouge_by_lvl[4] = new string[]
        {
            "When you add two identical triangle numbers you do not get a square number.",
            "Instead you get what I call an Oblong number.",
            "Oblong numbers are 1 taller then they are wide.",
            "Use two 2 high triangle numbers to construct a 2 wide Oblong number."
        };
        opening_dialouge_by_lvl[5] = new string[]
        {
            "Another way to construct an Oblong number is with even numbers.",
            "By taking the first n even numbers you can construct an n wide Oblong number.",
            "Use 2, 4, and 6 to construct a 3 wide Oblong number."
        };
        opening_dialouge_by_lvl[6] = new string[]
        {
            "Any number can be expressed as the total of 3 or less triangle numbers.",
            "Construct the three triangles that make up 15."
        };
        opening_dialouge_by_lvl[7] = new string[]
        {
            "As we have seen, square numbers can be constructed from consecutive triangle numbers.",
            "This means we should be able to construct 2 triangle numbers from a given square number.",
            "Construct the consecutive triangle numbers that make up a 6 by 6 square number."
        };
        opening_dialouge_by_lvl[8] = new string[]
        {
            "There are some special numbers are both square numbers and triangle numbers.",
            "For example the 6 by 6 square number is equivalent to the 8 high tringle which is equal to 36.",
            "Demonstrate this by constructing the 8 high triangle from from the 6 by 6 square."
        };
        opening_dialouge_by_lvl[9] = new string[]
        {
            "There is a sense in which every polygon has a corisponding set of numbers.",
            "We've seen triangle and square numbers, but there are also pentagon, hexagon and infinitely more.",
            "Let's take a look at a hexagon number. All hexagon numbers are also triangle numbers.",
            "Construct a triangle number from this hexagon number."
        };
    }

    public void Start_Opening_Speech(int lvl_ID)
    {
        current_level = lvl_ID;
        canvas.SetActive(true);
        lvl_select_screen.transform.position = offscreen;
        speech_pos = 0;
        speech.GetComponent<Text>().text = opening_dialouge_by_lvl[current_level][speech_pos];
        submit_button.SetActive(false);
        goal_box.SetActive(false);

        state = (int)States.OPENING;
    }

    public void Start_Closing_Speech(int lvl_ID)
    {
        current_level = lvl_ID;
        canvas.SetActive(true);
        lvl_select_screen.transform.position = offscreen;
        speech_pos = 0;
        speech.GetComponent<Text>().text = closing_dialouge_by_lvl[current_level][speech_pos];
        speech_pos++;
        submit_button.SetActive(false);
        goal_box.SetActive(false);

        state = (int)States.CLOSING;
    }

    public void Start_lvl(int lvl_ID)
    {
        current_level = lvl_ID;
        dot_handler.place_dots(dot_pos_by_lvl[lvl_ID]);
        dot_handler.play();
        lvl_select_screen.transform.position = offscreen;
        canvas.SetActive(false);
        submit_button.SetActive(true);
        goal_box.SetActive(true);
        goal_box.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = goal_by_lvl[lvl_ID];
        submit_button.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);

        state = (int)States.PLAY;
    }

    public void End_lvl(int lvl_ID)
    {
        current_level = -1;
        dot_handler.stop();
        lvl_select_screen.transform.position = center;
        canvas.SetActive(false);
        submit_button.SetActive(false);
        goal_box.SetActive(false);
        dot_handler.Remove_Dots();

        lvl_updater.Complete_lvl(lvl_ID);
        if (lvl_ID != NUM_LVLS - 1)
        {
            lvl_updater.Unlock_lvl(lvl_ID + 1);
        }

        state = (int)States.LVL_SELECT;
    }

    public void Start_lvl_select(int lvl_ID)
    {
        current_level = -1;
        dot_handler.stop();
        lvl_select_screen.transform.position = center;
        canvas.SetActive(false);
        title.SetActive(false);
        goal_box.SetActive(false);
        submit_button.SetActive(false);

        state = (int)States.LVL_SELECT;
    }

    public bool Check_Answer(char[][] key)
    {
        Vector2[] dot_positions = dot_handler.Get_Dot_Positions();
        bool win = false;
        bool dot_in_position = false;
        foreach (Vector2 pos in dot_positions)
        {
            win = true;
            for (int row = 0; row < key.Length; row++)
            {
                for (int col = 0; col < key[row].Length; col++)
                {
                    float x = pos.x + col;
                    float y = pos.y - row;
                    dot_in_position = false;
                    // win = true if every dot in the win condition corrisponds to a dot in dot_positions
                    foreach (Vector2 pos2 in dot_positions)
                    {
                        if (pos2.x == x && pos2.y == y && !used_dots.Contains(pos2))
                        {
                            dot_in_position = true;
                            temp_used_dots.Add(pos2);
                        }
                    }
                    if (   (!dot_in_position && key[row][col] == 'o') 
                        || (dot_in_position && key[row][col] == 'x' ) )
                    {
                        win = false;
                        temp_used_dots.Clear();
                        break;
                    }
                }
                if (win == false)
                {
                    break;
                }
            }
            if (win == true)
            {
                foreach (var dot in temp_used_dots)
                {
                    used_dots.Add(dot);
                }
                temp_used_dots.Clear();
                return win;
            }
        }
        return win;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == (int)States.OPENING)
        {
            if (Input.GetButtonDown("Fire1") && speech_pos < opening_dialouge_by_lvl[current_level].Length)
            {
                speech.GetComponent<Text>().text = opening_dialouge_by_lvl[current_level][speech_pos];
                speech_pos++;
            }
            else if (Input.GetButtonDown("Fire1") && speech_pos == opening_dialouge_by_lvl[current_level].Length)
            {
                Start_lvl(current_level);
            }
        }
        else if (state == (int)States.CLOSING)
        {
            if (Input.GetButtonDown("Fire1") && speech_pos < closing_dialouge_by_lvl[current_level].Length)
            {
                speech.GetComponent<Text>().text = closing_dialouge_by_lvl[current_level][speech_pos];
                speech_pos++;
            }
            else if (Input.GetButtonDown("Fire1") && speech_pos == closing_dialouge_by_lvl[current_level].Length)
            {
                End_lvl(current_level);
            }
        }
        else if (state == (int)States.PLAY)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Submit")
                    {

                        bool is_correct = true;
                        used_dots.Clear();
                        foreach (var win_condition in win_condition_by_lvl[current_level])
                        {
                            if (!Check_Answer(win_condition))
                                is_correct = false;
                        }
                        if (is_correct)
                        {
                            // win state
                            transform.GetChild(0).GetComponent<AudioSource>().Play();
                            Start_Closing_Speech(current_level);
                        }
                        else
                        {
                            // lose state
                            transform.GetChild(3).GetComponent<AudioSource>().Play();
                        }
                    }
                }
            }
        }
    }
}