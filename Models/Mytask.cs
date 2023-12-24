using System;
using System.Collections.Generic;

namespace TaskProject.Models;

public partial class Mytask
{
    public decimal Taskid { get; set; }

    public string? Taskname { get; set; }

    public string? Taskdescription { get; set; }

    public DateTime? Duedate { get; set; }

    public string? Priority { get; set; }

    public string? Status { get; set; }

    public DateTime? Createddate { get; set; }
}
