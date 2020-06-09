using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BikeStore.Models;

namespace BikeStore.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private Models.DatabaseShopEntities1 db = new Models.DatabaseShopEntities1();
        public int cout;
        private double[][] myArr;
        public double sum = 0;

        public ActionResult Index()
        {
            var Items = db.Bikes;
            return View(Items);
        }

        public ActionResult BikePage(int item_id)
        {
            var Items = db.Bikes.FirstOrDefault(x => x.Id == item_id);
            return View(Items);
        }

        public ActionResult Nav()
        {
            var Items = db.Bikes;
            string result = "";
            foreach (var item in Items)
            {
                result += "<li><a href='/Home/BikePage/?item_id=" + item.Id + "' title='" + item.Manufacturer + "'>" + item.Manufacturer + "</a></li>";
            }
            return Content(result);
        }
       
        public ActionResult Contact()
        {
            var Contact = db.Bikes;
            return View(Contact);
        }
        [HttpGet]
        public ActionResult Catalog()
        {
            var Items = db.Bikes;
            return View(Items);
        }

        [HttpPost]
        public ActionResult Catalog(string Manufacturer, string Type, string from_year, string to_year, string from_price, string to_price, string from_transmission, string to_transmission, string from_weight, string to_weight, string from_wheels, string to_wheels)
        {
            var bikes = from s in db.Bikes
                        select s;

            List<Bikes> Items = new List<Bikes>();
            List<Bikes> Items1 = new List<Bikes>();

            double f_price, t_price;
            double f_transmission, t_transmission;
            double f_weight, t_weight;
            double f_wheels, t_wheels;
            int f_year, t_year;

            Double.TryParse(from_price, out f_price);
            Double.TryParse(to_price, out t_price);

            Double.TryParse(from_transmission, out f_transmission);
            Double.TryParse(to_transmission, out t_transmission);

            Double.TryParse(from_weight, out f_weight);
            Double.TryParse(to_weight, out t_weight);

            Double.TryParse(from_wheels, out f_wheels);
            Double.TryParse(to_wheels, out t_wheels);

            Int32.TryParse(from_year, out f_year);
            Int32.TryParse(to_year, out t_year);


            var items = db.Bikes.Where(q => q.Manufacturer.Contains(Manufacturer) && q.Type.Contains(Type) && (f_price == 0 ? true : f_price <= q.Price) && (t_price == 0 ? true : t_price >= q.Price)
                                        && (f_transmission == 0 ? true : f_transmission <= q.Transmission) && (t_transmission == 0 ? true : t_transmission >= q.Transmission)
                                        && (f_weight == 0 ? true : f_weight  <= q.Weight) && (t_weight == 0 ? true : t_weight >= q.Weight)
                                        && (f_wheels == 0  ? true : f_wheels <= q.Wheels) && (t_wheels == 0 ? true : t_wheels >= q.Wheels)
                                        && (f_year == 0 ? true : f_year <= q.Year) && (t_year == 0 ? true : t_year >= q.Year));
            Items.AddRange(items);
            cout = Items.Count;//поле, хранящее кол-во элементов в списке
            if (cout == 0)
            {
                return View(Items1);
            }
            myArr = new double [cout][];
            List<double> listBetter = new List<double>();

            //создание матрицы сравнения
            int itemCount = 0;
            foreach (var item in items) { 
                myArr[itemCount] = new double[4] {item.Transmission.Value, item.Wheels.Value, item.Price.Value, item.Weight.Value };
                itemCount++;
            }

            //нормализация
            double[] maxs = new double[4] {myArr[0][0], myArr[0][1], myArr[0][2], myArr[0][3]};
            double[] mins = new double[4] {myArr[0][0], myArr[0][1], myArr[0][2], myArr[0][3]};
            double max, min;

            for (int j = 0; j < 4; j++) {
                for (int i = 1; i < cout; i++) {

                    if (maxs[j] < myArr[i][j]) maxs[j] = myArr[i][j];
                    if (mins[j] > myArr[i][j]) mins[j] = myArr[i][j];
                }
            }
            
            for (int i = 0; i < 4; i++) {
                max = maxs[i];
                min = mins[i];
                for (int j = 0; j < cout; j++)
                {
                    if (i < 2)
                    {
                        myArr[j][i] = (myArr[j][i] - min) / (max - min);
                    }
                    else
                    {
                        myArr[j][i] = (max - myArr[j][i]) / (max - min);
                    }
                } 
            }

            for (int i = 0; i < cout; i++)
            {
                sum = 0;
                for (int j = 0; j < 4; j++)
                {
                    sum += myArr[i][j];
                }
                listBetter.Add(sum);
            }

            int iter = 0;
            List<BestBike> listOfBestBike = new List<BestBike>();

            foreach (Bikes item in Items) {
                listOfBestBike.Add(new BestBike { Index = listBetter[iter], Item = item });
                iter++;
            }

            var sortedItems = listOfBestBike.OrderByDescending(u => u.Index);     //отсортированный список       
            
            foreach (BestBike u in sortedItems) {
                Items1.Add(u.Item);
            };
            
            return View(Items1); 
        }
            
        class BestBike
        {
            public double Index { get; set; }

            public Bikes Item { get; set; }
        }

    }


}