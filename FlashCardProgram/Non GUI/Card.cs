using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardProgram
{
    public class Card
    {
        public string FrontText;
        public string BackText;

        public string FrontImage;
        public string BackImage;

        public Card()
        {
            FrontText = "";
            BackText = "";
            FrontImage = "";
            BackImage = "";
        }

        public Card(string pFrontText, string pBackText)
        {
            FrontText = pFrontText;
            BackText = pBackText;
            FrontImage = "";
            BackImage = "";
        }
        
        public Card(string pFrontText, string pBackText, string pFrontImage, string pBackImage)
        {
            FrontText = pFrontText;
            BackText = pBackText;
            FrontImage = pFrontImage;
            BackImage = pBackImage;
        }
    }
}
