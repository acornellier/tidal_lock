using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class DebugShortcuts : MonoBehaviour
{
    [Inject] PersistentDataManager _persistentDataManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                PersistentDataManager.Reset();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            _persistentDataManager.Save();
        }
    }
}