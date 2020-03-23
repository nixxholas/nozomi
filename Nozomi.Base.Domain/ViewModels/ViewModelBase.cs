namespace Nozomi.Data.ViewModels
{
    /// <summary>
    /// Base abstract class for utilising inheritance in MVVM.
    /// </summary>
    /// <typeparam name="TModel">The base/parent object</typeparam>
    public abstract class ViewModelBase<TModel> where TModel : class
    {
        protected ViewModelBase(TModel dataObject)
        {
            DataObject = dataObject;
        }

        protected TModel DataObject { get; }
    }
}