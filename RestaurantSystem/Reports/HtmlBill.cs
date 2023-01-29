using RestaurantSystem.Strukts;

namespace RestaurantSystem.Reports
{
    public class HtmlBill
    {
        public string generateHTMLraport(BillInfo bill)
        {
            string htmlOutput = "";
            htmlOutput += "<!DOCTYPE html><html lang = 'en'><head><meta charset = 'UTF-8'/><title>Bill</title></head><body>";
            htmlOutput += "<table style='border: solid 1px black;'>";
            htmlOutput += "<tr style='background-color: rgba(0, 0, 0, 0.8); color: rgb(255, 255, 255);'><th>Užsakymas</th><th>Kaina, Eur</th></th>";
            List<MenuItem> Items = bill.MenuItems;
            foreach (MenuItem item in Items)
            {
                htmlOutput += $"<tr><td>{item.MealName}</td><td style='text-align: center;'>{item.MealPrice:0.00}</td></tr>";
            }
            htmlOutput += $"<tr style='background-color: rgba(0, 0, 0, 0.8); color: rgb(255, 255, 255);'><td>Viso:</td><td style='text-align: center;'>{bill.TotalValue:0.00}</td></tr>";
            htmlOutput += $"<tr><td>Staliukas: {bill.TableId}</td><td style='text-align: right;'>{bill.OccupiedSeats}/{bill.Seats}</td></tr>";
            htmlOutput += $"<tr><td>Data</td><td style='text-align: right;'>{bill.Date} {bill.Time}</td></tr>";
            htmlOutput += $"<tr><td>Sąskaitos numeris:</td><td style='text-align: right;'>#{bill.AccountingId}</td></tr>";
            htmlOutput += "</table>";
            htmlOutput += "</body></html>";
            return htmlOutput;
        }
    }
}
