namespace ConlangJson
{
    public class SoundMap
    {
        private string _phoneme;
        private string _romanization;
        private string _spelling_regex;
        private string _pronounciation_regex;

        public SoundMap()
        {
            this._phoneme = string.Empty;
            this._romanization = string.Empty;
            this._spelling_regex = string.Empty;
            this._pronounciation_regex = string.Empty;
        }

        public SoundMap(string phoneme, string romanization, string spelling_regex, string pronounciation_regex)
        {
            this._phoneme = phoneme;
            this._romanization = romanization;
            this._spelling_regex = spelling_regex;
            this._pronounciation_regex = pronounciation_regex;
        }

        public string phoneme
        {
            get { return _phoneme; }
            set { _phoneme = value; }
        }

        public string romanization
        {
            get { return _romanization; }
            set { _romanization = value; }
        }

        public string spelling_regex
        {
            get { return _spelling_regex; }
            set { _spelling_regex = value;}
        }

        public string pronounciation_regex
        {
            get { return _pronounciation_regex; }
            set { _pronounciation_regex = value; }
        }
    }
}