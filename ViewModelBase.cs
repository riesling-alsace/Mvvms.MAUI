using System.ComponentModel;

namespace Riesling.Mvvms.MAUI;

public class ViewModelBase : BindableBase {

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
	where TModelBase : BindableBase {

	#region Fields

	protected TModelBase? _Model;

	#endregion

	#region Properties

	public required TModelBase Model {
		get => _Model!;
		set => SetProperty(ref _Model, value, Model_Changed);
	}

	protected virtual void Model_Changed(TModelBase? newModel, TModelBase? oldModel) {
		if (oldModel != null) {
			oldModel.PropertyChanged -= Model_PropertyChanged;
		}
		if (newModel != null) {
			newModel.PropertyChanged += Model_PropertyChanged;
		}
	}

	#endregion

	#region Constructors

    protected ViewModelBase(TModelBase model) {
		Model = model;
	}

	protected abstract void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e);

	#endregion

}
