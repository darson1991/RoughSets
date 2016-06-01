namespace BusinessLogic.KMeans
{
    public class ClusterRange
    {
        public double From { get; set; }
        public double To { get; set; }

        public ClusterRange(double from, double to)
        {
            From = from;
            To = to;
        } 
    }
}
