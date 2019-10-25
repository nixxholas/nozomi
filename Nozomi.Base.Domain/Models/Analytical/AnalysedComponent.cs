using System;
using System.Collections.Generic;
using System.Linq;
using Nozomi.Base.Core.Models;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.Models.Analytical
{
    /// <summary>
    /// Component made only in runtime.
    /// </summary>
    public class AnalysedComponent : Entity
    {
        public AnalysedComponent() {}
        
        /// <summary>
        /// Constructor that defines the object with pagination
        /// </summary>
        /// <param name="component">The component we're constructing</param>
        /// <param name="index">Treat this like a page.</param>
        /// <param name="items">Treat this like defining the number of sentences in a page, where a sentence = an item.</param>
        public AnalysedComponent(AnalysedComponent component, int index = 0, int items = 100)
        {
            Id = component.Id;
            ComponentType = component.ComponentType;
            Currency = component.Currency;
            CurrencyId = component.CurrencyId;
            CurrencyPair = component.CurrencyPair;
            CurrencyPairId = component.CurrencyPairId;
            CurrencyType = component.CurrencyType;
            CurrencyTypeId = component.CurrencyTypeId;
            Value = component.Value;
            IsFailing = component.IsFailing;
            StoreHistoricals = component.StoreHistoricals;
            IsDenominated = component.IsDenominated;
            Delay = component.Delay;
            UIFormatting = component.UIFormatting;
            AnalysedHistoricItems = component.AnalysedHistoricItems != null 
                ? component.AnalysedHistoricItems
                .OrderByDescending(ahi => ahi.HistoricDateTime)
                .Skip(index * items)
                .Take(items)
                .ToList() 
                : new List<AnalysedHistoricItem>();
        }

        /// <summary>
        /// Constructor that defines the orderby clause in order to order historic items by a custom order.
        /// </summary>
        /// <param name="component">The component we're constructing</param>
        /// <param name="index">Treat this like a page.</param>
        /// <param name="items">Treat this like defining the number of sentences in a page, where a sentence = an item.</param>
        /// <param name="orderingExpr">The expression for the ordering.</param>
        public AnalysedComponent(AnalysedComponent component, int index = 0, int items = 100,
            Func<AnalysedHistoricItem, object> orderingExpr = null)
        {
            Id = component.Id;
            ComponentType = component.ComponentType;
            Currency = component.Currency;
            CurrencyId = component.CurrencyId;
            CurrencyPair = component.CurrencyPair;
            CurrencyPairId = component.CurrencyPairId;
            CurrencyType = component.CurrencyType;
            CurrencyTypeId = component.CurrencyTypeId;
            Value = component.Value;
            IsFailing = component.IsFailing;
            StoreHistoricals = component.StoreHistoricals;
            IsDenominated = component.IsDenominated;
            Delay = component.Delay;
            UIFormatting = component.UIFormatting;

            if (component.AnalysedHistoricItems == null)
            {
                AnalysedHistoricItems = new List<AnalysedHistoricItem>();
            }
            else
            {
                if (orderingExpr != null)
                {
                    AnalysedHistoricItems = component.AnalysedHistoricItems
                        .OrderByDescending(orderingExpr)
                        .Skip(index * items)
                        .Take(items)
                        .ToList();
                }
                else
                {
                    AnalysedHistoricItems = component.AnalysedHistoricItems
                        .OrderByDescending(ahi => ahi.HistoricDateTime)
                        .Skip(index * items)
                        .Take(items)
                        .ToList();
                }
            }
        }
        
        /// <summary>
        /// Constructor that defines the where clause in order to filter certain historic items 
        /// </summary>
        /// <param name="component">The component we're constructing</param>
        /// <param name="index">Treat this like a page.</param>
        /// <param name="items">Treat this like defining the number of sentences in a page, where a sentence = an item.</param>
        /// <param name="whereExpr">The expression for the filter</param>
        public AnalysedComponent(AnalysedComponent component, int index = 0, int items = 100,
            Func<AnalysedHistoricItem, bool> whereExpr = null)
        {
            Id = component.Id;
            ComponentType = component.ComponentType;
            Currency = component.Currency;
            CurrencyId = component.CurrencyId;
            CurrencyPair = component.CurrencyPair;
            CurrencyPairId = component.CurrencyPairId;
            CurrencyType = component.CurrencyType;
            CurrencyTypeId = component.CurrencyTypeId;
            Value = component.Value;
            IsFailing = component.IsFailing;
            StoreHistoricals = component.StoreHistoricals;
            IsDenominated = component.IsDenominated;
            Delay = component.Delay;
            UIFormatting = component.UIFormatting;

            if (component.AnalysedHistoricItems == null)
            {
                AnalysedHistoricItems = new List<AnalysedHistoricItem>();
            }
            else
            {
                if (whereExpr != null)
                {
                    AnalysedHistoricItems = component.AnalysedHistoricItems
                        .Where(whereExpr)
                        .OrderBy(ahi => ahi.HistoricDateTime)
                        .Skip(index * items)
                        .Take(items)
                        .ToList();
                }
                else
                {
                    AnalysedHistoricItems = component.AnalysedHistoricItems
                        .OrderBy(ahi => ahi.HistoricDateTime)
                        .Skip(index * items)
                        .Take(items)
                        .ToList();
                }   
            }
        }
        
        /// <summary>
        /// Constructor that defines the where and orderby clauses in order to filter and order historic items.
        /// </summary>
        /// <param name="component">The component we're constructing</param>
        /// <param name="index">Treat this like a page.</param>
        /// <param name="items">Treat this like defining the number of sentences in a page, where a sentence = an item.</param>
        /// <param name="orderingExpr">The expression for the ordering.</param>
        /// <param name="whereExpr">The expression for the filter</param>
        public AnalysedComponent(AnalysedComponent component, int index = 0, int items = 100,
            Func<AnalysedHistoricItem, object> orderingExpr = null, Func<AnalysedHistoricItem, bool> whereExpr = null)
        {
            Id = component.Id;
            ComponentType = component.ComponentType;
            Currency = component.Currency;
            CurrencyId = component.CurrencyId;
            CurrencyPair = component.CurrencyPair;
            CurrencyPairId = component.CurrencyPairId;
            CurrencyType = component.CurrencyType;
            CurrencyTypeId = component.CurrencyTypeId;
            Value = component.Value;
            IsFailing = component.IsFailing;
            StoreHistoricals = component.StoreHistoricals;
            IsDenominated = component.IsDenominated;
            Delay = component.Delay;
            UIFormatting = component.UIFormatting;

            if (component.AnalysedHistoricItems == null)
            {
                AnalysedHistoricItems = new List<AnalysedHistoricItem>();
            }
            else
            {
                if (orderingExpr != null && whereExpr != null)
                {
                    AnalysedHistoricItems = component.AnalysedHistoricItems
                        .Where(whereExpr)
                        .OrderByDescending(orderingExpr)
                        .Skip(index * items)
                        .Take(items)
                        .ToList();
                }
                else if (orderingExpr != null)
                {
                    AnalysedHistoricItems = component.AnalysedHistoricItems
                        .OrderByDescending(orderingExpr)
                        .Skip(index * items)
                        .Take(items)
                        .ToList();
                }
                else if (whereExpr != null)
                {
                    AnalysedHistoricItems = component.AnalysedHistoricItems
                        .Where(whereExpr)
                        .Skip(index * items)
                        .Take(items)
                        .ToList();
                }
                else
                {
                    AnalysedHistoricItems = component.AnalysedHistoricItems
                        .OrderByDescending(ahi => ahi.HistoricDateTime)
                        .Skip(index * items)
                        .Take(items)
                        .ToList();
                }
            }
        }
        
        public long Id { get; set; }
        
        public AnalysedComponentType ComponentType { get; set; }
        
        public string Value { get; set; }

        public bool IsDenominated { get; set; } = false;
        
        public bool IsFailing { get; set; }
        
        public bool StoreHistoricals { get; set; }
        
        public int Delay { get; set; }

        /// <summary>
        /// Defines the formatting for frontend libs.
        /// 
        /// i.e. for Numeral.js - '(0,0.0000)' to display (10,000.0000)
        /// 
        /// We shouldn't peg this to an AnalysedComponentType because there
        /// may be different variation on the same type
        /// </summary>
        /// <value>The UIF ormatting.</value>
        public string UIFormatting { get; set; }
        
        public long? CurrencyId { get; set; }
        
        public Currency.Currency Currency { get; set; }
        
        public long? CurrencyPairId { get; set; }
        
        public CurrencyPair CurrencyPair { get; set; }
        
        public long? CurrencyTypeId { get; set; }
        
        public CurrencyType CurrencyType { get; set; }
        
        public ICollection<AnalysedHistoricItem> AnalysedHistoricItems { get; set; }
    }
}