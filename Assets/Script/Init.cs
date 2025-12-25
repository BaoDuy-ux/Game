using UnityEngine;

public class Init : MonoBehaviour
{

    void Start()
    {
        GameObject selectedCharater = CharacterSelect.slectedCharacter;
        GameObject playerObject = Instantiate(selectedCharater, new Vector3(10f, 5f, 0f), Quaternion.identity);
        playerObject.name = "Player";
    }
  
}
