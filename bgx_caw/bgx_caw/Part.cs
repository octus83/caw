using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw
{
    class Part
    {
        private String _p_id;
        private String _preprefix;
        private String _prefix;

        public String D_id;
        public String P_id;
        public String BMK;
        public String PrePreFix
        {
            get
            {
                if (_preprefix == null)
                {
                    return "";
                }
                else
                {
                    return _preprefix;
                }
            }
            set
            {
                _preprefix = value;
            }
        }
        public String PreFix
        {
            get
            {
                if (_prefix == null)
                {
                    return "";
                }
                else
                {
                    return _prefix;
                }
            }
            set
            {
                _prefix = value;
            }
        }
    }
}
