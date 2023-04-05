using CardanoSharp.Wallet.Models;
using MonkeyWallet.Core.Data.Models;
using MonkeyWallet.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyWallet.Core.Contracts;

public interface IGetUtxosService
{
    Task<List<AddressUtxoSet>> Execute(Wallet wallet);
}
