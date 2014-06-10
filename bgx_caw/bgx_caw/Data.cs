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

        public List<Potential> getPotentialFromPageNumber(int number)
        {

            foreach (var item in diagramm.pages_List)
            {
                if (item.PageInDiagramm == number)
                {
                    return item.Potential_List;
                }            
            }
            return new List<Potential>();
      /*      var erg = from pot in diagramm.pages_List
                       where pot.PageInDiagramm == number
                       select pot;
      
            return erg.ElementAt(0).Potential_List;*/
        
        }
        public List<Part> getPartFomPageNumber(int number)
        {
            foreach (var item in diagramm.pages_List)
            {
                if (item.PageInDiagramm == number)
                {
                    return item.Parts_List;
                }
            }
            return new List<Part>();
        }
        public List<Page> getPagenumbersFromPotentialNames(String name)
        {
            List<Page> list = new List<Page>();
            foreach (var itemA in diagramm.pages_List)
            {
                foreach (var itemB in itemA.Potential_List)
                {
                    if (itemB.Name.Equals(name))
                    {
                        list.Add(itemA);
                    }
                }   
            }
            return list;
        }

        public int getPageCout()
        {
            return diagramm.pages_List.Count;
        }


        public List<Part> getCompletePartList()
        {
            List<Part> list = new List<Part>();
            foreach (var itemA in  diagramm.pages_List)
	        {
                foreach (var itemB in itemA.Parts_List)
                {
                    list.Add(itemB);
                }
	        }
            return list;
        }

    }
}
