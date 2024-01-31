using System.Diagnostics;
using System.Numerics;
using System.IO;
using BmpGBDKConverter.Models;

namespace BmpGBDKConverter
{
    public partial class ImportBMPForm : Form
    {

        // bitmap constants
        const int BMP_PIXEL_WIDTH_VALUE_OFFSET = 18;
        const int BMP_PIXEL_HEIGHT_VALUE_OFFSET = 22;
        const int BMP_IMAGE_DATA_OFFSET = 10;

        // multi array of tiles
        GBTile[,] tiles;

        // constants
        const int PIXELS_PER_TILE_ROW = 8;
        const int BYTES_PER_PIXEL = 4; // this theorectically is NOT constant depending on bmp depth, but its probably const for me, TODO fix tho
        const int TILE_ROWS = 8;

        string loadFilePath = "";

        // here are some hardcoded values for testing:
        const uint lightColor = 0xffd7f7d7;
        const uint midLightColor = 0xff6ca66c;
        const uint midHighColor = 0xff1e594a;
        const uint darkColor = 0xff00131a;

        // bmp header data
        int pixelDataOffset;
        int bmpPixelWidth;
        int bmpPixelHeight;


        // Tile relevant data
        int totalTiles;
        int numColumns;
        int numRows;

        // byte array read from file
        byte[] bmpBytes;

        public ImportBMPForm()
        {
            InitializeComponent();
        }



        private void btnConvert_Click(object sender, EventArgs e)
        {

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
                    bool isValidBMP = ValidateBMP(bmpBytes);
                    ProcessBMPHeader(bmpBytes);
                    uint[] pixels = TranslateBytesToPixels(bmpBytes);
                    SetUpTileArray();
                    ProcessPixels(pixels);
                    WriteMappedBytes();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading the file: {ex.Message}");
                }
            }
        }

        private bool ValidateBMP(byte[] bmpBytes)
        {
            // if the first two bytes are not char 'B' 'M' we aren't reading the correct format of file
            char b = (char)bmpBytes[0];
            char m = (char)bmpBytes[1];
            if (b != 'B' && b != 'M') return false;

            // if there is padding in this BMP none of the processing will work properly
            // TODO

            return true;
        }
        
        private void ProcessBMPHeader(byte[] readBytes)
        {
            pixelDataOffset = bmpBytes[BMP_IMAGE_DATA_OFFSET];
            bmpPixelWidth = bmpBytes[BMP_PIXEL_WIDTH_VALUE_OFFSET];
            bmpPixelHeight = bmpBytes[BMP_PIXEL_HEIGHT_VALUE_OFFSET];
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

        // method that starts at data offset and reads in full list of pixels
        private uint[] TranslateBytesToPixels(byte[] bmpBytes)
        {
            int pixelDataEndIndex = pixelDataOffset + (bmpPixelWidth * bmpPixelHeight * BYTES_PER_PIXEL);

            uint[] pixels = new uint[bmpPixelWidth * bmpPixelHeight];

            for (int i = pixelDataOffset, j = 0; i < pixelDataEndIndex; i += BYTES_PER_PIXEL, j++)
            {
                pixels[j] = DeterminePixelColor(bmpBytes[i..(i + BYTES_PER_PIXEL)]);
            }

            return pixels;
        }

        private void SetUpTileArray()
        {
            totalTiles = (bmpPixelHeight * bmpPixelWidth) / (GBTile.TILE_PIXEL_WIDTH * GBTile.TILE_PIXEL_HEIGHT);
            numColumns = bmpPixelWidth / GBTile.TILE_PIXEL_WIDTH;
            numRows = bmpPixelHeight / GBTile.TILE_PIXEL_HEIGHT;

            tiles = new GBTile[numRows, numColumns];
            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numColumns; c++)
                {
                    tiles[r, c] = new GBTile();
                }
            }
        }

        private void ProcessPixels(uint[] pixelData)
        {

            for (int bmpRowStart = 0; bmpRowStart < (bmpPixelWidth * bmpPixelHeight); bmpRowStart += bmpPixelWidth)
            {
                ProcessRow(pixelData[bmpRowStart..(bmpRowStart + bmpPixelWidth)], (bmpRowStart / (bmpPixelWidth * TILE_ROWS)) , ((bmpRowStart / bmpPixelWidth) % TILE_ROWS));
            }
        }

        private void ProcessRow(uint[] pixelRow, int tileRow, int tilePixelRow)
        {
            for (int pixel = 0; pixel < bmpPixelWidth; pixel += PIXELS_PER_TILE_ROW)
            {
                ProcessPixelRowChunk(pixelRow[pixel..(pixel + PIXELS_PER_TILE_ROW)], tiles[tileRow , (pixel / PIXELS_PER_TILE_ROW)].rows[tilePixelRow]);
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

        private void WriteMappedBytes()
        {
            string filePath = "testoutput.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for(int row = numRows - 1; row >= 0; row--)
                    {
                        for (int col = 0; col < (numColumns); col++)
                        {
                            string strToWrite = tiles[row, col].GetStringValues();
                            writer.WriteLine(strToWrite);
                        }
                    }
                }
                MessageBox.Show("Successfully converted BMP");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went VERY wrong: {ex.Message}");
            }
        }

    }
}
