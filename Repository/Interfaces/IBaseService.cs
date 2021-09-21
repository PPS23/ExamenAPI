using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
/*
    Version: 1.0
    Author: Pedro Pablo Soto
*/
namespace Repository.Interfaces
{
    public interface IBaseService<T> where T: class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Save(T model);
        Task<T> Update(T model, int id);
        Task<T> Delete(int id);
    }
}
