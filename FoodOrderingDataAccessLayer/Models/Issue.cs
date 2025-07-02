using System;
using System.Collections.Generic;

namespace FoodOrderingDataAccessLayer.Models;

public partial class Issue
{
    public int IssueId { get; set; }

    public int OrderItemId { get; set; }

    public string Email { get; set; } = null!;

    public string IssueDescription { get; set; } = null!;

    public string IssueStatus { get; set; } = null!;

    public virtual User EmailNavigation { get; set; } = null!;

    public virtual OrderItem OrderItem { get; set; } = null!;
}
