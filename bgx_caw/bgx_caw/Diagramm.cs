﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw
{
    /// <summary>
    /// struct that holds all the 
    /// </summary>
    class Diagramm
    {
        public List<Page> pages_List;       
        private String _endCustomer = "";
        private String _sourceFolder;
        private String _serialNumber;

        public String ID
        {
            get;
            set;
        }

        public String SerialNumber
        {
            get
            {
                if (_serialNumber == null)
                {
                    return "";
                }

                return _serialNumber;
            }
            set
            {
                _serialNumber = value;
            }
        }

        public String FieldName
        {
            get;
            set;
        }

        public String ProjectNumber
        {
            get;
            set;
        }

        public String ProjectName
        {
            get;
            set;
        }

        public String JobNumber
        {
            get;
            set;
        }

        public String Customer
        {
            get;
            set;
        }

        public String Endcustomer
        {
            get
            {
                return _endCustomer;
            }
            set
            {
                _endCustomer = value;
            }
        }

        public String SiteRow1
        {
            get;
            set;
        }

        public String SiteRow2
        {
            get;
            set;
        }

        public String SiteRow3
        {
            get;
            set;
        }

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

        public String ProductionPlace
        {
            get;
            set;
        }

        public Boolean IsActive
        {
            get;
            set;
        }

        public String SourceFolder
        {
            get;
            set;
        }

        public Diagramm(DirectoryInfo directory)
        {
            ID = Guid.NewGuid().ToString();
            pages_List = new List<Page>();
        }

        public Diagramm()
        {
            //ID = Guid.NewGuid().ToString();
            pages_List = new List<Page>();
        }

        public Diagramm(Guid id)
        {
            ID = id.ToString();
            SourceFolder = "c:\\caw\\" + ID + "\\";
            pages_List = new List<Page>();
        }
    }
}