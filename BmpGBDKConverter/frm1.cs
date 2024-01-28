namespace BmpGBDKConverter
{
    public partial class frm1 : Form
    {

        string loadFilePath = "";

        // here are some hardcoded values for testing:
        uint lightColor = 0xffd7f7d7;
        uint midLightColor = 0xff6ca66c;
        uint midHighColor = 0xff1e594a;
        uint darkColor = 0xff00131a;

        int tilePixelHeight = 8;
        int tilePixelWidth = 8;

        int tileMapRows = 2;
        int tileMapColumns = 8;
        int tilesInTileMap = 12;

        // byte array read from file
        byte[] bmpBytes;




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


        // general pseudo code
        // i need to load up the file into a byte array, i need some values from the header
        // i need to read in the width and height of the file in pixels
        // i need to let the user input a tile count and a row= and col= for the tile set 
        // basically what i need to write is a list of 16 bytes, 2 bytes per row encoding 0-3 value for pixel darkness
    }
}
