using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IpharmWebAppProject.Models;

namespace IpharmWebAppProject.Data
{
    public class DbInitializer
    {
        public static void Initialize(IpharmContext context)
        {
            context.Database.EnsureCreated();

            //user create
            if (context.Users.Any())
            {
                return; //DB exists
            }
            var users = new User[]
            {   new User{Email="shnikimaor@gmail.com",
                    FirstName="Shani", LastName="Maor",
                    Birthday=DateTime.Parse("2003-08-24"),Mobile="054-2081428",Password="12341234",
                    PostalCode="7544760",Country="Israel",City="Rishon Letzion",Adress="Inbar 7",
                    Type=Models.Type.Customer, Orders=new List<Order>(), Reviews=new List<Review>()},
                new User{Email="noyhadad@gmail.com",
                    FirstName="Noy", LastName="Hadad",
                    Birthday=DateTime.Parse("2003-09-15"),Mobile="058-4232011",Password="43214321",
                    PostalCode="7591819",Country="Israel",City="Rishon Letzion",Adress="Noam Launer 47",
                    Type=Models.Type.Customer, Orders=new List<Order>(), Reviews=new List<Review>()},
                new User{Email="yaari@gmail.com",
                    FirstName="Yaari", LastName="Sternberg",
                    Birthday=DateTime.Parse("1996-03-27"),Mobile="053-6215183",Password="12345678",
                    PostalCode="4634805",Country="Israel",City="Herzliya",Adress="Malkei Yehuda 5",
                    Type=Models.Type.Manager, Orders=null, Reviews=null}
            };
            //user add
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
            
            //wishlist create
            var wishlists = new WishList[]
            {   new WishList{Email="shnikimaor@gmail.com",
                    Counter=0, Products=new List<ProductInWishList>() },
                new WishList{Email="noyhadad@gmail.com",
                    Counter=0, Products=new List<ProductInWishList>() },
            };
            //wishlist add
            foreach (WishList u in wishlists)
            {
                context.WishLists.Add(u);
            }
            context.SaveChanges();

            //order create
            var orders = new Order[]
            {   new Order{Email="shnikimaor@gmail.com",
                    OrderDate=DateTime.Parse("2019-02-01"), Products=new List<ProductInOrder>(), Price=0, Status=Status.Cart },
                new Order{Email="noyhadad@gmail.com",
                    OrderDate=DateTime.Parse("2021-03-21"), Products=new List<ProductInOrder>(), Price=0, Status=Status.Cart },
            };
            //order add
            foreach (Order o in orders)
            {
                context.Orders.Add(o);
            }
            context.SaveChanges();

            //product create

            var products = new Product[]
            {
                new Product{Name="Niacinamide 10% + Zinc 1%",
                    Price=(float)(5.75),Amount=30,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Serum",Brand="The Ordinary",
                    Description="A serum to combat blemishes, congestions and sebum over-production.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/THEO0003F_1.jpg_s3.lmb_mdtdso/12b28eb479db78812f941e55870ecb1d/THEO0003F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/THEO0003F_2.jpg_s3.lmb_s2nkjc/08ad35262d7c7e61f86550e0a157d0c1/THEO0003F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/THEO0003F_3.jpg_s3.lmb_3qhc23/bc9c5579475a03ea3a6c1dcfba72353d/THEO0003F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=10, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Lactic Asid 10%",
                    Price=(float)(6.75),Amount=30,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Serum",Brand="The Ordinary",
                    Description="Visibly targets uneven tone, textural irregularities and fine lines. Offers a very mild exfoliation at a 10% concentration.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/THEO0009F_1.jpg_s3.lmb_npdt62/f5784ab4e8b9fbb1bb18a09307d3556e/THEO0009F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/THEO0009F_2.jpg_s3.lmb_szuvul/898abcc467c42a0432d864630bf4bbc0/THEO0009F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/THEO0009F_3.jpg_s3.lmb_rnkd5/bd73fddb65e9b6b7d5e9899cfa803a7b/THEO0009F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=15, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Salicylic Acid 2% Masque",
                    Price=(float)(11.75),Amount=50,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Face Mask",Brand="The Ordinary",
                    Description="A charcoal infused face mask.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/THEO0138F_1.jpg_s3.lmb_3em0e/cb46d48ecfbb82837373651d941281cc/THEO0138F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/THEO0138F_3.jpg_s3.lmb_39459b/95bdf95d7d48caebc676f1ae613c36d0/THEO0138F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3=null,
                    Stock=30, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Brilliant Skin Purifying Pink Clay Mask",
                    Price=(float)(43),Amount=60,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Face Mask",Brand="Sand&Sky",
                    Description="A 4-in-1 face mask.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/SASK0001F_1.jpg_s3.lmb_02bvi/5fe93fc09a460f83cfd2d07894a7bf1d/SASK0001F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/SASK0001F_2a.jpg_s3.lmb_xcy9vx/a991dbc8a714dcec751552f94b37adc0/SASK0001F_2a.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/SASK0001F_2b.jpg_s3.lmb_17fzj/4bec629250dd97cb431d42b78dc88239/SASK0001F_2b.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=30, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Super Bounce Mask",
                    Price=(float)(66),Amount=100,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Face Mask",Brand="Sand&Sky",
                    Description="A brightening face mask.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/SASK0007F_1.jpg_s3.lmb_mypzjf/81c7edfd252d5b6951f97e4eb92720ce/SASK0007F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/SASK0007F_2.jpg_s3.lmb_txm7c/db48c6cc80db648e40a06d7d27373d57/SASK0007F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/SASK0007F_4.jpg_s3.lmb_pgktn9/f6748d9da2cd03f08ad81989f1889c09/SASK0007F_4.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=12, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Australian Emu Apple Dreamy Glow Drops",
                    Price=(float)(37),Amount=17,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Serum",Brand="Sand&Sky",
                    Description="A brightening serum.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/SASK0022F_1.jpg_s3.lmb_f1zixd/558fd8a35ba1d8e3dfe930eb09c32436/SASK0022F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/SASK0022F_2.jpg_s3.lmb_hhok8j/e73a7749cd1e227c6cca8ba5ca2d9d9c/SASK0022F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/SASK0022F_3.jpg_s3.lmb_76hr3n/9fd211d1d73770683efbab972e777d49/SASK0022F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=7, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Tasmanian Spring Water Splash Serum",
                    Price=(float)(32.25),Amount=17,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Serum",Brand="Sand&Sky",
                    Description="An ultra-hydrating serum.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/SASK0020F_1.jpg_s3.lmb_mbx628e/ff21414ad0cd454a527d4225ffaf2f67/SASK0020F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/SASK0020F_2.jpg_s3.lmb_8h840c/fc8f5c2e899f0c8b5ad2dffdf184948e/SASK0020F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/SASK0020F_3.jpg_s3.lmb_9eip9b/8eb909685e82388a63b45b705208b3c5/SASK0020F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=9, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Revolution X Friends Monica Niacinamide Sheet Mask",
                    Price=(float)(5.25),Amount=20,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Sheet Mask",Brand="Makeup Revolution",
                    Description="A niacinamide infused sheet mask.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/REBE1753F_1.jpg_s3.lmb_4te5k5/d33cdba5440f31eb12a4b8d64407a667/REBE1753F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2=null,
                    PicUrl3=null,
                    Stock=15, Active=true
                },
                new Product{Name="Revolution X Friends Chandler Pink Clay Sheet Mask",
                    Price=(float)(5.25),Amount=20,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Sheet Mask",Brand="Makeup Revolution",
                    Description="A pink clay infused sheet mask.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/REBE1755F_1.jpg_s3.lmb_x7aaif/71fe6e0dc942a1f6a400354f1f82f9d1/REBE1755F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2=null,
                    PicUrl3=null,
                    Stock=15, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Revolution X Friends Phoebe Pineapple Sheet Mask",
                    Price=(float) (5.25),Amount=20,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Sheet Mask",Brand="Makeup Revolution",
                    Description="A vitamin C infused sheet mask.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/REBE1754F_1.jpg_s3.lmb_bv7mohf/9075035c19dcff8fc0578a4248483f3c/REBE1754F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2=null,
                    PicUrl3=null,
                    Stock=15, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Revolution X Friends Ross Tea Tree Sheet Mask",
                    Price=(float)(5.25),Amount=20,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Sheet Mask",Brand="Makeup Revolution",
                    Description="A tea tree infused sheet mask.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/REBE1756F_1.jpg_s3.lmb_zatjt5/cff937d6e0beff5d0ee1bf482179ca02/REBE1756F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2=null,
                    PicUrl3=null,
                    Stock=15, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Revolution X Friends Rachel Hyaluronic Sheet Mask",
                    Price=(float)(5.25),Amount=20,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Sheet Mask",Brand="Makeup Revolution",
                    Description="A hyaluronic acid infused sheet mask.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/REBE1752F_1.jpg_s3.lmb_kjvu7e/4d1c5ca3504d4259b94971a84c495402/REBE1752F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2=null,
                    PicUrl3=null,
                    Stock=15, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Revolution X Friends Joey Salicylic Sheet Mask",
                    Price=(float)(5.25),Amount=20,Gender=Genders.Unisex,
                    Category=Categories.Skincare,Type="Sheet Mask",Brand="Makeup Revolution",
                    Description="A salicylic acid infused sheet mask.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/REBE1757F_1.jpg_s3.lmb_jospx/252fc3391b7b83cb2fe8c19ec84ddfdb/REBE1757F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2=null,
                    PicUrl3=null,
                    Stock=15, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Lash Sensational Mascara",
                    Price=(float)(10.25),Amount=20,Gender=Genders.Unisex,
                    Category=Categories.Makeup,Type="Mascara",Brand="Maybelline",
                    Description="A volumising mascara.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/MAYB0059F_1.jpg_s3.lmb_n9cuxj/ba153194a0a265246de0ec9c1bd472ac/MAYB0059F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2 ="https://images.beautybay.com/eoaaqxyywn6o/MAYB0059F_2.jpg_s3.lmb_kvhkie/c555060d5a2ced978665094d015a6a83/MAYB0059F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3=null,
                    Stock=30, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Great Lash Mascara",
                    Price=(float)(6.95),Amount=20,Gender=Genders.Unisex,
                    Category=Categories.Makeup,Type="Mascara",Brand="Maybelline",
                    Description="A lengthening and thickening mascara.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/MAYB0053F_1.jpg_s3.lmb_cbys28/39a2d83838bfa220fb7a4cfa0706dbaf/MAYB0053F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2 ="https://images.beautybay.com/eoaaqxyywn6o/MAYB0053F_2.jpg_s3.lmb_f55vl/bf4ca67b42925109954fa4845d13835e/MAYB0053F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3=null,
                    Stock=4, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Built To Lash Mascara",
                    Price=(float)(6.95),Amount=20,Gender=Genders.Unisex,
                    Category=Categories.Makeup,Type="Mascara",Brand="florence by mills",
                    Description="A lengthening black mascara.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/FLOR0057F_1.jpg_s3.lmb_kse4a7/a2260323403ffd127811479826af084d/FLOR0057F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2 ="https://images.beautybay.com/eoaaqxyywn6o/FLOR0057F_2.jpg_s3.lmb_wlggmn/50d5febfcd8626259e8743042a92c7fc/FLOR0057F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/FLOR0057F_3.jpg_s3.lmb_a32sv8/2a194a535e124b348c49d2405c8eb176/FLOR0057F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=15, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Lipgloss - #Clear To Me",
                    Price=(float)(15),Amount=10,Gender=Genders.Unisex,
                    Category=Categories.Makeup,Type="Lipgloss",Brand="Doll Beauty",
                    Description="A high-shine lip gloss.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/DOBE0142F_1.jpg_s3.lmb_i468ed/8316d3767c12b9fb63fbf40d658d7795/DOBE0142F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/DOBE0142F_2.jpg_s3.lmb_0gf67m/48000eff08ee7ea99c460847cf0ee737/DOBE0142F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/DOBE0142F_3.jpg_s3.lmb_tgoyl/119b3ad948125b93c80751edfad4adb4/DOBE0142F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=40, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Lipgloss - #Glitterally Smitten",
                    Price=(float)(15),Amount=10,Gender=Genders.Unisex,
                    Category=Categories.Makeup,Type="Lipgloss",Brand="Doll Beauty",
                    Description="A high-shine lip gloss.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/DOBE0144F_1.jpg_s3.lmb_2d55rp/a6dc1c10fdb4030a91423c1c28730007/DOBE0144F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/DOBE0144F_2.jpg_s3.lmb_hgwgtt/db834916d5f93d7c6218361186d28b99/DOBE0144F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/DOBE0144F_3.jpg_s3.lmb_1vzw8c/862544ef33ca04935fe2400f20536327/DOBE0144F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=23, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Lipgloss - #Instaglam",
                    Price=(float)(15),Amount=7,Gender=Genders.Unisex,
                    Category=Categories.Makeup,Type="Lipgloss",Brand="Doll Beauty",
                    Description="A high-shine lip gloss. A berry pink metallic with gold reflect.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/DOBE0079F_1.jpg_s3.lmb_pwxdbk/5c08bb408b876f6b70075d4d97a442db/DOBE0079F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/DOBE0079F_2.jpg_s3.lmb_admhs7/b76fc598d93d5cfdf15a07c78cbe7b11/DOBE0079F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/DOBE0079F_3.jpg_s3.lmb_i0ec7/0af30146f3efb7790715d40583b322aa/DOBE0079F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=6, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Lipgloss - #Gobby",
                    Price=(float)(15),Amount=7,Gender=Genders.Unisex,
                    Category=Categories.Makeup,Type="Lipgloss",Brand="Doll Beauty",
                    Description="A high-shine lip gloss. A pink-toned nude.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/DOBE0077F_1.jpg_s3.lmb_5pmg99/a983e3a8b37d1035f1d21daefd9af0fe/DOBE0077F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/DOBE0077F_2.jpg_s3.lmb_7fzlj/00a146ab795d89c3b9ffe11b4345b35d/DOBE0077F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/DOBE0077F_3.jpg_s3.lmb_o97teq/5cb71a20ae8735fc2fa46ddc1392d102/DOBE0077F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=8, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="Lipgloss - #Blogged",
                    Price=(float)(15),Amount=7,Gender=Genders.Unisex,
                    Category=Categories.Makeup,Type="Lipgloss",Brand="Doll Beauty",
                    Description="A high-shine lip gloss. A metallic rose gold nude with gold reflect.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/DOBE0076F_1.jpg_s3.lmb_wkv2xi/da8c4ab9de5c51738bc57876857a4f56/DOBE0076F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/DOBE0076F_2.jpg_s3.lmb_kwi9ah/ada092c1e8dcb3ba188a140fa355ada1/DOBE0076F_2.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3="https://images.beautybay.com/eoaaqxyywn6o/DOBE0076F_3.jpg_s3.lmb_lw7pqj/f47d4c55ff473e18ffdfe8456438c86e/DOBE0076F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    Stock=6, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="No.7 Bonding Oil",
                    Price=(float)(28),Amount=30,Gender=Genders.Unisex,
                    Category=Categories.Haircare,Type="Hair Oil",Brand="OLAPLEX",
                    Description="A reparative, weightless styling oil.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/OLAP0008F_1.jpg_s3.lmb_1656k/6933dd078260f1a44783408ac28c9e36/OLAP0008F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2="https://images.beautybay.com/eoaaqxyywn6o/OLAP0008F_3.jpg_s3.lmb_spaxcq/b13dc1deac0fa2b730b47b89e3a118af/OLAP0008F_3.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl3=null,
                    Stock=15, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                },
                new Product{Name="No.4 Bond Maintenance Shampoo",
                    Price=(float)(28),Amount=250,Gender=Genders.Unisex,
                    Category=Categories.Haircare,Type="Shampoo",Brand="OLAPLEX",
                    Description="A repairing shampoo.",
                    PicUrl1="https://images.beautybay.com/eoaaqxyywn6o/OLAP0002F_1.jpg_s3.lmb_1e1ojn/0fd98d88b007c4a8b85e2e7191cbf9dc/OLAP0002F_1.jpg?w=1000&fm=jpg&fl=progressive&q=70",
                    PicUrl2=null,
                    PicUrl3=null,
                    Stock=15, Active=true, InOrders=new List<ProductInOrder>(), InWishList=new List<ProductInWishList>(), Reviews=new List<Review>()
                }
            };
            //product add
            foreach (Product p in products)
            {
                context.Products.Add(p);
            }
            context.SaveChanges();
        }
    }
}
