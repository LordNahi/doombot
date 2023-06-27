namespace Util
{
    public class Target
    {
        public double Tx { get; set; }
        public double Ty { get; set; }
        public int Tid { get; set; }

        public Target(double X, double Y, int Id)
        {
            Tx = X;
            Ty = Y;
            Tid = Id;
        }
    }
}