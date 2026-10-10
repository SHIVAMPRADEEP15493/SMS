using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Domain.Entities;
public sealed class ClassMst
{
    private ClassMst()
    {
    }
    public long ClassId { get; private set; }
    public string ClassCode { get; private set; } = null!;
    public string ClassName { get; private set; } = null!;
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public long? CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public long? UpdatedBy { get; private set; }
    public ClassMst(
        string classCode,
        string className,
        int displayOrder,
        long? createdBy = null)
    {
        ClassId = 0;

        ClassCode = classCode;
        ClassName = className;
        DisplayOrder = displayOrder;

        IsActive = true;

        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }
    public void Update(
        string className,
        int displayOrder,
        long? updatedBy = null)
    {
        ClassName = className;
        DisplayOrder = displayOrder;

        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
    public void Activate(long? updatedBy = null)
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
    public void Deactivate(long? updatedBy = null)
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
