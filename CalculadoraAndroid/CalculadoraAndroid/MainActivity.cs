using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;

namespace CalculadoraAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private TextView calculatorText;

        private string[] numbers = new string[2];
        private string @operator;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            calculatorText = FindViewById<TextView>(Resource.Id.calculator_text_view);
        }

        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(ViewAnimator v)
        {
            Button button = (Button)v;
            if ("0123456789.".Contains(button.Text))
                AddNumberOrDecimalPoint(button.Text);
            else if ("÷×+-".Contains(button.Text))
                AddOperator(button.Text);
            else if ("=" == button.Text)
                Calculate();
            else
                Erase();
        }

        private void Erase()
        {
            throw new NotImplementedException();
        }

        private void Calculate()
        {
            throw new NotImplementedException();
        }

        private void AddOperator(string text)
        {
            throw new NotImplementedException();
        }

        private void AddNumberOrDecimalPoint(string text)
        {
            throw new NotImplementedException();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}