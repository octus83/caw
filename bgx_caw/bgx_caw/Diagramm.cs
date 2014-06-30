using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw
{
    /// <summary>
    /// Representiert eine Datenbank Diagramm Tabelle als Objekt
    /// </summary>
    public class Diagramm
    {
        public List<Page> pages_List;
        private String _endCustomer = "";
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

        private String _fieldName;
        public String FieldName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_fieldName))
                {
                    return "Keine Angabe";
                }
                else
                {
                    return _fieldName;
                }
            }
            set
            {
                _fieldName = value;
            }
        }

        private String _projectNumber;
        public String ProjectNumber
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_projectNumber))
                {
                    return "Keine Angabe";
                }
                else
                {
                    return _projectNumber;
                }
            }
            set
            {
                _projectNumber = value;
            }
        }

        private String _projectName;
        public String ProjectName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_projectName))
                {
                    return "Keine Angabe";
                }
                else
                {
                    return _projectName;
                }
            }
            set
            {
                _projectName = value;
            }
        }

        private String _jobNumber;
        public String JobNumber
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_jobNumber))
                {
                    return "Keine Angabe";
                }
                else
                {
                    return _jobNumber;
                }
            }
            set
            {
                _jobNumber = value;
            }
        }

        private String _customer;
        public String Customer
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_customer))
                {
                    return "Keine Angabe";
                }
                else
                {
                    return _customer;
                }
            }
            set
            {
                _customer = value;
            }
        }

        public String EndCustomer
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_endCustomer))
                {
                    return "Keine Angabe";
                }
                else
                {
                    return _endCustomer;
                }
            }
            set
            {
                _endCustomer = value;
            }
        }

        private String _addressRow1;
        public String AddressRow1
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_addressRow1))
                {
                    return "Keine Angabe";
                }
                else
                {
                    return _addressRow1;
                }
            }
            set
            {
                _addressRow1 = value;
            }
        }

        private String _addressRow2;
        public String AddressRow2
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_addressRow2))
                {
                    return "Keine Angabe";
                }
                else
                {
                    return _addressRow2;
                }
            }
            set
            {
                _addressRow2 = value;
            }
        }

        private String _addressRow3;
        public String AddressRow3
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_addressRow3))
                {
                    return "Keine Angabe";
                }
                else
                {
                    return _addressRow3;
                }
            }
            set
            {
                _addressRow3 = value;
            }
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

        public int PageCount
        {
            get
            {
                return pages_List.Count;
            }
        }

        public Diagramm(DirectoryInfo directory)
        {
            ID = Guid.NewGuid().ToString();
            SourceFolder = "c:\\caw\\" + ID + "\\";
            pages_List = new List<Page>();
        }

        public Diagramm()
        {
            SourceFolder = "c:\\caw\\" + ID + "\\";
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
