namespace Overwave.Utils
{
    [System.Serializable]
    public class TranslationData
    {
        public string english;
        public string russian;
        public string french;
        public string german;
        public string chinese;

        public string this[int index]
        {
            get => index switch
                {
                    1 => russian,
                    2 => french,
                    3 => german,
                    4 => chinese,
                    _ => english
                };
            set
            {
                switch (index)
                {
                    case 1: russian = value; break;
                    case 2: french = value; break;
                    case 3: german = value; break;
                    case 4: chinese = value; break;
                    default: english = value; break;
                }
            }
        }
    }
}