namespace BusinessLogic.Clustering
{
    public class ClusterRange
    {
        public double From { get; set; }
        public double To { get; set; }

        public ClusterRange()
        {
            From = double.MaxValue;
            To = double.MinValue;
        }

        public ClusterRange(double from, double to)
        {
            From = from;
            To = to;
        } 
    }
}
