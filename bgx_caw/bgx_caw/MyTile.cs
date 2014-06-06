using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;

namespace bgx_caw
{
    class MyTile :Tile
    {
        private Object _data;

        public Object Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            
            }
        }

        public MyTile()
        {
            
        }
    }
}
