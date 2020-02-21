using System.Collections.Generic;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Web
{
    public class ComponentType : Entity
    {
        public long Id { get; set; }
        
        public string Slug { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public ICollection<Component> Components { get; set; }
    }

/*
    /// <summary>
    /// Reminder to everyone that this component type is only for RequestComponents.
    /// </summary>
    public enum ComponentType
    {
        [Comparable(false)]
        [Description("Unknown")]
        Unknown = 0, // string
        [Comparable(true)]
        [Description("Ask")]
        Ask = 1, // float
        [Comparable(false)]
        [Description("Ask period covered in days")]
        AskPeriod = 7, // int
        [Comparable(false)]
        [Description("Sum of the 25 lowest ask sizes")]
        AskSize = 8, // float
        [Comparable(true)]
        [Description("Bid")]
        Bid = 2, // float
        [Comparable(false)]
        [Description("Bid period covered in days")]
        BidPeriod = 4, // int
        [Comparable(false)]
        [Description("Sum of the 25 highest bid sizes")]
        BidSize = 5, // float
        [Comparable(false)]
        [Description("Flash Return Rate")]
        FRR = 3,	// float - average of all fixed rate funding over the last hour
        [Comparable(true)]
        [Description("Daily price change")]
        DailyChange = 9, // float
        [Comparable(true)]
        [Description("Daily price change expressed in percentage terms")]
        DailyChangePerc = 10, // float
        [Comparable(true)]
        [Description("Daily volume")]
        DailyVolume = 12, // float
        [Comparable(true)]
        [Description("Daily high")]
        High = 13, // float
        [Comparable(true)]
        [Description("Daily low")]
        Low	= 14, // float
        
        // Orderbook related enumerators.
        [Comparable(false)]
        [Description("Order")]
        Order = 100,
        [Comparable(true)]
        [Description("Price of the last successfully closed order")]
        LastPrice = 101, // float
        
        [Description("Supply % interest")]
        SupplyRate = 200,
        [Description("Borrow % interest")]
        BorrowRate = 201,
        [Description("Supply reserve amount")]
        SupplyReserve = 209,
        [Description("Borrowing collateral factor")]
        CollateralFactor = 210,
        [Description("Total supply amount for loans")]
        TotalLoanSupply = 211,

        [Comparable(false)]
        [Description("The current circulating supply of this asset.")]
        CirculatingSupply = 1000,
        [Comparable(false)]
        [Description("The current block count of this crypto.")]
        BlockCount = 1005,
        [Comparable(false)]
        [Description("The current mining difficulty of this asset.")]
        Difficulty = 1010,
        
        // Includes currency in circulation, though not currency held in the treasury, Reserve banks, and bank vaults.
        // It includes all traveler's checks and domestic checking account deposits, including those that pay interest.
        // However, it does not count checking deposits held in government accounts and in foreign banks. 
        [Description("Money Supply (M1)")]
        MoneySupplyM1 = 2000,
        // Includes everything in M1. adds savings accounts, money market accounts, and money market mutual funds,
        // along with time deposits under $100,000.
        [Description("Money Supply (M2)")]
        MoneySupplyM2 = 2001,
        // Includes everything in M2 as well as some longer-term time deposits and money market funds.
        [Description("Money Supply (M3)")]
        MoneySupplyM3 = 2002,
        // Includes everything in M3 as well as some additional deposits
        [Description("Money Supply (M4)")]
        MoneySupplyM4 = 2003,
        [Description("Quasi-money consists of highly liquid assets which are not cash but can easily be converted " +
                     "into cash")]
        QuasiMoneySupply = 2040,
    }
    */
}