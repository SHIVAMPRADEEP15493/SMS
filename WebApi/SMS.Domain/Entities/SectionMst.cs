using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Domain.Entities;
public sealed class SectionMst
{
    public long SectionId { get; private set; }
    public string SectionCode { get; private set; } = null!;
    public string SectionName { get; private set; } = null!;
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public long? CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public long? UpdatedBy { get; private set; }
    // EF Core
    private SectionMst()
    {
    }
    public SectionMst(
        string sectionCode,
        string sectionName,
        int displayOrder)
    {
        SectionCode = sectionCode;
        SectionName = sectionName;
        DisplayOrder = displayOrder;

        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }
    public void Update(
        string sectionName,
        int displayOrder)
    {
        SectionName = sectionName;
        DisplayOrder = displayOrder;

        UpdatedAt = DateTime.UtcNow;
    }
    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
