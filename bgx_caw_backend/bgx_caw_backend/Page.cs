using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw_backend
{
    public class Page
    {
        private String _author;
        public String D_id;
        public String P_id
        {
            get;
            set;
        }
        public String Title;

        public String PrePreFix;
        public String PreFix;
        public String OriginNumber;
        public String Author
        {
            get
            {
                if (_author == null)
                {
                    _author = "";
                }

                return _author;
            }
            set
            {
                _author = value;
            }
        }
        public String Source_Green;
        public String Source_Grey;
        public String Source_Red;
        public int Version;
        public DateTime Date_Init
        {
            get;
            set;
        }
        public DateTime Date_LastChange
        {
            get;
            set;
        }

        public List<Part> Parts_List;
        public List<Potential> Potential_List;


        public Page()
        {

        }

        public Page(String diagramm_id)
        {
            D_id = diagramm_id;
            P_id = Guid.NewGuid().ToString();

            Parts_List = new List<Part>();
            Potential_List = new List<Potential>();
        }
    }
}
