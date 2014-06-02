using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw
{
    class Data
    {
        Diagramm diagramm;

        public Data(String id)
        {
            DB_CAW db_caw = new DB_CAW();
            this.diagramm = db_caw.getDiagramm(id);        
        }

        public List<String> getPotentialNamesFromPageNumber()
        {
            List<String> list = new List<String>();

            foreach (var item in diagramm.pages_List)
            {
                list.Add(item.OriginNumber.ToString());
               
            }
            return list;
        
        }

    }
}
