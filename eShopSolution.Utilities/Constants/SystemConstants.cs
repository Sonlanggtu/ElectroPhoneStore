using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Utilities.Constants
{
    public class SystemConstants
    {
        public const string MainConnectionString = "eShopSolutionDb";
        public const string CartSession = "CartSession";

        // Để những cái key chuẩn cần thiết để set string key-value trong session
        public class AppSettings
        {
            public const string DefaultLanguageId = "DefaultLanguageId";
            public const string Token = "Token";
            public const string BaseAddressBackend = "BaseAddressBackend";
            public const string BaseAddress = "BaseAddress";
        }

        public class ProductSettings
        {
            public const int NumberOfFeturedProducts = 20;
            public const int NumberOfLatestProducts = 6;
        }

        public class ProductConstants
        {
            public const string NA = "N/A";

            public const int PRIORITY_ONE = 1;
            public const int PRIORITY_TWO = 2;
            public const int PRIORITY_THREE = 3;


            // 1. Laptop Gaming, Đồ Họa - Kỹ Thuật
            // 2.  Đồ Họa - Kỹ Thuật
            // 3. Học Tập - Văn Phòng
            // 4. Cao Cấp - Sang Trọng
            // 5. Không thuộc loại nào

            public const int LAPTOP_GAMING = 1;
            public const int LAPTOP_GRAPHICS = 2;
            public const int LAPTOP_OFFFICE = 3;
            public const int LAPTOP_LUXURY = 4;
            public const int LAPTOP_NO_TYPE = 5;

        }

		public class UrlConstants
		{

			public const string HOME_PAGE = "";
			public const string PRODUCT_DETAIL = "san-pham";
			public const string PRODUCT_CATEGORY = "danh-muc-san-";

		}
	}
}