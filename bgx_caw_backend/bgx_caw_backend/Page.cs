using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw_backend
{
    class Page
    {

        public String D_id;
        public String P_id
        {
            get;
            private set;
        }
        public String Title;
        public String PrePreFix;
        public String PreFix;
        public String OriginNumber;
        public String Author;
        public String Source_Green;
        public String Source_Grey;
        public String Source_Red;
        public int Version;
        public DateTime Date_Init;
        public DateTime Date_LastChange;

        public List<Part> Parts_List;
        public List<Potential> Potential_List;

        public Page(String diagramm_id)
        {
            D_id = diagramm_id;
            P_id = Guid.NewGuid().ToString();

            Parts_List = new List<Part>();
            Potential_List = new List<Potential>();
        }
    }
}
