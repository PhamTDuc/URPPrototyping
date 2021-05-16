using Guinea.Core;

namespace Guinea.UI
{
    public class OptionsMenu : Menu
    {
        void OnSliderChanged(float value)
        {
            Commons.Log("Value of slider: " + value);
        }
    }
}