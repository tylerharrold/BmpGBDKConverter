using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpGBDKConverter.Models
{
    public class GBTile
    {
        public const int TILE_PIXEL_HEIGHT = 8;
        public const int TILE_PIXEL_WIDTH = 8;
        public const int ROWS_PER_TILE = 8;
        public TileRow[] rows;

        public GBTile()
        {
            rows = new TileRow[ROWS_PER_TILE];
            for(int i = 0; i < ROWS_PER_TILE; i++)
            {
                rows[i] = new TileRow();
            }
        }

        public string GetStringValues()
        {
            string rowVals = string.Empty;

            for(int i = rows.Length - 1; i >= 0; i--)
            {
                rowVals += rows[i].GetRowHexValuesAsString();
            }

            return rowVals;
        }
    }
}
