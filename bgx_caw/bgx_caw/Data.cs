using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace bgx_caw
{
    class Data
    {
        Diagramm diagramm;
        DB_CAW db_caw;
        List<String> sortedPageIdList;

        public Data(String id)
        {
            db_caw = new DB_CAW();
            this.diagramm = db_caw.getDiagramm(id);
            this.sortedPageIdList = getSortedPageIdList();
        }

        private List<String> getSortedPageIdList()
        {
            List<String> list = new List<String>();
            foreach (var item in diagramm.pages_List)
	        {
		        for (int i = 0; i <  diagramm.PageCount; i++)
                {
                    if(item.PageInDiagramm == i)
                    {
                        list.Add(item.P_id);
 
                    }
                }
	        }
            return list;
        }

        public List<Potential> getPotentialFromPageNumber(int number)
        {
            //pages starts at 0
            number -= 1;
            foreach (var item in diagramm.pages_List)
            {
                if (item.PageInDiagramm == (number))
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
            //pages starts at 0
            number -= 1;
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

        public List<BitmapImage> getBitmapList()
        {
            List<BitmapImage> list = new List<BitmapImage>();
            foreach (var item in sortedPageIdList)
            {

                list.Add(createbitmapsourceList(db_caw.getBLOB(item)));
            }
            return list;
        }
        private BitmapImage createbitmapsourceList(byte[] imageBytes)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }

    }
}

