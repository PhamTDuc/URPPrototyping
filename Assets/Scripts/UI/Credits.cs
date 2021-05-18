using UnityEngine;
using UnityEngine.SceneManagement;
namespace Guinea.UI
{
    public class Credits : MonoBehaviour
    {
        public void Back()
        {
            SceneManager.LoadScene(0);
        }
    }
}