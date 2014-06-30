using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace bgx_caw
{
    /// <summary>
    /// Eigene Klasse die Orginal und Custom BitmapImages verwalet
    /// sowie einige informationen zu den Bildern Speichert
    /// Wird für das Laden von Bildern aus der Datenbank verwendet
    /// </summary>
    public class CustomBitmapImage 
    {
        private BitmapImage _customImage;
        /// <summary>
        /// Bild mit Makierungen
        /// </summary>
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
        /// <summary>
        /// OrginalBild ohne Makierungen
        /// </summary>
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
        /// <summary>
        /// Ist ein custom Bild vorhanden
        /// </summary>
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
        /// <summary>
        /// Seite des Bildes im Projekt/Diagramm
        /// </summary>
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
        /// <summary>
        /// Seiten ID (Primary Key) der Seite aus der Datenbank
        /// </summary>
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
