using System.Diagnostics;
using System.Numerics;

namespace BmpGBDKConverter
{
    public partial class frm1 : Form
    {
        // constants
        const int PIXELS_PER_TILE_ROW = 8;
        const int BYTES_PER_PIXEL = 4; // this theorectically is not constant depending on bmp depth, but its probably const for me
        const int LSB = 0;
        const int MSB = 1;
        const int TILE_ROWS = 8; 
        const int BYTES_PER_ROW_MAP = 2;

        string loadFilePath = "";

        // here are some hardcoded values for testing:
        const uint lightColor = 0xffd7f7d7;
        const uint midLightColor = 0xff6ca66c;
        const uint midHighColor = 0xff1e594a;
        const uint darkColor = 0xff00131a;

        int tilePixelHeight = 8;
        int tilePixelWidth = 8;
        int pixelWidth;
        int pixelHeight;

        int tileMapRows = 2;
        int tileMapColumns = 8;
        int tilesInTileMap = 12;

        // byte array read from file
        byte[] bmpBytes;
        byte[] convertedByteData;




        public frm1()
        {
            InitializeComponent();
        }



        private void btnConvert_Click(object sender, EventArgs e)
        {
            // validate inputs

            // convert specified color values to numeric values?

            // get the file to convert into a byte array

            // read the byte array using the values and conver it to a .cpp and .h file 
            // of the type that can be easily used in G


            throw new NotImplementedException();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                loadFilePath = openFileDialog.FileName;

                try
                {
                    bmpBytes = File.ReadAllBytes(loadFilePath);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Error reading the file: {ex.Message}");
                }
            }
        }

        private void ProcessImportedBytes()
        {
            // read in pixel data offset
            int pixelOffset = bmpBytes[10];
            // read in bitmap width/height
            pixelWidth = bmpBytes[18];
            pixelHeight = bmpBytes[22];

            // setup the mapped byte array
            // we have two bytes per tile row, 8 rows per tile, tile_rows , tile_height
            convertedByteData = new byte[2 * TILE_ROWS * tileMapColumns * tileMapRows];

            int ROW_TILE_OFFSET = BYTES_PER_PIXEL * PIXELS_PER_TILE_ROW; 
            
            int tilesConverted = 0;
            bool finished = false;
            for(int rows = pixelOffset; rows < bmpBytes.Length; rows+= (pixelWidth * BYTES_PER_PIXEL * TILE_ROWS))
            {
                int colStartOffset = rows;
                for(int cols = colStartOffset; cols < (colStartOffset + (ROW_TILE_OFFSET * TILE_ROWS)); cols += ROW_TILE_OFFSET)
                {

                    MapTileByRow(bmpBytes, convertedByteData, cols, tilesConverted);
                    tilesConverted+= BYTES_PER_ROW_MAP * TILE_ROWS;
                    
                    // NEED TO CHANGE THIS THIS DOESN'T WORK ANYMORE
                    if(tilesConverted > convertedByteData.Length)
                    {
                        finished = true;
                        if (finished) break;
                    }
                }

                if (finished) break;
            }
        }
        
        // mappedValues NEEDS TO BE SLICED HERE
        private void MapTileByRow(byte[] pixelBytes , byte[] mappedValues , int tileStartIndex , int mapStartIndex)
        {
            for(int row = tileStartIndex , mapIndex = mapStartIndex; row < (tileStartIndex + (pixelWidth * BYTES_PER_PIXEL * TILE_ROWS)); row += (pixelWidth * BYTES_PER_PIXEL) , mapIndex+= BYTES_PER_ROW_MAP)
            {
                MapPixelRow(pixelBytes[row..(row + PIXELS_PER_TILE_ROW * BYTES_PER_PIXEL)] , mappedValues[mapIndex..(mapIndex + BYTES_PER_ROW_MAP)]);
            }
        }

        private void MapPixelRow(byte[] pixelBytes , byte[] mappedValues)
        {
            Debug.Assert(pixelBytes.Length == (PIXELS_PER_TILE_ROW * BYTES_PER_PIXEL));
            Debug.Assert(mappedValues.Length == 2);

            for (int i = 0; i < (PIXELS_PER_TILE_ROW * BYTES_PER_PIXEL); i = i + BYTES_PER_PIXEL)
            {
                uint colorValue = DeterminePixelColor(pixelBytes[i..(i+BYTES_PER_PIXEL)]);
                int pixelIndexForByte = i / 4;
                switch (colorValue)
                {
                    case (lightColor):
                        break;
                    case midLightColor:
                        mappedValues[LSB] = MarkPixelValueAtIndex(mappedValues[LSB], pixelIndexForByte);
                        break;
                    case midHighColor:
                        mappedValues[MSB] = MarkPixelValueAtIndex(mappedValues[MSB], pixelIndexForByte);
                        break;
                    case darkColor:
                        mappedValues[LSB] = MarkPixelValueAtIndex(mappedValues[LSB], pixelIndexForByte);
                        mappedValues[MSB] = MarkPixelValueAtIndex(mappedValues[MSB], pixelIndexForByte);
                        break;
                    default:
                        break;
                }
            }
        }

        private uint DeterminePixelColor(byte[] pixelBytes)
        {
            Debug.Assert(pixelBytes.Length == 4);

            byte r, g, b, alpha;
            b = pixelBytes[0];
            g = pixelBytes[1];
            r = pixelBytes[2];
            alpha = pixelBytes[3];

            uint colorValue = (uint)((alpha << 24 ) | (r << 16) | (g << 8) | (b));

            switch (colorValue)
            {
                case (lightColor):
                    return lightColor;
                case midLightColor:
                    return midLightColor;
                case midHighColor:
                    return midHighColor;
                case darkColor:
                    return darkColor;
                default:
                    return lightColor;
            }
        }

        private byte MarkPixelValueAtIndex(byte rowByte , int index)
        {
            byte indexByte = 1;
            indexByte = (byte)(indexByte <<= index);

            rowByte = (byte)(rowByte | indexByte);

            return rowByte;
            
        }




        // general pseudo code
        // i need to load up the file into a byte array, i need some values from the header
        // i need to read in the width and height of the file in pixels
        // i need to let the user input a tile count and a row= and col= for the tile set 
        // basically what i need to write is a list of 16 bytes, 2 bytes per row encoding 0-3 value for pixel darkness
    }
}
