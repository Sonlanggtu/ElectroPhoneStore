using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Sales
{
    public class CartItemViewModel
    {
        public int id { get; set; }
        public int quantity { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string price { get; set; }
		public string code { get; set; }
		public string href { get; set; }
	}
}

//"id": 27,
//"name": "Lenovo LOQ 15IRH8",
//"price": "18000000,00",
//"image": "https://localhost:5001/user-content/757ffb43-0162-4bf6-b0b6-00355327810b.webp",
//"href": "https://localhost:5008/san-pham/lenovo-loq-15irh8/27",
//"code": "<figure class=\"table\"><table><tbody><tr><td>CPU: Intel® Core™ i5-13420H, 8 Cores (4P + 4E) / 12 Threads, P-core up to 4.6GHz, E-core up to 3.4GHz, 12MB</td></tr><tr><td>RAM: 1 x 8GB DDR5 5200MHz (2x SO-DIMM socket, up to 16GB SDRAM)</td></tr><tr><td>Ổ cứng: 1TB SSD M.2 2242 PCIe 4.0x4 NVMe (2 Slots: M2 2242 PCIe 4.0 x4 Slot, M.2 2280 PCIe 4.0 x4 Slot)</td></tr><tr><td>Card đồ họa:NVIDIA® GeForce RTX™ 3050 6GB GDDR6, Boost Clock 2370MHz, TGP 95W</td></tr><tr><td>Màn hình:15.6 inch FHD (1920x1080) IPS 350nits Anti-glare, 45% NTSC, 144Hz, G-SYNC</td></tr><tr><td>Cổng kết nối:1x USB 3.2 Gen 1, 2x USB 3.2 Gen 2, 1x USB-C® 3.2 Gen 2 (support data transfer, Power Delivery 140W and DisplayPort™ 1.4), 1x HDMI®, up to 8K/60Hz, 1x Ethernet (RJ-45), 1x Headphone / microphone combo jack (3.5mm)</td></tr><tr><td>Kích thước:359.6 x 264.8 x 22.1-25.2 mm</td></tr><tr><td>Trọng lượng:2.4 kg</td></tr><tr><td>Pin:Integrated 60Wh</td></tr></tbody></table></figure>",
//"quantity": 1,