using System.Collections.Generic;
using Newtonsoft.Json;
using Stripe;

namespace Nozomi.Base.Auth.ViewModels.Payment
{
    public class AddPaymentMethodInputModel
    {
        /// <summary>
        /// Unique identifier for the object.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// String representing the object’s type. Objects of the same type share the same value.
        /// </summary>
        [JsonProperty("object")]
        public string Object { get; set; }

        #region Expandable Application

        // /// <summary>
        // /// ID of the Connect application that created the SetupIntent.
        // /// </summary>
        // [JsonIgnore]
        // public string ApplicationId
        // {
        //     get => this.InternalApplication?.Id;
        //     set => this.InternalApplication = SetExpandableFieldId(value, this.InternalApplication);
        // }
        //
        // [JsonIgnore]
        // public Application Application
        // {
        //     get => this.InternalApplication?.ExpandedObject;
        //     set => this.InternalApplication = SetExpandableFieldObject(value, this.InternalApplication);
        // }
        //
        // [JsonProperty("application")]
        // [JsonConverter(typeof(ExpandableFieldConverter<Application>))]
        // internal ExpandableField<Application> InternalApplication { get; set; }
        
        #endregion

        /// <summary>
        /// Reason for canceling this SetupIntent. Possible values are <c>abandoned</c>,
        /// <c>requested_by_customer</c>, or <c>duplicate</c>.
        /// </summary>
        [JsonProperty("cancellation_reason")]
        public string CancellationReason { get; set; }

        /// <summary>
        /// <para>
        /// The client secret of this SetupIntent. Used for client-side retrieval using a
        /// publishable key.
        /// </para>
        /// <para>
        /// The client secret can be used to complete payment setup from your frontend. It should
        /// not be stored, logged, embedded in URLs, or exposed to anyone other than the customer.
        /// Make sure that you have TLS enabled on any page that includes the client secret.
        /// </para>
        /// </summary>
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Time at which the object was created. Measured in seconds since the Unix epoch.
        /// </summary>
        [JsonProperty("created")]
        public long Created { get; set; }

        #region Expandable Customer

        // /// <summary>
        // /// ID of the Customer this SetupIntent belongs to, if one exists. If present, payment
        // /// methods used with this SetupIntent can only be attached to this Customer, and payment
        // /// methods attached to other Customers cannot be used with this SetupIntent.
        // /// </summary>
        // [JsonIgnore]
        // public string CustomerId
        // {
        //     get => this.InternalCustomer?.Id;
        //     set => this.InternalCustomer = SetExpandableFieldId(value, this.InternalCustomer);
        // }
        //
        // [JsonIgnore]
        // public Customer Customer
        // {
        //     get => this.InternalCustomer?.ExpandedObject;
        //     set => this.InternalCustomer = SetExpandableFieldObject(value, this.InternalCustomer);
        // }
        //
        // [JsonProperty("customer")]
        // [JsonConverter(typeof(ExpandableFieldConverter<Customer>))]
        // internal ExpandableField<Customer> InternalCustomer { get; set; }
        
        #endregion

        /// <summary>
        /// An arbitrary string attached to the object. Often useful for displaying to users.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        // /// <summary>
        // /// The error encountered in the previous SetupIntent confirmation.
        // /// </summary>
        // [JsonProperty("last_setup_error")]
        // public StripeError LastSetupError { get; set; }

        /// <summary>
        /// A set of key/value pairs that you can attach to an order object. It can be useful for
        /// storing additional information about the order in a structured format.
        /// </summary>
        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }


        #region Expandable PaymentMethod

        /// <summary>
        /// ID of the payment method used with this SetupIntent.
        /// </summary>
        public string PaymentMethodId { get; set; }

        /// <summary>
        /// The list of payment method types (e.g. card) that this SetupIntent is allowed to set up.
        /// </summary>
        public List<string> PaymentMethodTypes { get; set; }
        
        #endregion

        /// <summary>
        /// Status of this SetupIntent, one of <c>requires_payment_method</c>,
        /// <c>requires_confirmation</c>, <c>requires_action</c>, <c>processing</c>,
        /// <c>canceled</c>, or <c>succeeded</c>.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Indicates how the payment method is intended to be used in the future. Use
        /// <c>on_session</c> if you intend to only reuse the payment method when the customer is in
        /// your checkout flow. Use <c>off_session</c> if your customer may or may not be in your
        /// checkout flow. If not provided, this value defaults to <c>off_session</c>.
        /// </summary>
        [JsonProperty("usage")]
        public string Usage { get; set; }
    }
}