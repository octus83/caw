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
        private MainWindow caller;

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
        public Data(String id, MainWindow caller)
        {
            this.caller = caller;
            db_caw = new DB_CAW();
            this.diagramm = db_caw.getDiagramm(id);
            this._sortedPageIdList = getSortedPageIdList();
        }

        public List<String> getSortedPageIdList()
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
        public String getPIDFromPagenumber(int number)
        {
            number = number - 1;
            foreach (var item in diagramm.pages_List)
            {
                if (number == item.PageInDiagramm)
                {
                    return item.P_id;
                }
            }
            return "";
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
        public byte[] getBlob(String id)
        {
            return db_caw.getBLOB(id);
        }
        public  BitmapImage createbitmapsource(byte[] imageBytes)
        {
            Console.WriteLine("Ausgabe Console ->"+imageBytes.Length);
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public void savaBitmapimageToFile(BitmapImage b, int page)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            String diagrammPath = System.IO.Path.Combine("D:\\caw", caller.ID);

            if(!System.IO.Directory.Exists(diagrammPath))
            {
            System.IO.Directory.CreateDirectory(diagrammPath);
            }
            String pagePath = System.IO.Path.Combine(diagrammPath, page+".jpg");
            if (!File.Exists(pagePath))
            {
                encoder.Frames.Add(BitmapFrame.Create(b));
                Console.WriteLine(pagePath);
                using (var filestream = new FileStream(pagePath, FileMode.Create))
                    encoder.Save(filestream);
            }
        }

        public void creatAllPictures(){
            List<CustomBitmapImage> list = db_caw.getAllBLOBs(diagramm.ID);
            foreach (var item in list)
            {
                savaBitmapimageToFile(item.OrginalImage, item.PageInDiagramm);
            }
            
        }

        public async void getBitmapList(MainWindow caller)
        {
            List<String> sortedPageIdList = SortedPageIdList;
            List<CustomBitmapImage> list = new List<CustomBitmapImage>();
            caller.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
         
            var progressDialog = await caller.ShowProgressAsync("Schaltplan wird geladen ....", "");
            progressDialog.SetProgress(0.0);

            await Task.Delay(200);
            
            double percent = 0.1 + (100 / sortedPageIdList.Count) / 100;
            int count = 1;

            foreach (var item in sortedPageIdList)
            {
                CustomBitmapImage cbi = new CustomBitmapImage();
                BitmapImage b = createbitmapsource(getBlob(item));
                cbi.OrginalImage = b;
                savaBitmapimageToFile(b, count);
                list.Add(cbi);
                progressDialog.SetMessage("Seite: " + count + " wurde geladen");
                progressDialog.SetProgress(0.99 / sortedPageIdList.Count * count);
                await Task.Delay(50);
                count++;
            }           
            progressDialog.SetMessage("fertig geladen");
            progressDialog.SetProgress(1.0);
            await Task.Delay(500);
            caller.Images = list;
            caller.onProjectOpenFinish();          
            await Task.Delay(100);
            await progressDialog.CloseAsync();
            MessageBox.Show("asyn finished");
        }

    }
}

