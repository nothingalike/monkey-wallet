﻿using CardanoSharp.Koios.Client.Contracts;
using SQLite;

namespace MonkeyWallet.Core.Data.Models
{
    public class Wallet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int WalletType { get; set; }

        [Ignore]
        public List<WalletKey> Keys { get; set; }

        [Ignore]
        public List<AddressInformation> AddressInformation { get; set; }
    }
}
