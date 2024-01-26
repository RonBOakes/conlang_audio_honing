using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConlangJson
{
    public class DerivationalAffix
    {
        private string? _type;
        private string? _pronounciation_add = null;
        private string? _spelling_add = null;
        private string? _pronounciation_regex = null;
        private string? _spelling_regex = null;
        private string? _t_pronounciation_add = null;
        private string? _t_spelling_add = null;    
        private string? _f_pronounciation_add = null;
        private string? _f_spelling_add = null;

        public DerivationalAffix(string? type, string? pronounciation_add, string? spelling_add, string? pronounciation_regex, string? spelling_regex, string? t_pronounciation_add, string? t_spelling_add, string? f_pronounciation_add, string? f_spelling_add)
        {
            this._type = type;
            this._pronounciation_add = pronounciation_add;
            this._spelling_add = spelling_add;
            this._pronounciation_regex = pronounciation_regex;
            this._spelling_regex = spelling_regex;
            this._t_pronounciation_add = t_pronounciation_add;
            this._t_spelling_add= t_spelling_add;
            this._f_pronounciation_add= f_pronounciation_add;
            this._f_spelling_add = f_spelling_add;
        }

        public DerivationalAffix()
        {
            type = "";
        }

        public string? type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string? pronounciation_add
        {
            get { return _pronounciation_add; }
            set { _pronounciation_add = value; }
        }

        public string? spelling_add
        {
            get { return _spelling_add; }
            set { _spelling_add = value; }
        }

        public string? pronounciation_regex
        {
            get { return _pronounciation_regex; }
            set { _pronounciation_regex = value; }
        }

        public string? spelling_regex
        {
            get { return _spelling_regex; }
            set { _spelling_regex = value; }
        }

        public string? t_pronounciation_add
        {
            get { return _t_pronounciation_add; }
            set { _t_pronounciation_add = value; }
        }

        public string? t_spelling_add
        {
            get { return _t_spelling_add; }
            set { _t_spelling_add = value; }
        }

        public string? f_pronounciation_add
        {
            get { return _f_pronounciation_add; }
            set { _f_pronounciation_add = value; }
        }

        public string? f_spelling_add
        {
            get { return _f_spelling_add; }
            set { _f_spelling_add = value;}
        }

    }
}
