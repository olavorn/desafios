using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagCerto.Enums
{
    public enum TransactionStatus
    {
        Waiting,
        InAnalisys,
        Done
    }

    public enum TransactionResult
    {
        Approved,
        Rejected
    }

}
