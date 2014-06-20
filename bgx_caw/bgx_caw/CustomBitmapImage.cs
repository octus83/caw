using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace bgx_caw
{
    public class CustomBitmapImage 
    {
        private BitmapImage _customImage;
        public BitmapImage CustomImage
        {
            set{
                if (value != null)
                {
                    this._customImage = value;
                    this._isCustomImage = true;
                }
            }
            get{
                return _customImage;
            }
        }

        private BitmapImage _originalImage;
        public BitmapImage OrginalImage
        {
            set
            {
                this._originalImage = value;
            }
            get
            {
                return this._originalImage;
            }
        }

        private bool _isCustomImage = false;
        public bool IsCustomImage
        {
            set
            {
                this._isCustomImage = value;
            }
            get
            {
                return this._isCustomImage;
            }
        }
        private int _pageInDiagramm;
        public int PageInDiagramm
        {
            set
            {
                this._pageInDiagramm = value;
            }
            get
            {
                return this._pageInDiagramm;
            }
        }
        private String _p_Id;
        public String P_id
        {
            set
            {
                this._p_Id = value;
            }
            get
            {
                return this._p_Id;
            }
        }

        public CustomBitmapImage()
        {

        }
    }
}
