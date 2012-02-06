using System.Windows;
using System.Windows.Controls;


namespace WPF.RealTime.Data.Binding
{
    public class WpfGridColumn : DataGridTemplateColumn
    {

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            TextBox editElement = new TextBox {BorderThickness = new Thickness(0.0), Padding = new Thickness(0.0)};

            System.Windows.Data.Binding textBinding = new System.Windows.Data.Binding(cell.Column.Header + ".DisplayValue")
                                                          {Source = dataItem};
            editElement.SetBinding(TextBox.TextProperty, textBinding);

            System.Windows.Data.Binding backgroundBinding = new System.Windows.Data.Binding(cell.Column.Header + ".Background")
                                                                {Source = dataItem};
            editElement.SetBinding(TextBlock.BackgroundProperty, backgroundBinding);

            System.Windows.Data.Binding foreGroundBinding = new System.Windows.Data.Binding(cell.Column.Header + ".Foreground")
                                                                {Source = dataItem};
            editElement.SetBinding(TextBlock.ForegroundProperty, foreGroundBinding);

            return editElement;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {

            TextBlock cellElement = new TextBlock();

            System.Windows.Data.Binding textBinding = new System.Windows.Data.Binding(cell.Column.Header + ".DisplayValue")
                                                          {Source = dataItem};
            cellElement.SetBinding(TextBlock.TextProperty, textBinding);

            System.Windows.Data.Binding backgroundBinding = new System.Windows.Data.Binding(cell.Column.Header + ".Background")
                                                                {Source = dataItem};
            cellElement.SetBinding(TextBlock.BackgroundProperty, backgroundBinding);

            System.Windows.Data.Binding foreGroundBinding = new System.Windows.Data.Binding(cell.Column.Header + ".Foreground")
                                                                {Source = dataItem};
            cellElement.SetBinding(TextBlock.ForegroundProperty, foreGroundBinding);

            return cellElement;
        }
    }
}
