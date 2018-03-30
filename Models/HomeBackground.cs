using System;
namespace SBTCustomerManager.Models
{
    public class HomeBackground
    {
        #region Properties
        public string ImageURL { get; set; }
        #endregion
        #region Constructors
        public HomeBackground()
        {
            SetImageUrl();
        }
        #endregion
        #region Public Methods
        public void SetImageUrl ()
        {
            Random rnd = new Random();
            int imageID = rnd.Next(1, 13);

            switch (imageID)
            {
                case 1:
                    this.ImageURL = "https://farm5.staticflickr.com/4657/38819658160_727a1d992c_o.jpg";
                    break;
                case 2:
                    this.ImageURL = "https://farm5.staticflickr.com/4752/38821508940_1e8f651ab6_o.jpg";
                    break;
                case 3:
                    this.ImageURL = "https://farm5.staticflickr.com/4742/25760670907_1c4dd53171_o.jpg";
                    break;
                case 4:
                    this.ImageURL = "https://farm5.staticflickr.com/4777/38861546900_cfbf2ac3e3_o.jpg";
                    break;
                case 5:
                    this.ImageURL = "https://farm5.staticflickr.com/4752/40629922212_d865b92d68_o.jpg";
                    break;
                case 6:
                    this.ImageURL = "https://farm5.staticflickr.com/4742/38861695670_6cb3989049_o.jpg";
                    break;
                case 7:
                    this.ImageURL = "https://farm5.staticflickr.com/4618/38861706870_09eaa6fb28_o.jpg";
                    break;
                case 8:
                    this.ImageURL = "https://farm5.staticflickr.com/4611/39777148525_a831f5d73c_o.jpg";
                    break;
                case 9:
                    this.ImageURL = "https://farm5.staticflickr.com/4654/40630146792_d082fdd92a_o.jpg";
                    break;
                case 10:
                    this.ImageURL = "https://farm5.staticflickr.com/4801/38861704220_54134d5c8f_o.jpg";
                    break;
                case 11:
                    this.ImageURL = "https://farm5.staticflickr.com/4621/40630149512_5fff9d2c63_o.jpg";
                    break;
                case 12:
                    this.ImageURL = "https://farm5.staticflickr.com/4782/39909537065_f09a2f735c_o.jpg";
                    break;
                case 13:
                    this.ImageURL = "https://farm1.staticflickr.com/892/40392342684_4a1ed2ac93_o.jpg";
                    break;
            }
        }
        #endregion

    }
}
