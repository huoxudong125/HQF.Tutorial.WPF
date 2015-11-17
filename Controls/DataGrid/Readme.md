
[WPF Datagrid formatting (part 1)](https://csharphardcoreprogramming.wordpress.com/2014/04/29/wpf-datagrid-formatting-part-1/)
------------------------------
This source code demonstrates the use of a simple DataGrid. You can sort the rows by any column. The Age column is horizontally aligned to the right. The column headers are using a bold font.

The DataGrid element itself is very flexible. You can add all kinds of columns. The most interesting one is DataGridTemplateColumn, which allows you to add any template. The example template only uses a DatePicker, but you could add far more complexity to it.

Set the SortMemberPath to enable sorting, otherwise the DataGrid sorting algorithm cannot know what data to look at. Remember, we are using a template and not a clearly identifiable data type. In today’s example SortMemberPath is set to “Birthday.Day”, which sorts by the day of the month. In case you prefer to sort by date in general, use SortMemberPath=”Birthday” instead.

I changed the selection color, because the dark blue had a low contrast compared to the web-links. This is dealt with by style triggers. The advantage of triggers is that they only override properties temporarily. As soon as the trigger becomes invalid the control element returns to its previous formatting.


[WPF Datagrid formatting (part 2, advanced](https://csharphardcoreprogramming.wordpress.com/2014/05/06/wpf-datagrid-formatting-part-2-advanced/)
------------------------------
We stick to the previous DataGrid example and enhance it now.

The improvements/additions are:

Cells are vertically centered now.
Copy/paste includes the header text.  
``` c#
ClipboardCopyMode="IncludeHeader"
```  

Templates cannot be copied/pasted. The DataGrid does not know what property it has to read. Therefore a ClipboardContentBinding was added.  

``` c#
ClipboardContentBinding="{Binding Birthday}
```   

A yellow smiley is drawn on a Canvas with ellipses and a Bézier curve.
The birthday string is formatted.
The DataGrid rows use alternating colors.
CheckBoxes are centered in the cells.
A bit closer to hardcore: DatePicker
The method to remove all borders requires slightly more know-how. The required information was taken from my earlier post WPF Control Templates (part 1). Also the background color of the DatePickerTextBox is made transparent. This is done without defining a new template for the DatePicker.
The XAML definition

``` XAML
<DatePicker SelectedDate="{Binding Birthday, UpdateSourceTrigger=PropertyChanged, Mode=TwoWay}"  BorderThickness="0" Loaded="DataGrid_DatePicker_Loaded" />
```

calls:

``` c#
private void DataGrid_DatePicker_Loaded(object sender, RoutedEventArgs e) {...}
```

which in turn calls:

```
private static void RemoveBorders(DependencyObject xDependencyObject) {...}
```

RowHeaders were added. On the internet you can find a lot of ToggleButton examples. You face the same code roots over and over again. To avoid coming up with a similar example I used a button with a +/- sign instead. This way you can easily change the code and replace the text by custom images.

My advice here is: Play with the `FrozenColumnCount` property. I am sure you will need it someday.

This example uses more templates than the last one.

RowDetailsTemplate was added. This enables expanding DataGrid rows to eg. show or enter additional information.

`UpdateSourceTrigger` makes sure you can see DataGridCell changes immediately on the RowDetailsTemplate.

To achieve this, the class Person needs to inherit `INotifyPropertyChanged`.