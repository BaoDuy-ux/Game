using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    private int index;
    [SerializeField] GameObject[] characters;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      index = 0;
        SelectCharacter();
    }

   public void BackBtnClick()
    {
        if (index > 0)
        {
            index--;
        }
        SelectCharacter();
    }
    public void NextBtnClick()
    {
        if (index < characters.Length - 1)
        {
            index++;
        }
        SelectCharacter();
    }
        private void SelectCharacter()
        {
            for(int i=0; i< characters.Length; i++)
            {
                if (i == index)
                {
                    characters[i].GetComponent<SpriteRenderer>().color = Color.white;
                    characters[i].GetComponent<Animator>().enabled = true;
                }
                else
                {
                characters[i].GetComponent<SpriteRenderer>().color = Color.black;
                characters[i].GetComponent<Animator>().enabled = false;
            }
            }
        }
} 


  

