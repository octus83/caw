using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw_backend
{
    /// <summary>
    /// struct that holds all the 
    /// </summary>
    class Diagramm : DB_CAW
    {
        public int ID
        {
            get;
            set;
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

        public String Author
        {
            get;
            set;
        }

        public String DeviceID
        {
            get;
            set;
        }

        public String Productionplace
        {
            get;
            set;
        }

        public String Worker
        {
            get;
            set;
        }

        public Boolean IsActive
        {
            get;
            set;
        }
    }
}
