using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
namespace Application.Repositories;

public interface IOrderRepository:ICommandRepository<Order> , IQueryRepository<Order>
{

}
