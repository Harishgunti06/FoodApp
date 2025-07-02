namespace FoodOrderingWebServices.Models
{
    public class Issue
    {
        public int IssueId { get; set; }
        public int OrderItemId { get; set; }
        public string Email { get; set; }

        public string IssueDescription { get; set; }

        public string IssueStatus { get; set; }

    }
}
