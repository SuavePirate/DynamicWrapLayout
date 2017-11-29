# DynamicWrapLayout
A Xamarin.Forms layout for creating dynamically wrapped views. Inspired by the `WrapLayout` example: https://developer.xamarin.com/samples/xamarin-forms/UserInterface/CustomLayout/WrapLayout/

## Installation

It's on NuGet! https://www.nuget.org/packages/DynamicWrapLayout/2017.11.30
```
Install-Package DynamicWrapLayout
```

Be sure to install in all projects that use it.

## Usage

There are two key properties that make this control useful - the `ItemsSource` (like a `ListView`) and the `DataTemplate` (although, you can also just add children to the view - it does both!)
Be sure to wrap it in a `ScrollView` though

**XAML**

Add the `xmlns`:

```
xmlns:suave="clr-namespace:SuaveControls.DynamicWrapLayout;assembly=SuaveControls.DynamicWrapLayout"
```

Use it in your View:
```
<ScrollView>
    <suave:DynamicWrapLayout ItemsSource="{Binding Items}" HorizontalOptions="Fill">
        <suave:DynamicWrapLayout.ItemTemplate>
            <DataTemplate>
                <StackLayout BackgroundColor="Gray" WidthRequest="120" HeightRequest="180">
                    <Label Text="{Binding .}" TextColor="White" VerticalOptions="FillAndExpand" HorizontalOptions="FillAndExpand" VerticalTextAlignment="Center" HorizontalTextAlignment="Center" />
                </StackLayout>
            </DataTemplate>
        </suave:DynamicWrapLayout.ItemTemplate>
    </suave:DynamicWrapLayout>
</ScrollView>
```

Don't like data-binding and want to just use child views? You can do that too!

<ScrollView>
    <suave:DynamicWrapLayout HorizontalOptions="Fill">
      <StackLayout BackgroundColor="Gray" WidthRequest="120" HeightRequest="180">
          <Label Text="0" TextColor="White" VerticalOptions="FillAndExpand" HorizontalOptions="FillAndExpand" VerticalTextAlignment="Center" HorizontalTextAlignment="Center" />
      </StackLayout>
      <StackLayout BackgroundColor="Gray" WidthRequest="120" HeightRequest="180">
          <Label Text="1" TextColor="White" VerticalOptions="FillAndExpand" HorizontalOptions="FillAndExpand" VerticalTextAlignment="Center" HorizontalTextAlignment="Center" />
      </StackLayout>
      <StackLayout BackgroundColor="Gray" WidthRequest="120" HeightRequest="180">
          <Label Text="2" TextColor="White" VerticalOptions="FillAndExpand" HorizontalOptions="FillAndExpand" VerticalTextAlignment="Center" HorizontalTextAlignment="Center" />
      </StackLayout>
      <StackLayout BackgroundColor="Gray" WidthRequest="120" HeightRequest="180">
          <Label Text="3" TextColor="White" VerticalOptions="FillAndExpand" HorizontalOptions="FillAndExpand" VerticalTextAlignment="Center" HorizontalTextAlignment="Center" />
      </StackLayout>
      <StackLayout BackgroundColor="Gray" WidthRequest="120" HeightRequest="180">
          <Label Text="4" TextColor="White" VerticalOptions="FillAndExpand" HorizontalOptions="FillAndExpand" VerticalTextAlignment="Center" HorizontalTextAlignment="Center" />
      </StackLayout>
    </suave:DynamicWrapLayout>
</ScrollView>

## Features

- Bindable child views
- Bindable to collections
- Handles layout changing well (try rotating the device)
- Doesn't require custom renderers (All Xamarin.Forms baby!)

## What does this thing look like?

Android:

<img src="https://i0.wp.com/alexdunndev.files.wordpress.com/2017/11/screen-shot-2017-11-29-at-11-11-08-am.png" width="200"/>
<img src="https://i1.wp.com/alexdunndev.files.wordpress.com/2017/11/screen-shot-2017-11-29-at-11-11-19-am.png" height="200"/>

iOS:

<img src="https://i1.wp.com/alexdunndev.files.wordpress.com/2017/11/screen-shot-2017-11-29-at-11-08-44-am.png" width="200"/>
<img src="https://i2.wp.com/alexdunndev.files.wordpress.com/2017/11/screen-shot-2017-11-29-at-11-09-09-am.png" height="200"/>


## Notes
This does not use any native view virtualization, which means performance does not scale well with extremely large data sets.

## Coming soon

- `ItemSelected` event and `SelectedItem` bindable property (for now, you can add custom gestures and commands to your `DataTemplate` and handle the events yourself)
