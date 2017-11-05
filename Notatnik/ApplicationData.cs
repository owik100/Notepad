using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Notatnik
{
    [Serializable]
    class ApplicationData
    {
        private string myDocumentspatch;
        private string finalPatch;

        private bool textWrapping;
        private bool checkMiss;
        private string backColor;
        private string fontColor;
        private string fontFamily;
        private double fontSize = 0;
        private string fontWeight;
        private string fontStyle;
        private double windowHeight;
        private double windowWidth;

        public ApplicationData()
        {
            myDocumentspatch = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            finalPatch = myDocumentspatch + @"\Notepad data";
        }

        public void Serialize()
        {
            if (!Directory.Exists(finalPatch))
            {
                Directory.CreateDirectory(finalPatch);
            }

                using (Stream output = File.Create(finalPatch + @"\data.dat"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(output, this);
            }
        }

        public void Serialize(string patch)
        {
            using (Stream output = File.Create(patch))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(output, this);
            }
        }

        public void Deserialize(ApplicationData appData)
        {
            try
            {
                using (Stream input = File.OpenRead(finalPatch + @"\data.dat"))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    appData = formatter.Deserialize(input) as ApplicationData;
                    textWrapping = appData.textWrapping;
                    checkMiss = appData.checkMiss;
                    backColor = appData.backColor;
                    fontColor = appData.fontColor;
                    fontFamily = appData.fontFamily;
                    fontSize = appData.fontSize;
                    fontWeight = appData.fontWeight;
                    fontStyle = appData.fontStyle;
                    windowHeight = appData.windowHeight;
                    windowWidth = appData.windowWidth;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR " + ex);
            }
        }

              public void Deserialize(ApplicationData appData,string patch)
        {
            try
            {
                using (Stream input = File.OpenRead(patch))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    appData = formatter.Deserialize(input) as ApplicationData;
                    textWrapping = appData.textWrapping;
                    checkMiss = appData.checkMiss;
                    backColor = appData.backColor;
                    fontColor = appData.fontColor;
                    fontFamily = appData.fontFamily;
                    fontSize = appData.fontSize;
                    fontWeight = appData.fontWeight;
                    fontStyle = appData.fontStyle;
                    windowHeight = appData.windowHeight;
                    windowWidth = appData.windowWidth;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR " + ex);
            }

        }

        public void GetTextWrappingInfo(bool textWrapping)
        {
            this.textWrapping = textWrapping;
        }

        public void GetCheckMissPellingsInfo(bool checkMiss)
        {
            this.checkMiss = checkMiss;
        }

        public void GetBackgroundColor(string backColor)
        {
            this.backColor = backColor;
        }

        public void GetFontColor(string fontColor)
        {
            this.fontColor = fontColor;
        }

        public void GetFontFamily(string fontFamily)
        {
            this.fontFamily = fontFamily;
        }

        public void GetFontSize(double fontSize)
        {
            this.fontSize = fontSize;
        }

        public void GetFontWeight(string fontWeight)
        {
            this.fontWeight = fontWeight;
        }

        public void GetFontStyle(string fontStyle)
        {
            this.fontStyle = fontStyle;
        }

        public void GetWindowHeight(double windowHeight)
        {
            this.windowHeight = windowHeight;
        }

        public void GetWindowWidth(double windowWidth)
        {
            this.windowWidth = windowWidth;
        }

        public bool SetTextWrappingInfo()
        {
            return textWrapping;
        }

        public bool SetCheckMissPellingsInfo()
        {
            return checkMiss;
        }

        public string SetBackgroundColor()
        {
            if(backColor==null)
                backColor ="#FFFFFFFF";
            return backColor;
        }

        public string SetFontColor()
        {
            if (fontColor == null)
                fontColor = "#000000";
            return fontColor;
        }

        public string SetFontFamily()
        {
            if (fontFamily == null)
                fontFamily = "Consolas";
            return fontFamily;
        }

        public double SetFontSize()
        {
            if(fontSize==0)
                fontSize = 15;
            return fontSize;
        }

        public string SetFontWeight()
        {
            if (fontWeight == null)
                fontWeight = "Regular";
            return fontWeight;            
        }

        public string SetFontStyle()
        {
            if (fontStyle == null)
                fontStyle = "Normal";
            return fontStyle;
        }

        public double SetWindowHeight()
        {
            if (windowHeight <= 0)
                windowHeight = 350;
            return windowHeight;
        }
        
        public double SetWindowsWidth()
        {
            if (windowWidth <= 0)
                windowWidth = 525;
            return windowWidth;
        } 



    }
}
