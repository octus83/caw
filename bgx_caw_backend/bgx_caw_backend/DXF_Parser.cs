﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace bgx_caw_backend
{
    class DXF_Parser
    {
        private int pageCounter = 0;

        public Diagramm Diagramm
        {
            private set;
            get;
        }
        
        private static bool FindAttribute(String attr)
        {
            if (attr == "AcDbAttribute")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DXF_Parser(DirectoryInfo folderPath)
        {
            Diagramm = new Diagramm {   
                                        ID = Guid.NewGuid().ToString(),
                                        Date_Init = DateTime.Now,
                                        Date_LastChange = DateTime.Now
                                    };

            foreach (FileInfo file in folderPath.GetFiles().OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f.ToString()))))
            {
                if(file.Extension == ".dxf")
                {
                    StreamReader fileReader = new StreamReader(file.FullName);
                    String line;
                    List<String> fileRows = new List<String>();
                    
                    if(Diagramm.pages_List == null)
                    {
                        Diagramm.pages_List = new List<Page>();
                    }

                    Diagramm.pages_List.Add(new Page{ 
                                                        D_id = Diagramm.ID,
                                                        PageInDiagramm = pageCounter
                                                    }); 

                    while ((line = fileReader.ReadLine()) != null) //Add Row to RowsList
                    {
                        fileRows.Add(line);
                    }

                    if (Path.GetFileNameWithoutExtension(file.Name.ToString()) == "1" || Path.GetFileNameWithoutExtension(file.Name.ToString()) == "0") //wenn Deckblatt
                    {
                        getMainData(fileRows); //Lese Deckblatt aus
                    }
                    else //Wenn nicht, lese Schaltplan-Daten aus
                    {
                        getBlocks(fileRows);
                    }

                    pageCounter++;
                }              
            }            
        }

        /// <summary>
        /// Parses the MainData from first Page of DXF-Diagramm
        /// </summary>
        /// <param name="rowsList">List of String to check for Blocks</param>
        private void getMainData(List<String> rowsList)
        {
            for (int i = 1; i < rowsList.Count; i++)
            {
                switch (rowsList[i])
                {
                    case "EPLAN451": //Sondertexte.Projekte

                        if (int.Parse(rowsList[i + 1]) == 62) //wenn echter block und nicht definition
                        {
                            int filecount = rowsList.FindIndex(i, 60, FindAttribute); //finde Attribute

                            if (filecount >= 0)
                            {
                                switch (rowsList[filecount + 2])
                                {
                                    case "14_10011":    //Feldname
                                        //MessageBox.Show("Seite " + pageCounter + " hat den Feldnamen " + rowsList[i + 14]);
                                        Diagramm.FieldName = rowsList[i + 14];
                                        break;

                                    case "14_40001":    //Projektnummer
                                        //MessageBox.Show("Seite " + pageCounter + " hat die Projektnummer " + rowsList[i + 14]);
                                        Diagramm.ProjectNumber = rowsList[i + 14];
                                        break;

                                    case "14_40002":    //Auftragsnummer
                                        //MessageBox.Show("Seite " + pageCounter + " hat die Auftragsnummer " + rowsList[i + 14]);
                                        Diagramm.JobNumber = rowsList[i + 14];
                                        break;

                                    case "14_40003":    //Kunde
                                        //MessageBox.Show("Seite " + pageCounter + " hat den Kunden " + rowsList[i + 14]);
                                        Diagramm.Customer = rowsList[i + 14];
                                        break;

                                    case "14_40004":    //Endkunde
                                        //MessageBox.Show("Seite " + pageCounter + " hat den Endkunden " + rowsList[i + 14]);
                                        Diagramm.EndCustomer = rowsList[i + 14];
                                        break;

                                    case "14_40005":    //Aufstellungsort Zeile 1
                                        //MessageBox.Show("Seite " + pageCounter + " hat den Aufstellungsort 1 " + rowsList[i + 14]);
                                        Diagramm.AddressRow1 = rowsList[i + 14];
                                        break;

                                    case "14_40005/1":  //Aufstellungsort Zeile 2
                                        //MessageBox.Show("Seite " + pageCounter + " hat den Aufstellungsort 2 " + rowsList[i + 14]);
                                        Diagramm.AddressRow2 = rowsList[i + 14];
                                        break;

                                    case "14_40005/2":  //Aufstellungsort Zeile 3
                                        //MessageBox.Show("Seite " + pageCounter + " hat den Aufstellungsort 3 " + rowsList[i + 14]);
                                        Diagramm.AddressRow3 = rowsList[i + 14];
                                        break;
                                }
                            }
                            else
                            {
                                throw new Exception("Der echte Block in Zeile " + i + " hat kein Attribute");
                            }                                                       
                        }
                        break;
                    case "EPLAN452":
                        if (rowsList[i + 1] == " 62") //wenn echter block und nicht definition
                        {
                            int filecount = rowsList.FindIndex(i, FindAttribute); //finde Attribute

                            if(filecount >= 0)
                            {
                                switch (rowsList[filecount + 2])
                                {
                                    case "4_40101":
                                        //MessageBox.Show("Seite " + pageCounter + " hat den Konstrukteur " + rowsList[i + 14]);
                                        Diagramm.pages_List[pageCounter].Author = rowsList[i + 14];
                                        break;

                                    case "4_40107":     //Projektname
                                        //MessageBox.Show("Seite " + pageCounter + " hat den Projektnamen " + rowsList[i + 14]);
                                        Diagramm.ProjectName = rowsList[i + 14];
                                        break;

                                    case "4_40108":     //Seriennummer
                                        //MessageBox.Show("Seite " + pageCounter + " hat die Seriennummer " + rowsList[i + 14]);
                                        Diagramm.SerialNumber = rowsList[i + 14];
                                        break;

                                    case "4_11011":
                                        //MessageBox.Show("Seite" + pageCounter + " hat den Titel " + rowsList[i + 14]);
                                        Diagramm.pages_List[pageCounter].Title = rowsList[i + 14];
                                        break;

                                    case "4_1120":
                                        //MessageBox.Show("Seite" + pageCounter + " hat das Anlagenkennzeichen " + rowsList[i + 14]);
                                        Diagramm.pages_List[pageCounter].PrePreFix = rowsList[i + 14];
                                        break;

                                    case "4_1220":
                                        //MessageBox.Show("Seite" + pageCounter + " hat das Ortskennzeichen " + rowsList[i + 14]);
                                        Diagramm.pages_List[pageCounter].PreFix = rowsList[i + 14];
                                        break;

                                    case "4_11000":
                                        //MessageBox.Show("Seite" + pageCounter + " hat die Seitenzahl " + rowsList[i + 14]);
                                        Diagramm.pages_List[pageCounter].OriginNumber = rowsList[i + 14];
                                        break;
                                }
                            }
                            else
                            {
                                throw new Exception("Der echte Block in Zeile " + i + " hat kein Attribute");
                            }                           
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Parses the Information about Parts and Potentials from regular Pages
        /// </summary>
        /// <param name="rowsList">List of String to check for Blocks</param>
        private void getBlocks(List<String> rowsList)
        {
            for(int i = 1; i < rowsList.Count; i++)
            {
                switch (rowsList[i])
                {
                    case "EPLAN400":
                        if (rowsList[i + 1] == " 62") //wenn echter block und nicht definition
                        {
                            int filecount = rowsList.FindIndex(i, FindAttribute); //finde Attribute

                            if (rowsList[filecount + 2] == "17_20010") //Wenn Bauteil-BMK (sichtbar)
                            {                              
                                if (Diagramm.pages_List[pageCounter].Parts_List == null)
                                {
                                    Diagramm.pages_List[pageCounter].Parts_List = new List<Part>();
                                }

                                if (!String.IsNullOrEmpty(rowsList[i + 14]))
                                {
                                    if(!Diagramm.pages_List[pageCounter].Parts_List.Any(part => part.BMK == rowsList[i + 14]))
                                    {
                                        //MessageBox.Show("Seite " + pageCounter + " hat das Bauteil " + rowsList[i + 14]);

                                        Diagramm.pages_List[pageCounter].Parts_List.Add(new Part
                                        {
                                            D_id = Diagramm.ID,
                                            P_id = pageCounter.ToString(),
                                            BMK = rowsList[i + 14]
                                        });
                                    }                                   
                                }                                     
                            }
                        }
                        break;
                    case "EPLAN403": //finde abbruchstelle

                        if(rowsList[i+1] == " 62") //wenn echter block und nicht definition
                        {
                            int filecount = rowsList.FindIndex(i, FindAttribute); //finde Attribute

                            if(rowsList[filecount+2] == "70_20010") //Wenn Potentialname
                            {
                                if (Diagramm.pages_List[pageCounter].Potential_List == null) //Wenn liste noch nicht vorhanden
                                {
                                    Diagramm.pages_List[pageCounter].Potential_List = new List<Potential>(); //neue liste
                                }

                                if (!Diagramm.pages_List[pageCounter].Potential_List.Any(potential => potential.Name == rowsList[i + 14])) //Wenn Potentialname auf dieser Seite n.v.
                                {
                                    //MessageBox.Show("Seite " + pageCounter + " hat das Potential " + rowsList[i + 14]);

                                    Diagramm.pages_List[pageCounter].Potential_List.Add(new Potential //Neuer Potentialeintrag
                                    {
                                        D_id = Diagramm.ID,
                                        P_id = pageCounter.ToString(),
                                        Name = rowsList[i + 14]
                                    });
                                }                                
                            }
                        }
                        
                        break;
                    case "EPLAN452":
                        if (rowsList[i + 1] == " 62") //wenn echter block und nicht definition
                        {
                            int filecount = rowsList.FindIndex(i, FindAttribute); //finde Attribute

                            switch (rowsList[filecount + 2])
                            {
                                case "4_40101": //Kosntrukteur Seite
                                    Diagramm.pages_List[pageCounter].Author = rowsList[i + 14];
                                    break;

                                case "4_11011": //Seitentitel
                                    Diagramm.pages_List[pageCounter].Title = rowsList[i + 14];
                                    break;

                                case "4_1120": //Anlagenkennzeichen Seite
                                    Diagramm.pages_List[pageCounter].PrePreFix = rowsList[i + 14];
                                    break;

                                case "4_1220": //Ortskennzeichen Seite
                                    Diagramm.pages_List[pageCounter].PreFix = rowsList[i + 14];
                                    break;

                                case "4_11000": //Original Seitenzahl
                                    Diagramm.pages_List[pageCounter].OriginNumber = rowsList[i + 14];
                                    break;
                            }
                        }
                        break;
                }
            }
        }
    }
}
