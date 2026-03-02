using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; init; }
    //public bool IsDeleted { get;  private set; } = false;

    //public void MarkAsDeleted()=> IsDeleted = true;


}
