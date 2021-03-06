﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw_backend
{
    public class Page
    {
        public String D_id;
        public String Title;
        public String OriginNumber;
        public List<Part> Parts_List;
        public List<Potential> Potential_List;
        public byte[] Image;

        public int PageInDiagramm
        {
            get;
            set;
        }
       
        public String P_id
        {
            get;
            set;
        }
        
        private String _prePreFix;
        public String PrePreFix
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_prePreFix))
                {
                    return "Keine Angabe";
                }
                else
                {
                    return _prePreFix;
                }
            }
            set
            {
                _prePreFix = value;
            }
        }

        private String _preFix;
        public String PreFix
        {
            get
            {
                if(String.IsNullOrWhiteSpace(_preFix))
                {
                    return "Keine Angabe";
                }
                else
                {
                    return _preFix;
                }
            }
            set
            {
                _preFix = value;
            }
        }
        
        private String _author;
        public String Author
        {
            get
            {
                if (_author == null)
                {
                    _author = "";
                }

                return _author;
            }
            set
            {
                _author = value;
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

        public Page()
        {
            P_id = Guid.NewGuid().ToString();

            Parts_List = new List<Part>();
            Potential_List = new List<Potential>();
        }

        public Page(String diagramm_id)
        {
            D_id = diagramm_id;
            P_id = Guid.NewGuid().ToString();

            Parts_List = new List<Part>();
            Potential_List = new List<Potential>();
        }
    }
}
