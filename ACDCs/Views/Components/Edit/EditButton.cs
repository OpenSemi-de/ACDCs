using System;
using System.Threading.Tasks;
using Microsoft.Maui;
using Sharp.UI;
using Button = Sharp.UI.Button;


namespace ACDCs.Views.Components.Edit
{
    public class EditButton : Button
    {
        public EditButton(Action onClickAction, Action<EditButton> onSelectAction)
        {
            IsSelected = false;

            this.Margin(new Thickness(0))
                .Padding(new Thickness(2))
                .CornerRadius(1)
                .WidthRequest(60)
                .HeightRequest(60)
                .BackgroundColor(BackgroundColor.WithAlpha(0.2f))
                .Text(onClickAction.Method.Name);

            _onClickAction = onClickAction;
            _onSelectAction = onSelectAction;
            Clicked += OnClicked;
            Loaded += OnLoaded;
            ButtonContentLayout layout = new(ButtonContentLayout.ImagePosition.Right, -10f);
            this.ContentLayout(layout);
        }



        public bool IsSelected { get; set; }

        private void OnLoaded(object? sender, EventArgs e)
        {
            //   ImageSource = ImageService.BackgroundImageSource(60, 60);
        }

        private readonly Action _onClickAction;
        private readonly Action<EditButton> _onSelectAction;

        private void OnClicked(object? sender, EventArgs e)
        {
            if (IsSelected)
            {
                Deselect();
            }
            else
            {
                Select();
            }

            _onClickAction.Invoke();
        }

        public void Select()
        {
            _onSelectAction.Invoke(this);
            IsSelected = true;
            this.BackgroundColor(BackgroundColor.WithAlpha(1f));
        }

        public async void Deselect()
        {
            IsSelected = false;


            await Task.Delay(200);
            this.BackgroundColor(BackgroundColor.WithAlpha(0.2f));

        }
    }
}
