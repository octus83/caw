﻿using System;
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
    /// <summary>
    /// Eigene Klasse die die Informationsdaten eines Projektes/Diagramms
    /// verwaltet und mit der Datenbank Klasse kommuniziert
    /// </summary>
    class Data
    {
        /// <summary>
        /// referenz auf ein diagramm object des aktuellen Projektes/Diagramms
        /// </summary>
        private Diagramm diagramm;
        /// <summary>
        /// referenz auf die Datenbank Klasse
        /// </summary>
        private DB_CAW db_caw;
        /// <summary>
        /// referenz auf die Hauptklasse (Main Window)
        /// </summary>
        private MainWindow caller;

         /// <summary>
         /// Constructor
         /// </summary>
         /// <param name="d_id">Projekt/Diagramm ID aus der Datenbank</param>
         /// <param name="caller">referenz </param>
        public Data(String d_id, MainWindow caller)
        {
            this.caller = caller;
            db_caw = new DB_CAW();
            this.diagramm = db_caw.getDiagramm(d_id);          
        }
      /// <summary>
      /// Liefert die Liste aller Potentiale einer Seite zurück
      /// </summary>
      /// <param name="number">Seitenzahl</param>
      /// <returns></returns>
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
        }
        /// <summary>
        /// Liefert die Liste aller Bauteile einer Seite zurück
        /// </summary>
        /// <param name="number">Seitenzahl</param>
        /// <returns></returns>
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
        /// <summary>
        /// Liefert eine Liste mit allen Page Objekten  
        /// von einem Potential Namen zurück
        /// </summary>
        /// <param name="name">Names des Potentials</param>
        /// <returns></returns>
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
        /// <summary>
        /// Liefert eine Liste mit allen Page Objekten
        /// von einem Bauteil Namen zurück
        /// </summary>
        /// <param name="name">Name des Bauteils</param>
        /// <returns></returns>
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
        /// <summary>
        /// Liefert die P_ID ( Primary Key) von einer
        /// Seitenzahl eines Projektes/Diagramms zurück
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Liefert die Anzahl der Seiten eines Projektes/Diagramms zurück
        /// </summary>
        /// <returns></returns>
        public int getPageCout()
        {
            return diagramm.pages_List.Count;
        }

        /// <summary>
        /// Liefert eine Liste mit allen Bauteilen
        /// eines Projektes/Diagramms zurück
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Lädt ein CustomBitmapImage Objekt mit Orginal und Custom Bild
        /// aus der Datenbank
        /// </summary>
        /// <param name="id">Seitenzahl ID</param>
        /// <returns></returns>
        public CustomBitmapImage getBlob(String id)
        {
            return db_caw.getBLOB(id);
        }
        /// <summary>
        /// Lädt ein CustomBitmapImage Objekt mit Orginal und Custom Bild
        /// aus der Datenbank mit Priorität
        /// </summary>
        /// <param name="id">Seitenzahl ID</param>
        /// <returns></returns>
        public CustomBitmapImage getBlobFast(String id)
        {
            return db_caw.getBLOBFast(id);
        }
        /// <summary>
        /// Speichert das BitmapImage als orginal jpg Datei im Programmverzeichnis
        /// in einem einzigartigen generierten Pfad
        /// </summary>
        /// <param name="b"></param>
        /// <param name="page"></param>
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
        /// <summary>
        /// Speichert das BitmapImage als custom jpg Datei im Programmverzeichnis
        /// in einem einzigartigen generierten Pfad
        /// </summary>
        /// <param name="b"></param>
        /// <param name="page"></param>
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
        /// <summary>
        /// schreibt ein erzeugtes custom Bild zurück in die Datenbank
        /// </summary>
        /// <param name="page"></param>
        public void saveCustomBLOBInDB( int page)
        {
            String path = caller.getCustomPicturesPath(page);
            String p_id = getPIDFromPagenumber(page);
            db_caw.writeCustomBlobToDatabase(GetPhoto(path), p_id);
        }
        /// <summary>
        /// Liest eine Foto Datei in ein Byte Array
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
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

