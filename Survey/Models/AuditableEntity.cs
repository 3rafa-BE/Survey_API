namespace Survey.Models
{
    public class AuditableEntity
    {
        public string CreatedByID { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? UpdatedByID { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public ApplicationUser CreatedBy { get; set; } = default!;
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
