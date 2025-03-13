public class LeaseAgreementDTO
{
    public int PropertyID { get; set; } // Required
    public int TenantID { get; set; } // Required
    public DateTime StartDate { get; set; } // Required
    public DateTime EndDate { get; set; } // Required
    public string TenantSignaturePath { get; set; } // Required
    public string TenantDocumentPath { get; set; } // Required

    }