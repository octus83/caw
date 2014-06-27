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

     
        public Data(String id, MainWindow caller)
        {
            this.caller = caller;
            db_caw = new DB_CAW();
            this.diagramm = db_caw.getDiagramm(id);          
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
        public List<Page> getPagenumbersFromPartlNames(String name)
        {
            List<Page> list = new List<Page>();
            foreach (var itemA in diagramm.pages_List)
            {
                foreach (var itemB in itemA.Parts_List)
                {
                    if (itemB.BMK.Equals(name))
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

        public CustomBitmapImage getBlob(String id)
        {
            return db_caw.getBLOB(id);
        }
        public CustomBitmapImage getBlobFast(String id)
        {
            return db_caw.getBLOBFast(id);
        }


        public void savaOrginalBitmapimageToFile(BitmapImage b, int page)
        {
            String p_id = getPIDFromPagenumber(page);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            String diagrammPath = System.IO.Path.Combine(caller.ProgrammPath, caller.DiagrammId);

            if(!System.IO.Directory.Exists(diagrammPath))
            {
            System.IO.Directory.CreateDirectory(diagrammPath);
            }
            String pagePath = System.IO.Path.Combine(diagrammPath, p_id);
          
            if (!System.IO.Directory.Exists(pagePath))
            {
                System.IO.Directory.CreateDirectory(pagePath);
            }

            pagePath = System.IO.Path.Combine(pagePath, page + "orginal.jpg");
            if (!File.Exists(pagePath))
            {
                try
                {
                    encoder.Frames.Add(BitmapFrame.Create(b));
                    Console.WriteLine(pagePath);
                    using (var filestream = new FileStream(pagePath, FileMode.Create))
                        encoder.Save(filestream);
                }
                catch (IOException)
                {
                    Console.WriteLine("Console -> IOException! Sleep for 500 ms and try again"); 
                    System.Threading.Thread.Sleep(500);
                    savaOrginalBitmapimageToFile(b, page);
                }
               
            }
           
     
        }

        public void savaCustomBitmapimageToFile(BitmapImage b, int page)
        {
            String p_id = getPIDFromPagenumber(page);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            String diagrammPath = System.IO.Path.Combine(caller.ProgrammPath, caller.DiagrammId);

            if (!System.IO.Directory.Exists(diagrammPath))
            {
                System.IO.Directory.CreateDirectory(diagrammPath);
            }

            String pagePathCustom = System.IO.Path.Combine(diagrammPath, p_id);
            if (!System.IO.Directory.Exists(pagePathCustom))
            {
                System.IO.Directory.CreateDirectory(pagePathCustom);
            }
            pagePathCustom = System.IO.Path.Combine(pagePathCustom, page + "custom.jpg");
            if (!File.Exists(pagePathCustom))
            {
                encoder.Frames.Add(BitmapFrame.Create(b));
                using (var filestream = new FileStream(pagePathCustom, FileMode.Create))
                    encoder.Save(filestream);
            }

        }
        public void saveCustomBLOBInDB( int page)
        {
            String path = caller.getCustomPicturesPath(page);
            String p_id = getPIDFromPagenumber(page);
            db_caw.writeCustomBlobToDatabase(GetPhoto(path), p_id);
        }

        public static byte[] GetPhoto(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte[] photo = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return photo;
        }
    }
}

