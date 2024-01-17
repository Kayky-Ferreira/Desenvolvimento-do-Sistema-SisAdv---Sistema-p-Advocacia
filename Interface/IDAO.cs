using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisAdv.Interface
{
    interface IDAO<T>
    {
        /// <summary>
        ///     Interface (contrato) para classes DAO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// 

        void Insert(T t);

        void Update(T t);

        void Delete(T t);

        List<T> List();

        T GetById(int id);
    }
}
