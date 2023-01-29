namespace RestaurantSystem.Strukts
{
    public struct BillInfo
    {
        public string Date;
        public string Time;
        public int TableId;
        public int Seats;
        public int OccupiedSeats;
        public int AccountingId;
        public decimal TotalValue;
        public List<MenuItem> MenuItems;
    }
}
