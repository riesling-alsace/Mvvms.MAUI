namespace Riesling.Mvvms.MAUI;

public class ViewModelBase : ModelBase {

	#region Raise Methods

	protected override void RaisePropertyChanged(params string[] propertyNames) {
		if (MainThread.IsMainThread) {
			base.RaisePropertyChanged(propertyNames);
		} else {
			MainThread.BeginInvokeOnMainThread(() => {
				lock (this) {
					base.RaisePropertyChanged(propertyNames);
				}
			});
		}
	}

	#endregion

}

public abstract class ViewModelBase<TModelBase> : ViewModelBase
    where TModelBase : ModelBase, new() {

    #region Properties

    public TModelBase Model {
        get;
        init {
            field = value;
            Model.PropertiesChanged += Model_PropertiesChanged;
        }
    }

    #endregion

    #region Constructors

    protected ViewModelBase(TModelBase model) {
        Model = model;
    }

    ~ViewModelBase() {
        Model.PropertiesChanged -= Model_PropertiesChanged;
    }

    #endregion

    #region Methods

    protected abstract void Model_PropertiesChanged(object? sender, PropertiesChangedEventArgs e);

    #endregion

}
