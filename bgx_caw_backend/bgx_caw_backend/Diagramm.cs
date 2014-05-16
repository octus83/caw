using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw_backend
{
    /// <summary>
    /// struct that holds all the 
    /// </summary>
    class Diagramm
    {
        public List<Page> pages_List;       
        private String _endCustomer = "";

        public String ID
        {
            get;
            private set;
        }

        public String Serialnumber
        {
            get;
            set;
        }

        public String Fieldname
        {
            get;
            set;
        }

        public String Projectnumber
        {
            get;
            set;
        }

        public String Projectname
        {
            get;
            set;
        }

        public String Jobumber
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

        public DateTime Date_init
        {
            get;
            set;
        }

        public DateTime Date_lastchange
        {
            get;
            set;
        }

        public String Productionplace
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
            get
            {
                return "c:\\caw\\" + ID + "\\";
            }
        }

        public Diagramm(DirectoryInfo directory)
        {
            ID = Guid.NewGuid().ToString();
            pages_List = new List<Page>();
        }

        public Diagramm()
        {
            ID = Guid.NewGuid().ToString();
            pages_List = new List<Page>();
        }
    }
}
