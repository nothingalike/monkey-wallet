using CardanoSharp.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyWallet.Core.Dto;

public class AddressUtxoSet
{
    public string Address { get; set; }
    public int Index { get; set; }
    public List<Utxo> Utxos { get; set; }
}
