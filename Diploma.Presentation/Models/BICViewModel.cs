namespace Diploma.Presentation.Models
{
    public class BICViewModel
    {
        public int Component { get; set; }
        public double EM_BIC_1_cluster { get; set; }
        public double EM_BIC_2_cluster { get; set; }
        public double SEM_BIC_1_cluster { get; set; }
        public double SEM_BIC_2_cluster { get; set; }
        public int EM_Recom_Amount_Of_Clusters { get; set; }
        public int SEM_Recom_Amount_Of_Clusters { get; set; }
    }
}