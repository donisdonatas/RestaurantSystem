namespace RestaurantSystem.Models
{
    public class Table
    {
        //public int TableID;
        public int Seats;
        private bool _isReserved;
        private bool _isOrderAccepted;

        public bool IsReserved
        {
            get { return _isReserved; }
            set { _isReserved = value; }
        }

        public bool IsOrderAccepted
        {
            get { return _isOrderAccepted; }
            set { _isOrderAccepted = value; }
        }

        public Table(int seats)
        {
            Seats = seats;
            IsReserved = false;
            IsOrderAccepted = false;
        }
    }
}
