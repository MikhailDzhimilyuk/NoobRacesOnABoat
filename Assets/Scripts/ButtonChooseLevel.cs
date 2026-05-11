using UnityEngine;

public class ButtonChooseLevel : MonoBehaviour
{
    ChooseLevelController chooseLevelController;

    private void Start()
    {
        chooseLevelController = transform.parent.GetComponent<ChooseLevelController>();
    }

    public void ChooseLevel()
    {
        GameWorld.currentLevel = int.Parse(name) - 1;
        chooseLevelController.SpawnLevel();
        PauseController.SetUnpause();
    }
}
