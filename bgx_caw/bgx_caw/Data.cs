using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Configuration;
using MahApps.Metro.Controls.Dialogs;

namespace bgx_caw
{
    class Data
    {
        private Diagramm diagramm;
        private DB_CAW db_caw;
        private List<String> _sortedPageIdList;
        public List<String> SortedPageIdList
        {
            set
            {
                this._sortedPageIdList = value;
            }
            get
            {
                return this._sortedPageIdList;
            }

        }

        public Data(String id)
        {
            db_caw = new DB_CAW();
            this.diagramm = db_caw.getDiagramm(id);
            this._sortedPageIdList = getSortedPageIdList();
        }

        private List<String> getSortedPageIdList()
        {
            List<String> list = new List<String>();
            for (int i = 0; i < diagramm.pages_List.Count; i++)
            {
                
          
            foreach (var item in diagramm.pages_List)
	        {
		            if(item.PageInDiagramm ==i)
                        list.Add(item.P_id);
 
                    
                
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

        public async void  getBitmapList(MainWindow caller)
        {
            caller.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            var progressDialog = await caller.ShowProgressAsync("Schaltplan wird geladen", "Diagram Ordner ");
            progressDialog.SetProgress(0.001);
            List<CustomBitmapImage> list = new List<CustomBitmapImage>();
            double percent = (100 / _sortedPageIdList.Count) / 100;
            int count = 0;
            foreach (var item in _sortedPageIdList)
            {
                count++;
                progressDialog.SetMessage("Page " + count + " wird geladen");
                CustomBitmapImage cbi = new CustomBitmapImage();
                cbi.OrginalImage = createbitmapsource(db_caw.getBLOB(item));
                list.Add(cbi);
                percent += percent;
                progressDialog.SetProgress(percent);
            }
            progressDialog.SetProgress(1.0);

            await progressDialog.CloseAsync();
            caller.Images= list;
            caller.onProjectOpenFinish();
        }
        public byte[] getBlob(String id)
        {
            return db_caw.getBLOB(id);
        }
        public  BitmapImage createbitmapsource(byte[] imageBytes)
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

