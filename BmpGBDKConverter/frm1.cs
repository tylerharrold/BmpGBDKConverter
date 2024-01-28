using System.Diagnostics;
using System.Numerics;
using System.IO;
using BmpGBDKConverter.Models;

namespace BmpGBDKConverter
{
    public partial class frm1 : Form
    {

        // bitmap constants
        const int BMP_PIXEL_WIDTH_VALUE_OFFSET = 18;
        const int BMP_PIXEL_HEIGHT_VALUE_OFFSET = 22;
        const int BMP_IMAGE_DATA_OFFSET = 10;

        // multi array of tiles
        GBTile[,] tiles;

        // constants
        const int PIXELS_PER_TILE_ROW = 8;
        const int BYTES_PER_PIXEL = 4; // this theorectically is not constant depending on bmp depth, but its probably const for me
        const int LSB = 0;
        const int MSB = 1;
        const int TILE_ROWS = 8;

        string loadFilePath = "";

        // here are some hardcoded values for testing:
        const uint lightColor = 0xffd7f7d7;
        const uint midLightColor = 0xff6ca66c;
        const uint midHighColor = 0xff1e594a;
        const uint darkColor = 0xff00131a;

        int pixelWidth;
        int pixelHeight;


        // THESE HARDCODED VALUES SHOULD BE FIGURED BY DATA
        int tileMapRows = 2;
        int tileMapColumns = 8;

        // byte array read from file
        byte[] bmpBytes;
        ByteHolder[] convertedByteData;

        private class ByteHolder
        {
            public byte msbData;
            public byte lsbData;
            public ByteHolder()
            {
                msbData = lsbData = 0;
            }
        }


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
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                loadFilePath = openFileDialog.FileName;

                try
                {
                    bmpBytes = File.ReadAllBytes(loadFilePath);
                    // refactor here, this is testing
                    //ProcessImportedBytes();
                    //WriteMappedBytes();

                    uint[] pixels = TranslateBytesToPixels(bmpBytes);
                    SetUpTileArray();
                    ProcessPixels(pixels);
                    WriteMappedBytesAlternate();



                }
                catch (Exception ex)
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

            // setup the mapped byte array
            // we have two bytes per tile row, 8 rows per tile, tile_rows , tile_height
            convertedByteData = new ByteHolder[TILE_ROWS * tileMapColumns * tileMapRows];
            for (int i = 0; i < convertedByteData.Length; i++)
            {
                convertedByteData[i] = new ByteHolder();
            }

            int ROW_TILE_OFFSET = BYTES_PER_PIXEL * PIXELS_PER_TILE_ROW;

            int tilesConverted = 0;
            bool finished = false;
            for (int rows = pixelOffset; rows < bmpBytes.Length; rows += (pixelWidth * BYTES_PER_PIXEL * TILE_ROWS))
            {
                int colStartOffset = rows;
                for (int cols = colStartOffset; cols < (colStartOffset + (ROW_TILE_OFFSET * TILE_ROWS)); cols += ROW_TILE_OFFSET)
                {

                    MapTileByRow(bmpBytes, convertedByteData, cols, tilesConverted);
                    tilesConverted += TILE_ROWS;

                }

            }
        }

        // mappedValues NEEDS TO BE SLICED HERE
        private void MapTileByRow(byte[] pixelBytes, ByteHolder[] mappedValues, int tileStartIndex, int mapStartIndex)
        {
            for (int row = tileStartIndex, mapIndex = mapStartIndex; row < (tileStartIndex + (pixelWidth * BYTES_PER_PIXEL * TILE_ROWS)); row += (pixelWidth * BYTES_PER_PIXEL), mapIndex++)
            {
                MapPixelRow(pixelBytes[row..(row + PIXELS_PER_TILE_ROW * BYTES_PER_PIXEL)], mappedValues[mapIndex]);
            }
        }

        private void MapPixelRow(byte[] pixelBytes, ByteHolder rowBytes)
        {
            Debug.Assert(pixelBytes.Length == (PIXELS_PER_TILE_ROW * BYTES_PER_PIXEL));

            for (int i = 0; i < (PIXELS_PER_TILE_ROW * BYTES_PER_PIXEL); i = i + BYTES_PER_PIXEL)
            {
                uint colorValue = DeterminePixelColor(pixelBytes[i..(i + BYTES_PER_PIXEL)]);
                int pixelIndexForByte = i / 4;
                switch (colorValue)
                {
                    case (lightColor):
                        break;
                    case midLightColor:
                        rowBytes.lsbData = MarkPixelValueAtIndex(rowBytes.lsbData, pixelIndexForByte);
                        break;
                    case midHighColor:
                        rowBytes.msbData = MarkPixelValueAtIndex(rowBytes.msbData, pixelIndexForByte);
                        break;
                    case darkColor:
                        rowBytes.lsbData = MarkPixelValueAtIndex(rowBytes.lsbData, pixelIndexForByte);
                        rowBytes.msbData = MarkPixelValueAtIndex(rowBytes.msbData, pixelIndexForByte);
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

            uint colorValue = (uint)((alpha << 24) | (r << 16) | (g << 8) | (b));

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

        private byte MarkPixelValueAtIndex(byte rowByte, int index)
        {
            byte indexByte = 1;
            indexByte = (byte)(indexByte <<= index);

            rowByte = (byte)(rowByte | indexByte);

            return rowByte;

        }

        private void WriteMappedBytes()
        {
            string filePath = "testoutput.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    /*
                    int bytesWritten = 0;
                    foreach (ByteHolder b in convertedByteData)
                    {
                        writer.Write("0x");
                        writer.Write(b.lsbData.ToString("X2"));
                        writer.Write(",");
                        writer.Write("0x");
                        writer.Write(b.msbData.ToString("X2"));
                        writer.Write(",");
                        bytesWritten++;
                        if (bytesWritten > 7)
                        {
                            writer.Write("\n");
                            bytesWritten = 0;
                        }
                    }
                    */
                    for (int i = 0; i < convertedByteData.Length; i += TILE_ROWS)
                    {
                        for (int j = TILE_ROWS - 1; j >= 0; j--)
                        {
                            int index = i + j;
                            writer.Write("0x");
                            writer.Write(convertedByteData[index].lsbData.ToString("X2"));
                            writer.Write(",");
                            writer.Write("0x");
                            writer.Write(convertedByteData[index].msbData.ToString("X2"));
                            writer.Write(",");
                        }
                        writer.Write("\n");
                    }

                }



                MessageBox.Show("Maybe worked?");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went VERY wrong {ex.Message}");
            }
        }

        // we need a different way

        // method that starts at data offset and reads in full list of pixels
        private uint[] TranslateBytesToPixels(byte[] bmpBytes)
        {

            int pixelOffset = bmpBytes[BMP_IMAGE_DATA_OFFSET];
            pixelWidth = bmpBytes[BMP_PIXEL_WIDTH_VALUE_OFFSET];
            pixelHeight = bmpBytes[BMP_PIXEL_HEIGHT_VALUE_OFFSET];
            int pixelDataEndIndex = pixelOffset + (pixelWidth * pixelHeight * BYTES_PER_PIXEL);

            uint[] pixels = new uint[pixelWidth * pixelHeight];

            for (int i = pixelOffset, j = 0; i < pixelDataEndIndex; i += BYTES_PER_PIXEL, j++)
            {
                pixels[j] = DeterminePixelColor(bmpBytes[i..(i + BYTES_PER_PIXEL)]);
            }

            return pixels;
        }

        private void SetUpTileArray()
        {
            int totalTiles = (pixelHeight * pixelWidth) / (GBTile.TILE_PIXEL_WIDTH * GBTile.TILE_PIXEL_HEIGHT);
            int tilesInRow = pixelWidth / GBTile.TILE_PIXEL_WIDTH;
            int tilesInCol = pixelHeight / GBTile.TILE_PIXEL_HEIGHT;

            tiles = new GBTile[tilesInRow, tilesInCol];
            for (int i = 0; i < tilesInRow; i++)
            {
                for (int j = 0; j < tilesInCol; j++)
                {
                    tiles[i, j] = new GBTile();
                }
            }
        }

        private void ProcessPixels(uint[] pixelData)
        {

            for (int rowStart = 0; rowStart < (pixelWidth * pixelHeight); rowStart += pixelWidth)
            {
                ProcessRow(pixelData[rowStart..(rowStart + pixelWidth)], (rowStart / (pixelWidth * TILE_ROWS)) , ((rowStart / pixelWidth) % TILE_ROWS));
            }
        }

        private void ProcessRow(uint[] pixelRow, int tileRow, int tilePixelRow)
        {
            for (int pixel = 0; pixel < pixelWidth; pixel += PIXELS_PER_TILE_ROW)
            {
                ProcessPixelRowChunk(pixelRow[pixel..(pixel + PIXELS_PER_TILE_ROW)], tiles[(pixel / PIXELS_PER_TILE_ROW) , tileRow].rows[tilePixelRow]);
            }
        }

        private void ProcessPixelRowChunk(uint[] pixels, TileRow row)
        {
            Debug.Assert(pixels.Length == 8);

            for (int i = 0; i < pixels.Length; i++)
            {
                row.MarkPixel(GetColorValue(pixels[i]), i);
            }
        }

        private ColorValue GetColorValue(uint pixelValue)
        {
            switch (pixelValue)
            {
                case (lightColor):
                    return ColorValue.LOW;
                case midLightColor:
                    return ColorValue.MID_LOW;
                case midHighColor:
                    return ColorValue.MID_HIGH;
                case darkColor:
                    return ColorValue.HIGH;
                default:
                    return ColorValue.LOW;
            }
        }

        private void WriteMappedBytesAlternate()
        {
            string filePath = "testoutput.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    /*
                    int bytesWritten = 0;
                    foreach (ByteHolder b in convertedByteData)
                    {
                        writer.Write("0x");
                        writer.Write(b.lsbData.ToString("X2"));
                        writer.Write(",");
                        writer.Write("0x");
                        writer.Write(b.msbData.ToString("X2"));
                        writer.Write(",");
                        bytesWritten++;
                        if (bytesWritten > 7)
                        {
                            writer.Write("\n");
                            bytesWritten = 0;
                        }
                    }
                    */
                    for(int row = 1; row >= 0; row--)
                    {
                        for (int col = 0; col < (pixelWidth / GBTile.TILE_PIXEL_WIDTH); col++)
                        {
                            string strToWrite = tiles[col, row].GetStringValues();
                            writer.WriteLine(strToWrite);
                        }
                    }

                }



                MessageBox.Show("Maybe worked?");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went VERY wrong {ex.Message}");
            }
        }

    }
}
