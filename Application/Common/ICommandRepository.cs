using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common;

public interface ICommandRepository<T>
{
    int Create(T entity);
    int Update(T entity);
}
