namespace Riesling.Mvvms.MAUI;

public class ViewModelBase : ModelBase {

	#region Raise Methods

	protected override void RaisePropertiesChanged(params string[] propertyNames) {
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
    where TModelBase : ModelBase, new() {

    #region Properties

    public required TModelBase Model {
        get;
        init {
            field = value;
            Model.PropertiesChanged += Model_PropertiesChanged;
        }
    }

    #endregion

    #region Constructors

    ~ViewModelBase() {
        Model.PropertiesChanged -= Model_PropertiesChanged;
    }

    #endregion

    #region Methods

    protected abstract void Model_PropertiesChanged(object? sender, PropertiesChangedEventArgs e);

    #endregion

}
