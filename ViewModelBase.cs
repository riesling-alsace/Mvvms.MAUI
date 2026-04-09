namespace Riesling.Mvvms.MAUI;

public class ViewModelBase : PropertyBase {

	#region Raise Methods

	public override void RaisePropertiesChanged(params string[] propertyNames) {
		if (MainThread.IsMainThread) {
			base.RaisePropertiesChanged(propertyNames);
		} else {
			MainThread.BeginInvokeOnMainThread(() => {
				lock (this) {
					base.RaisePropertiesChanged(propertyNames);
				}
			});
		}
	}

	#endregion

}

public abstract class ViewModelBase<TModelBase> : ViewModelBase
    where TModelBase : PropertyBase, new() {

    #region Properties

    public TModelBase Model {
        get;
        private set;
    }

    #endregion

    #region Constructors

    protected ViewModelBase(TModelBase model) {
        Model = model;
        Model.PropertiesChanged += Model_PropertiesChanged;
    }

    ~ViewModelBase() {
        Model.PropertiesChanged -= Model_PropertiesChanged;
    }

    #endregion

    #region Methods

    protected abstract void Model_PropertiesChanged(object? sender, PropertiesChangedEventArgs e);

    #endregion

}
