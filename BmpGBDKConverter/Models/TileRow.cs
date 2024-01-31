using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpGBDKConverter.Models
{
    public enum ColorValue
    {
        LOW,
        MID_LOW,
        MID_HIGH,
        HIGH
    }

    public class TileRow
    {
        public byte LsByte { get; set; }
        public byte MsByte { get; set; }
        public TileRow() 
        {
            LsByte = MsByte = 0;
        }

        public string GetRowHexValuesAsString()
        {

            return "0x" + LsByte.ToString("X2") + "," + "0x" + MsByte.ToString("X2") + ",";
        }

        public void MarkPixel(ColorValue cv , int indexInRow)
        {
            switch (cv)
            {
                case ColorValue.LOW:
                    break;
                case ColorValue.MID_LOW:
                    LsByte = MarkPixelValueAtIndex(LsByte, indexInRow);
                    break;
                case ColorValue.MID_HIGH:
                    MsByte = MarkPixelValueAtIndex(MsByte, indexInRow);
                    break;
                case ColorValue.HIGH:
                    LsByte = MarkPixelValueAtIndex(LsByte, indexInRow);
                    MsByte = MarkPixelValueAtIndex(MsByte, indexInRow);
                    break;
                default:
                    break;
            }
        }

        private byte MarkPixelValueAtIndex(byte rowByte, int index)
        {
            index = 7 - index;
            byte indexByte = 1;
            indexByte = (byte)(indexByte <<= index);

            rowByte = (byte)(rowByte | indexByte);

            return rowByte;

        }
    }
}
