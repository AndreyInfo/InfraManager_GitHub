using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Accounts
{
    public enum CryptographicAlgorithms
    {
        None = 0,
        DES3 = 1, 
        AES128 = 2, 
        AES192 = 3, 
        AES256 = 4, 
        DES = 5
    }
}
