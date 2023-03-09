using SQLite;

namespace MonkeyWallet.Core.Data.Models;

public class Settings
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}