using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Accounts
{
    public enum HashAlgorithms
    {
        None = 0,
        MD5 = 1, 
        SHA_1 = 2, 
        SHA_224 = 3, 
        SHA_256 = 4, 
        SHA_384 = 5, 
        SHA_512 = 6
    }
}
